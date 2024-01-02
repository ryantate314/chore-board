import { Options as RRuleOptions, Frequency as RRuleFrequency, RRule } from "rrule";
import { CreateTaskDefinitionDto } from "../models/createTaskDefinitionDto.model";
import { Schedule, TaskDefinition } from "../models/taskDefinition.model";
import { taskRepository } from "../data/task.repository";

export const MAX_DATE = new Date("9999-12-31T23:59:59.999Z");

export const taskDefinitionService = {
    createTaskDefinition: function(dto: CreateTaskDefinitionDto): TaskDefinition {
        let schedules: Schedule[] = [];
        if (dto.doesRepeat) {
            const options: Partial<RRuleOptions> = {
                freq: <RRuleFrequency>(<unknown>dto.frequency),
                dtstart: dto.startDate ? new Date(dto.startDate) : new Date()
            };
        
            if (dto.count !== null)
                options.count = dto.count;
            else
                options.until = dto.until ? new Date(dto.until) : null;

            // TODO: handle days of week
            
        
            const rule = new RRule(options);
            const ruleString = RRule.optionsToString({
                freq: rule.options.freq,
                interval: rule.options.interval,
                count: rule.options.count,
                bymonthday: rule.options.bymonthday,
                byhour: rule.options.byhour,
                byminute: rule.options.byminute
            });

            schedules.push({
                taskDefinitionId: null,
                activeStartDate: rule.options.dtstart,
                activeEndDate: rule.options.until ?? MAX_DATE,
                rrule: ruleString
            });
        }

        const definition: TaskDefinition = {
            id: null,
            shortDescription: dto.shortDescription,
            description: dto.description,
            schedules: schedules
        };

        const newDefinition = taskRepository.createTaskDefinition(definition);

        return newDefinition;
    }
};