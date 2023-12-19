import dayjs from 'dayjs';
import rrule from 'rrule';
const { RRule } = rrule;

import { taskRepository, taskStatus, MAX_DATE } from './taskRepository.js';

export const handler = async (req, context) => {
    const routes = [
        {
            path: /\/task-definitions/,
            method: 'GET',
            action: getAll
        },
        {
            path: /\/tasks/,
            method: 'GET',
            action: getTasks
        },
        {
            path: /\/task-definitions\/[a-z0-9\-]+\/create/,
            method: 'POST',
            action: createTask
        },
        {
            path: /\/tasks\/[a-z0-9\-]+\/status\/?/,
            method: 'PUT',
            action: updateTaskStatus
        },
        {
            path: /\/task-definitions\/?/,
            method: 'POST',
            action: createTaskDefinition
        }
    ];

    console.log("Request:", req);

    for (let route of routes) {
        if (req.httpMethod == route.method
            && route.path.test(req.path)
        ) {
            console.log("Executing Handler for path " + route.path);
            try {
                return route.action(req, context);
            }
            catch (ex) {
                console.log("Error: " + ex.toString());
                return {
                    statusCode: 500,
                    body: JSON.stringify({
                        message: ex.toString(),
                        stackTrace: ex.stack
                    })
                };
            }
        }
    }

    return {
        statusCode: 404,
        body: JSON.stringify({
            message: `Could not find handler for action ${req.httpMethod} ${req.path}.`
        })
    };
};

async function getAll(request, context) {
    console.log("Executing task-definition handler");
    return ok(
        taskRepository.getDefinitions()
    );
}

function getTasks(request, context) {
    const startDateString = request.queryStringParameters?.["startDate"];
    const endDateString = request.queryStringParameters?.["endDate"];

    if (!startDateString || !dayjs(startDateString).isValid())
        return badRequest("startDate is missing or invalid.");
    if (!endDateString || !dayjs(endDateString).isValid())
        return badRequest("endDate is missing or invalid.");

    const startDate = dayjs(startDateString);
    const endDate = dayjs(endDateString);

    const schedules = taskRepository.getDefinitionSchedules(startDate.toDate(), endDate.toDate());
    console.log("Found schedules: " + schedules);

    let allConcreteInstances = taskRepository.getTasks(startDate, endDate);

    let allTasks = [];

    for (let schedule of schedules) {
        // Construct and modify the options first, because the RRule object is immutable
        // https://github.com/jkbrzt/rrule#rruleparsestringrfcstring
        let options = RRule.parseString(schedule.rrule);
        options.dtstart = schedule.activeStartDate;
        options.until = schedule.activeEndDate;
        let rule = new RRule(options);

        let nextInstanceDate = schedule.activeStartDate;

        let lastIncompleteTask = taskRepository.getLastIncompleteTask(schedule.taskDefinitionId);
        if (lastIncompleteTask == null) {
            let lastCompletedTask = taskRepository.getLastCompletedTask(schedule.taskDefinitionId);
            if (lastCompletedTask !== null) {
                console.log("Last completed task", lastCompletedTask.instanceDate);
                nextInstanceDate = rule.after(lastCompletedTask.instanceDate)
            }
        }

        let concreteInstances = allConcreteInstances.filter(x =>
            x.definition.id == schedule.taskDefinitionId
            && (lastIncompleteTask == null || x.id != lastIncompleteTask.id)
        );

        let allInstances = concreteInstances;
        // If there is a pending incomplete task, don't add new tasks to the board
        if (lastIncompleteTask !== null)
            allInstances = [
                ...allInstances,
                lastIncompleteTask
            ];
        // If the next virtual instance is within the requested window, return a virtual task instance
        else if (nextInstanceDate < endDate)
            allInstances = [
                ...concreteInstances,
                {
                    id: null,
                    definition: taskRepository.getDefinition(schedule.taskDefinitionId),
                    instanceDate: nextInstanceDate,
                    // If the instance date is in the past
                    status: getStatus(null, nextInstanceDate)
                }
            ];

        allInstances = allInstances.map(x => ({
            ...x,
            status: getStatus(x.status, x.instanceDate)
        }));

        allTasks = [
            ...allTasks,
            ...allInstances
        ];
    }

    return ok(allTasks);
}

function getStatus(status, instanceDate) {
    return status !== null ? status : 
        instanceDate <= new Date()
            ? taskStatus.todo
            : taskStatus.upcoming
}


function createTask(request, context) {

    const definitionId = request.pathParameters.id;
    const definition = taskRepository.getDefinition(definitionId);

    const data = JSON.parse(request.body);

    if (definition === null)
        return notFound("Could not find task definition with id " + definitionId);

    if (!data.instanceDate || !dayjs(data.instanceDate).isValid())
        return badRequest("Missing instanceDate property.");

    const task = {
        definition: definition,
        instanceDate: new Date(data.instanceDate),
        status: data.status ?? null
    };

    let newTask = taskRepository.createTask(task);

    newTask = {
        ...newTask,
        status: getStatus(newTask.status, newTask.instanceDate)
    };

    return ok(newTask);
}

function updateTaskStatus(request, context) {
    const taskId = request.pathParameters.id;

    const data = JSON.parse(request.body);

    const oldTask = taskRepository.getTask(taskId);
    if (oldTask == null) {
        return notFound("Could not find a task with id " + taskId);
    }

    const newTask = {
        ...oldTask,
        status: data.status
    };

    taskRepository.updateTaskStatus(taskId, newTask.status);

    return ok(newTask);
}

function createTaskDefinition(request, context) {
    console.log("Body", JSON.parse(request.body));
    const { shortDescription,
        description,
        startDate,
        profileId,
        doesRepeat,
        interval,
        daysOfWeek,
        frequency,
        count,
        until
    } = JSON.parse(request.body);

    let schedules = [];
    if (doesRepeat) {
        const options = {
            freq: frequency,
            dtstart: startDate ? new Date(startDate) : new Date()
        };
    
        if (count)
            options.count = count;
        else
            options.until = until ? new Date(until) : null;
    
        const rule = new RRule(options);
        const ruleString = RRule.optionsToString({
            freq: rule.options.freq,
            interval: rule.options.interval,
            count: rule.options.count,
            bymonthday: rule.options.bymonthday,
            byhour: rule.options.byhour,
            byminute: rule.options.byminute
        });

        schedules.push({
            activeStartDate: rule.options.dtstart,
            activeEndDate: rule.options.until ?? MAX_DATE,
            rrule: ruleString
        });
    }

    const definition = {
        shortDescription: shortDescription,
        description: description,
        schedules: schedules
    };

    const newDefinition = taskRepository.createTaskDefinition(definition);

    return ok(newDefinition);
}

function badRequest(message) {
    return {
        statusCode: 400,
        body: message ?
            JSON.stringify({ message: message })
            : ""
    };
}

function ok(body) {
    return {
        statusCode: 200,
        body: JSON.stringify(body),
        headers: {
            "Content-Type": "application/json"
        }
    }
}

function notFound(message) {
    return {
        statusCode: 404,
        body: message ?
            JSON.stringify({ message: message })
            : ""
    }
}