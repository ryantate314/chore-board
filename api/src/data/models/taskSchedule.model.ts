import { CreationOptional, DataTypes, InferAttributes, InferCreationAttributes, Model, NonAttribute, Sequelize } from "sequelize";
import { TaskDefinition } from "./taskDefinition.model";

export class TaskSchedule extends Model<InferAttributes<TaskSchedule>, InferCreationAttributes<TaskSchedule>> {
    declare id: CreationOptional<number>;
    declare startDate: Date;
    declare endDate: CreationOptional<Date>;
    declare rrule: CreationOptional<string>;

    declare taskDefinitionId: number;
    declare taskDefinition: NonAttribute<TaskDefinition>;
}

export const taskScheduleInit = function(sequelize: Sequelize) {
    TaskSchedule.init({
        id: {
            type: DataTypes.INTEGER,
            primaryKey: true,
            autoIncrement: true
        },
        startDate: {
            type: DataTypes.DATE,
            allowNull: false
        },
        endDate: {
            type: DataTypes.DATE,
            allowNull: true
        },
        rrule: {
            type: DataTypes.STRING
        },
        taskDefinitionId: {
            type: DataTypes.INTEGER,
            allowNull: false
        }
    }, {
        sequelize
    });
}