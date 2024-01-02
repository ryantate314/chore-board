import { Response } from "express";

export function notFound(res: Response, message: string) {
    return res.status(404)
        .json({
            message: message
        });
}