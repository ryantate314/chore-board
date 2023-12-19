import express from 'express'
import { router } from './router.js';
import cors from 'cors'
import nocache from 'nocache';



const app = express()

app.use(express.json());
app.use(cors({
    exposedHeaders: ["Content-Type"]
}));
app.use(nocache());

router(app);

app.listen(3001)

console.log("App is listening on port 3001");