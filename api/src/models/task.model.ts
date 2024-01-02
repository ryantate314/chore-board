import { TaskDefinition } from "./taskDefinition.model";
import { TaskStatus } from "./taskStatus";

export interface Task {
    id: string | null;
    definition: TaskDefinition;
    createdAt: Date | null;
    instanceDate: Date;
    status: TaskStatus | null;
}