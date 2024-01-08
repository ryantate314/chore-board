import { Router, Request, Response } from 'express';
import { query, body, validationResult, param } from 'express-validator';
import { taskService } from '../services/task.service';
import { Task } from '../models/task.model';
import { RRule } from 'rrule';
import dayjs from 'dayjs';
import { TaskStatus } from '../models/taskStatus';

const router = Router();


const getTasksValidationRules = [
    query('startDate')
        .notEmpty()
            .withMessage("startDate is required.")
        .isISO8601()
            .withMessage("startDate is must be a valid date.")
        .toDate(),
    query('endDate')
        .notEmpty()
            .withMessage("endDate is required.")
        .isISO8601()
            .withMessage("endDate is must be a valid date.")
        .toDate(),
];
router.get("/", getTasksValidationRules, async (req: Request, res: Response) => {

    const errors = validationResult(req);

    console.log(req.query);

    if (!errors.isEmpty())
        return res.status(400)
            .json({ errors: errors.array() });

    const startDate = new Date(req.query.startDate!.toString());
    const endDate = new Date(req.query.endDate!.toString());

    const tasks = await taskService.getTasks(startDate, endDate);

    res.status(200)
        .json(tasks);
});

const updateStatusValidators = [
    param("id")
        .exists()
        .isUUID(),
    body("newStatus")
        .exists()
        .isIn(Object.values(TaskStatus))
];
router.put("/:id/status", async (req: Request, res: Response) => {
    const id = req.params.id;
    const newStatus = req.body.newStatus;

    const newTask = await taskService.updateStatus(id, newStatus);

    res.status(200)
        .json(newTask);
});

export default router;