export interface TaskDefinition {
    id: string | null;
    shortDescription: string;
    description: string | null;
    schedules: Schedule[];
}

export interface Schedule {
    taskDefinitionId: string | null;
    activeEndDate: Date;
    activeStartDate: Date;
    rrule: string;
}