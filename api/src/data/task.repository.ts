
import { v4 as uuid } from 'uuid';
import { TaskStatus } from '../models/taskStatus';
import { Task } from '../models/task.model';
import { TaskDefinition } from '../models/taskDefinition.model';
import * as Models from './models';
import { Op } from 'sequelize';

export const MAX_DATE = new Date("9999-12-31T23:59:59.999Z");

export const taskRepository = {
    getDefinitions: () => {
        return Models.TaskDefinition.findAll({
            include: Models.TaskSchedule
        });
    },
    getDefinitionSchedules: (startDate: Date, endDate: Date) => {
        return Models.TaskSchedule.findAll({
            where: {
                // Checking between dates: https://stackoverflow.com/a/325964
                startDate: {
                    [Op.lte]: endDate
                },
                endDate: {
                    [Op.gte]: startDate
                },
                '$TaskDefinition$.deletedAt$': null
            },
            include: Models.TaskDefinition
        });
    },
    getLastCompletedTask: (taskDefinitionId: string) => {
        return Models.TaskInstance.findOne({
            where: {
                taskDefinitionId: taskDefinitionId,
                status: TaskStatus.Complete
            },
            order: [
                ['instanceDate', 'DESC']
            ],
            include: Models.TaskDefinition
        });
    },
    getLastIncompleteTask: (taskDefinitionId: string) => {
        return Models.TaskInstance.findOne({
            where: {
                status: {
                    [Op.or]: [
                        null,
                        { [Op.notIn]: [1, 2] }
                    ]
                },
                taskDefinitionId: taskDefinitionId
            },
            order: [
                ['instanceDate', 'DESC']
            ]
        });
    },
    getDefinition: (id: string) => {
        return Models.TaskDefinition.findByPk(id, {
            include: Models.TaskSchedule
        });
    },
    getTasks: (startDate: Date, endDate: Date) => {
        return Models.TaskInstance.findAll({
            where: {
                instanceDate: {
                    [Op.gte]: startDate,
                    [Op.lt]: endDate
                }
            },
            include: Models.TaskDefinition
        });
    },
    getTask: (id: string) => {
        return Models.TaskInstance.findByPk(id, {
            include: Models.TaskDefinition
        });
    },
    createTask: (task: Task) => {
        Models.TaskInstance.create({
            instanceDate: task.instanceDate,
            taskDefinitionId: task.definition.id
        })
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