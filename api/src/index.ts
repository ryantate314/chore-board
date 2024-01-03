// Must run this first because some of the imported objects use the environment variables
import dotenv from 'dotenv';
dotenv.config();

import express, { Request, Response } from "express";

import taskDefinitionRoutes from "./routes/task-definitions.route";
import taskRoutes from "./routes/tasks.route";
import cors from "cors";



const app = express();
app.use(express.json());
app.use(cors());

const port = process.env.PORT || 3001;

app.get("/", (req: Request, res: Response) => {
  res.send("Hello, TypeScript Express!");
});

app.use('/task-definitions', taskDefinitionRoutes);
app.use('/tasks', taskRoutes);

app.listen(port, () => {
  console.log(`Server running at http://localhost:${port}`);
});
