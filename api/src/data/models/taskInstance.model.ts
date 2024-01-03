import { CreationOptional, DataTypes, InferAttributes, InferCreationAttributes, Model, NonAttribute, Sequelize } from "sequelize";
import { TaskDefinition } from "./taskDefinition.model";

export class TaskInstance extends Model<InferAttributes<TaskInstance>, InferCreationAttributes<TaskInstance>> {
    declare id: CreationOptional<number>;
    declare uuid: CreationOptional<string>;

    declare status: number | null;
    declare instanceDate: Date;

    declare taskDefinitionId: number;
    declare taskDefinition: NonAttribute<TaskDefinition>;
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
            type: DataTypes.NUMBER,
            allowNull: true
        },
        instanceDate: {
            type: DataTypes.DATE,
            allowNull: false
        },
        taskDefinitionId: {
            type: DataTypes.NUMBER,
            allowNull: false
        }
    }, {
        sequelize,
        paranoid: true
    });

    TaskDefinition.hasMany(TaskInstance);
    TaskInstance.belongsTo(TaskDefinition, {
        foreignKey: "taskDefinitionId"
    });
}