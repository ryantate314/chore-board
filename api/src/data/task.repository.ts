
import { v4 as uuid } from 'uuid';
import { TaskStatus } from '../models/taskStatus';
import { Task } from '../models/task.model';
import { Schedule, TaskDefinition } from '../models/taskDefinition.model';
import * as Models from './models';
import { Op } from 'sequelize';

export const MAX_DATE = new Date("9999-12-31T23:59:59.999Z");

function mapStatusToModel(status: TaskStatus): number {
    switch (status) {
        case TaskStatus.Upcoming:
            return 1;
        case TaskStatus.Todo:
            return 2;
        case TaskStatus.Complete:
            return 3;
        case TaskStatus.Deleted:
            return 4;
        default:
            throw new Error("Invalid task status.");
    }
}

function mapStatusToDto(status: number | null): TaskStatus | null {
    switch (status) {
        case 1:
            return TaskStatus.Upcoming;
        case 2:
            return TaskStatus.Todo;
        case 3:
            return TaskStatus.Complete;
        case 4:
            return TaskStatus.Deleted;
        case null:
            return null;
        default:
            throw new Error("Invalid task status.");
    }
}

function mapTaskDefinitionToDto(definition: Models.TaskDefinition | null): TaskDefinition | null {
    if (definition == null)
        return null;

    return {
        id: definition.uuid,
        description: definition.description,
        schedules: definition.schedules.map(x => mapScheduleToDto(definition.uuid, x)),
        shortDescription: definition.shortDescription
    };
}

function mapScheduleToDto(taskDefinitionId: string, schedule: Models.TaskSchedule): Schedule {
    return {
        activeEndDate: schedule.endDate,
        activeStartDate: schedule.startDate,
        rrule: schedule.rrule,
        taskDefinitionId: taskDefinitionId
    };
}

function mapTaskToDto(task: Models.TaskInstance | null) : Task | null {
    if (task === null)
        return null;

    return {
        createdAt: task.createdAt,
        definition: mapTaskDefinitionToDto(task.taskDefinition)!,
        id: task.uuid,
        instanceDate: task.instanceDate,
        status: mapStatusToDto(task.status)
    }
}

function _getDefinition(id: string): Promise<Models.TaskDefinition | null> {
    return Models.TaskDefinition.findOne({
        where: {
            uuid: id
        },
        include: Models.TaskSchedule
    });

    
};

export const taskRepository = {
    getDefinitions: async (): Promise<TaskDefinition[]> => {
        const definitions = await Models.TaskDefinition.findAll({
            include: Models.TaskSchedule
        });

        return definitions.map(x => mapTaskDefinitionToDto(x)!);
    },
    getDefinitionSchedules: async (startDate: Date, endDate: Date): Promise<Schedule[]> => {
        const schedules = await Models.TaskSchedule.findAll({
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

        return schedules.map(x => mapScheduleToDto(x.taskDefinition.uuid, x));
    },
    getLastCompletedTask: async (taskDefinitionId: string): Promise<Task | null> => {
        const task = await Models.TaskInstance.findOne({
            where: {
                taskDefinitionId: taskDefinitionId,
                status: TaskStatus.Complete
            },
            order: [
                ['instanceDate', 'DESC']
            ],
            include: Models.TaskDefinition
        });

        return mapTaskToDto(task);
    },
    getLastIncompleteTask: async (taskDefinitionId: string): Promise<Task | null> => {
        const task = await Models.TaskInstance.findOne({
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

        return mapTaskToDto(task);
    },
    getDefinition: async (id: string): Promise<TaskDefinition | null> => {
        const definition = await _getDefinition(id);

        return mapTaskDefinitionToDto(definition);
    },
    getTasks: async (startDate: Date, endDate: Date): Promise<Task[]> => {
        const tasks = await Models.TaskInstance.findAll({
            where: {
                instanceDate: {
                    [Op.gte]: startDate,
                    [Op.lt]: endDate
                }
            },
            include: Models.TaskDefinition
        });

        return tasks.map(x => mapTaskToDto(x)!);
    },
    getTask: async (id: string): Promise<Task | null> => {
        const task = await Models.TaskInstance.findOne({
            where: {
                uuid: id
            },
            include: Models.TaskDefinition
        });
        return mapTaskToDto(task);
    },
    createTask: async (task: Task): Promise<Task> => {
        const definition = await _getDefinition(task.definition.id!);
        if (!definition)
            throw new Error("Task definition does note exist with id " + task.definition?.id);

        const newTask = await Models.TaskInstance.create({
            instanceDate: task.instanceDate,
            taskDefinitionId: definition.id!,
            createdAt: new Date()
        });
        
        return mapTaskToDto(newTask)!;
    },
    updateTaskStatus: async (taskId: string, status: TaskStatus) => {
        await Models.TaskInstance.update({
            status: mapStatusToModel(status)
        }, {
            where: {
                uuid: taskId
            }
        });
    },
    createTaskDefinition: async (definition: TaskDefinition): Promise<TaskDefinition> => {
        const newDefinition = await Models.TaskDefinition.create({
            shortDescription: definition.shortDescription,
            description: definition.description,
        }, {
            include: { 
                model: Models.TaskSchedule,
                as: 'schedules'
            }
        });
        return mapTaskDefinitionToDto(newDefinition)!;
    }
};