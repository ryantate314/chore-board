import { Router, Request, Response } from 'express';
import { body, param, validationResult } from 'express-validator';
import { taskRepository } from '../data/task.repository';
import { notFound } from './route-utils';
import dayjs from 'dayjs';
import { Task } from '../models/task.model';
import { taskService } from '../services/task.service';
import { CreateTaskDefinitionDto, Frequency } from '../models/createTaskDefinitionDto.model';
import { taskDefinitionService } from '../services/task-definition.service';

const router = Router();

// Get all task definitions
router.get("/", (request, response) => {
    const tasks = taskRepository.getDefinitions();

    response.status(200)
        .json(tasks);
});

// Create Task Definition
const createTaskDefinitionValidators = [
    body("shortDescription")
        .exists()
        .isLength({
            max: 64
        }),
    body("description")
        .optional()
        .isLength({
            max: 256
        }),
    body("startDate")
        .exists()
        .isISO8601()
        .toDate(),
    body("doesRepeat")
        .exists()
        .isBoolean()
        .toBoolean(),
    body("interval")
        .optional()
        .isInt(),
    body("daysOfWeek")
        .optional()
        .isArray(),
    body("frequency")
        .optional()
        .isIn(Object.values(Frequency)),
    body("count")
        .optional()
        .isInt({
            gt: 0
        }),
    body("until")
        .optional()
        .isISO8601()
        .toDate()
];
router.post("/", createTaskDefinitionValidators, (req: Request, res: Response) => {
    const errors = validationResult(req);

    if (!errors.isEmpty())
        return res.status(400)
            .json({ errors: errors.array() });

    const dto = <CreateTaskDefinitionDto>req.body;

    const definition = taskDefinitionService.createTaskDefinition(dto);

    return res.status(200)
        .json(definition);
});

// Create task for definition
const createTaskValidationRules = [
    param("taskDefinitionId")
        .isUUID()
            .withMessage("Invalid task definition id."),
    body("instanceDate")
        .exists()
            .withMessage("instanceDate is required.")
        .isISO8601()
            .withMessage("instanceDate must be a valid date string.")
];
router.post("/:taskDefinitionId/", createTaskValidationRules, (req: Request, res: Response) => {
    const errors = validationResult(req);

    if (!errors.isEmpty())
        return res.status(400)
            .json({ errors: errors.array() });

    const definitionId = req.params.taskDefinitionId;
    const definition = taskRepository.getDefinition(definitionId);

    if (definition === null)
        return notFound(res, "Could not find task definition with id " + definitionId);

    const task: Task = {
        id: null,
        definition: definition,
        instanceDate: new Date(req.body.instanceDate),
        status: req.body.status ?? null,
        createdAt: new Date()
    };

    const newTask = taskService.createTask(task);

    return res.status(200)
        .json(newTask);
});

export default router;