import { CreationOptional, DataTypes, InferAttributes, InferCreationAttributes, Model, NonAttribute, Sequelize } from "sequelize";
import { TaskDefinition } from "./taskDefinition.model";

export class TaskInstance extends Model<InferAttributes<TaskInstance>, InferCreationAttributes<TaskInstance>> {
    declare id: CreationOptional<number>;
    declare uuid: CreationOptional<string>;

    declare status: number | null;
    declare instanceDate: Date;

    declare taskDefinitionId: number;
    declare taskDefinition: NonAttribute<TaskDefinition>;

    declare createdAt: Date;
}

export const taskInstanceInit = function(sequelize: Sequelize) {
    TaskInstance.init({
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
        status: {
            type: DataTypes.INTEGER,
            allowNull: true
        },
        instanceDate: {
            type: DataTypes.DATE,
            allowNull: false
        },
        taskDefinitionId: {
            type: DataTypes.INTEGER,
            allowNull: false
        },
        createdAt: {
            type: DataTypes.DATE,
            allowNull: false
        }
    }, {
        sequelize,
        paranoid: true
    });
}