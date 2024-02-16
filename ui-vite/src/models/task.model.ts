export interface FamilyMember {
    id: string;
    name: string;
}

export interface TaskDefinition {
    id: string;
    shortDescription: string;
    description: string;
}

export interface TaskInstance {
    id: string;
    definition: TaskDefinition;
    dueDate: Date | null;
    status: TaskStatus;
    instanceDate: Date;
    createdAt: Date;
    completedAt: Date | null;
}

export enum TaskStatus {
    Upcoming = "U",
    ToDo = "T",
    InProgress = "I",
    Done = "C",
    Deleted = "D"
}