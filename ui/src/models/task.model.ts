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
    createdAt: Date;
    completedAt: Date | null;
}

export enum TaskStatus {
    Upcoming = 1,
    ToDo = 2,
    InProgress = 3,
    Done = 4
}