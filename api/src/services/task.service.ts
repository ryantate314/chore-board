import { Task } from "../models/task.model";

import { taskRepository }  from "../data/task.repository";
import { TaskStatus } from "../models/taskStatus";
import { RRule } from "rrule";

function getStatus(status: TaskStatus | null, instanceDate : Date) {
    return status !== null ? status : 
        instanceDate <= new Date()
            ? TaskStatus.Todo
            : TaskStatus.Upcoming
}

export const taskService = {
    createTask: function(task: Task): Task {
        let newTask = taskRepository.createTask(task);

        newTask = {
            ...newTask,
            status: getStatus(newTask.status, newTask.instanceDate)
        };

        return newTask;
    },
    getTasks: function(startDate: Date, endDate: Date): Task[] {
        const schedules = taskRepository.getDefinitionSchedules(startDate, endDate);
        console.log("Found schedules: " + schedules);
    
        let allConcreteInstances = taskRepository.getTasks(startDate, endDate);
    
        let allTasks: Task[] = [];
    
        for (let schedule of schedules) {
            // Construct and modify the options first, because the RRule object is immutable
            // https://github.com/jkbrzt/rrule#rruleparsestringrfcstring
            let options = RRule.parseString(schedule.rrule);
            options.dtstart = schedule.activeStartDate;
            options.until = schedule.activeEndDate;
            let rule = new RRule(options);
    
            let nextInstanceDate: Date | null = schedule.activeStartDate;
    
            let lastIncompleteTask = taskRepository.getLastIncompleteTask(schedule.taskDefinitionId!);
            if (lastIncompleteTask == null) {
                let lastCompletedTask = taskRepository.getLastCompletedTask(schedule.taskDefinitionId!);
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
            else if (nextInstanceDate != null && nextInstanceDate < endDate)
                allInstances = [
                    ...concreteInstances,
                    {
                        id: null,
                        definition: taskRepository.getDefinition(schedule.taskDefinitionId!),
                        instanceDate: nextInstanceDate,
                        // If the instance date is in the past
                        status: getStatus(null, nextInstanceDate),
                        createdAt: null
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

        return allTasks;
    },
    updateStatus: function(id: string, newStatus: TaskStatus): Task {
        const oldTask = taskRepository.getTask(id);
        if (oldTask == null) {
            throw new Error("Could not find task with id " + id);
        }
    
        const newTask = {
            ...oldTask,
            status: newStatus
        };
    
        taskRepository.updateTaskStatus(id, newTask.status);

        return newTask;
    }
}