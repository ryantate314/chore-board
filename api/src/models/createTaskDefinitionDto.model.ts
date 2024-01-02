export interface CreateTaskDefinitionDto {
    shortDescription: string;
    description: string;
    startDate: Date;
    profileId: string;
    doesRepeat: boolean;
    interval: number;
    daysOfWeek: string[];
    frequency: Frequency;
    count: number | null;
    until: Date | null;
}

export enum Frequency {
    Daily = 3,
    Weekly = 2,
    Monthly = 1,
    Yearly = 0
}