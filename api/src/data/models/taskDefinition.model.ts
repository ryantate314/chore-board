import { CreationOptional, DataTypes, InferAttributes, InferCreationAttributes, Model, NonAttribute, Sequelize } from "sequelize";
import { TaskSchedule } from "./taskSchedule.model";

export class TaskDefinition extends Model<InferAttributes<TaskDefinition>, InferCreationAttributes<TaskDefinition>> {
    declare id: CreationOptional<number>;
    declare uuid: CreationOptional<string>;

    declare shortDescription: string;
    declare description: CreationOptional<string>;

    declare schedules: NonAttribute<TaskSchedule[]>;
}

export const init = function(sequelize: Sequelize) {
    TaskDefinition.init({
        id: {
            type: DataTypes.INTEGER,
            primaryKey: true,
            autoIncrement: true
        },
        uuid: {
            type: DataTypes.UUID,
            defaultValue: DataTypes.UUIDV4,
            allowNull: false,
            unique: true
        },
        shortDescription: {
            type: DataTypes.STRING(64),
            allowNull: false,
        },
        description: {
            type: DataTypes.STRING(255),
            allowNull: true
        }
    }, {
        sequelize,
        paranoid: true
    });

    
}