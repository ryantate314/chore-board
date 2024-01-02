export abstract class Profile {
    public abstract get name(): string;
    public abstract getNextInstance(): Date;
}

export class SlidingSchedule extends Profile {
    public get name() {
        return "SlidingSchedule";
    }

    public getNextInstance(): Date {
        throw new Error("Not Implemented");
    }
}

