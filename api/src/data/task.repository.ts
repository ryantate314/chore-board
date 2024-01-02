
import { v4 as uuid } from 'uuid';
import { TaskStatus } from '../models/taskStatus';
import { Task } from '../models/task.model';
import { TaskDefinition } from '../models/taskDefinition.model';

export const MAX_DATE = new Date("9999-12-31T23:59:59.999Z");

let definitions: TaskDefinition[] = [
    {
        id: "7b987da7-20da-48b7-90d3-8ee687707c56",
        shortDescription: "Take out the Trash",
        description: "",
        schedules: [
            {
                taskDefinitionId: "7b987da7-20da-48b7-90d3-8ee687707c56",
                activeEndDate: MAX_DATE,
                activeStartDate: new Date("2023-10-01T12:00:00.000Z"),
                rrule: "RRULE:FREQ=DAILY"
            }
        ]
    }
];

let tasks: Task[] = [
    {
        id: "bar",
        definition: definitions[0],
        createdAt: new Date("2023-10-09T14:12:00.000Z"),
        instanceDate: new Date("2023-10-10T12:00:00.000Z"),
        status: TaskStatus.Todo
    }
];

export const taskRepository = {
    getDefinitions: () => {
        return [...definitions];
    },
    getDefinitionSchedules: (startDate: Date, endDate: Date) => {
        return definitions.map(x => x.schedules)
            .reduce((all, schedules) => all.concat(schedules), [])
            .filter(x =>
                // https://stackoverflow.com/a/325964
                x.activeStartDate.getTime() <= endDate.getTime() && x.activeEndDate.getTime() >= startDate.getTime()
            );
    },
    getLastCompletedTask: (taskDefinitionId: string) => {
        return tasks.filter(x => x.definition.id == taskDefinitionId && x.status == TaskStatus.Complete)
            .sort((a, b) => a.instanceDate.getTime() - b.instanceDate.getTime())[0] ?? null;
    },
    getLastIncompleteTask: (taskDefinitionId: string) => {
        return tasks.filter(x => x.definition.id == taskDefinitionId
                && x.status != TaskStatus.Complete
                && x.status != TaskStatus.Deleted)
            .sort((a, b) => a.instanceDate.getTime() - b.instanceDate.getTime())
            [0] || null
    },
    getDefinition: (id: string) => {
        return definitions.filter(x => x.id == id)
            [0] ?? null;
    },
    getTasks: (startDate: Date, endDate: Date) => {
        return tasks.filter(x => x.instanceDate >= startDate && x.instanceDate < endDate);
    },
    getTask: (id: string) => {
        return tasks.filter(x => x.id == id)
            [0] ?? null
    },
    createTask: (task: Task) => {
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
    updateTaskStatus: (taskId: string, status: TaskStatus) => {
        tasks = tasks.map(x => x.id == taskId ?
            {
                ...x,
                status: status
            } : x);
    },
    createTaskDefinition: (definition: TaskDefinition) => {
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