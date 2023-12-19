
import { v4 as uuid } from 'uuid';

export const MAX_DATE = new Date("9999-12-31T23:59:59.999Z");

export const profiles = {
    default: "default"
};


let definitions = [
    {
        id: "foo",
        shortDescription: "Take out the Trash",
        schedules: [
            {
                taskDefinitionId: "foo",
                activeEndDate: MAX_DATE,
                activeStartDate: new Date("2023-10-01T12:00:00.000Z"),
                rrule: "RRULE:FREQ=DAILY"
            }
        ]
    }
];

let tasks = [
    {
        id: "bar",
        definition: definitions[0],
        createdAt: new Date("2023-10-09T14:12:00.000Z"),
        instanceDate: new Date("2023-10-10T12:00:00.000Z"),
        status: 'ToDo'
    }
];

export const taskStatus = {
    complete: "Done",
    deleted: "Deleted",
    todo: "ToDo",
    upcoming: "Upcoming"
};

export const taskRepository = {
    getDefinitions: () => {
        return [...definitions];
    },
    /**
     * @param {Date} startDate 
     * @param {Date} endDate 
     */
    getDefinitionSchedules: (startDate, endDate) => {
        return definitions.map(x => x.schedules)
            .reduce((all, schedules) => all.concat(schedules), [])
            .filter(x =>
                // https://stackoverflow.com/a/325964
                x.activeStartDate <= endDate.getTime() && x.activeEndDate >= startDate.getTime()
            );
    },
    getLastCompletedTask: (taskDefinitionId) => {
        return tasks.filter(x => x.definition.id == taskDefinitionId && x.status == taskStatus.complete)
            .sort((a, b) => a.instanceDate.getTime() - b.instanceDate.getTime())[0] ?? null;
    },
    getLastIncompleteTask: (taskDefinitionId) => {
        return tasks.filter(x => x.definition.id == taskDefinitionId
                && x.status != taskStatus.complete
                && x.status != taskStatus.deleted)
            .sort((a, b) => a.instanceDate.getTime() - b.instanceDate.getTime())
            [0] || null
    },
    getDefinition: (id) => {
        return definitions.filter(x => x.id == id)
            [0] ?? null;
    },
    getTasks: (startDate, endDate) => {
        return tasks.filter(x => x.instanceDate >= startDate && x.instanceDate < endDate);
    },
    getTask: (id) => {
        return tasks.filter(x => x.id == id)
            [0] ?? null
    },
    createTask: (task) => {
        task = {
            ...task,
            id: uuid(),
            createdAt: new Date()
        };

        tasks = [
            ...tasks,
            task
        ];

        return {
            ...task
        };
    },
    updateTaskStatus: (taskId, status) => {
        tasks = tasks.map(x => x.id == taskId ?
            {
                ...x,
                status: status
            } : x);
    },
    createTaskDefinition: (definition) => {
        const id = uuid();
        const newDefinition = {
            ...definition,
            schedules: definition.schedules.map(x => ({
                ...x,
                taskDefinitionId: id
            })),
            id: id,
            createdAt: new Date()
        };
        definitions = [
            ...definitions,
            newDefinition
        ];
        return {
            ...newDefinition
        }
    }
};