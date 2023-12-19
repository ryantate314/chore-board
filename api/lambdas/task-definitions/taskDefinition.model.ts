export interface TaskDefinition {
    id: string;
    schedules: TaskSchedule[];
    shortDescription: string;
}

export interface TaskSchedule {
    taskDefinitionId: string;
    
    rrule: string;

    activeStartDate: Date;
    activeEndDate: Date;
}

export interface TaskInstance {
    id: string;
    definition: TaskDefinition;
    createdAt: Date;
}

