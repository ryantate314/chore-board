import { Sequelize } from "sequelize";
import { TaskDefinition, init as taskDefinitionInit } from "./models/taskDefinition.model";
import { TaskInstance, taskInstanceInit } from "./models/taskInstance.model";
import { TaskSchedule, taskScheduleInit } from "./models/taskSchedule.model";

export default async function() {
    const sequelize = new Sequelize(
        process.env.DB_DATABASE!,
        process.env.DB_USERNAME!,
        process.env.DB_PASSWORD!,
        {
            dialect: 'mssql',
            dialectOptions: {
                options: {
                    useUTC: true,
                    dateFirst: 1
                }
            },
            define: {
                // Prevent pluralizing table names
                freezeTableName: true
            },
            logging: console.log
        }
    );
    
    await sequelize.authenticate();
    
    taskDefinitionInit(sequelize);
    taskInstanceInit(sequelize);
    taskScheduleInit(sequelize);

    TaskDefinition.hasMany(TaskSchedule, {
        foreignKey: 'taskDefinitionId'
    });
    TaskSchedule.belongsTo(TaskDefinition, {
        foreignKey: 'taskDefinitionId'
    });

    TaskDefinition.hasMany(TaskInstance, {
        foreignKey: 'taskDefinitionId'
    });
    TaskInstance.belongsTo(TaskDefinition, {
        foreignKey: "taskDefinitionId"
    });
    
    await sequelize.sync({
        logging: console.log,
        force: process.env.IS_DEVELOPMENT == "true"
    });
}

