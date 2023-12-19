import { TaskDefinition, TaskInstance, TaskStatus } from '../models/task.model';


const API_URL = "http://localhost:3001";

const TaskService = {
    updateStatus: function(taskId: string, newStatus: TaskStatus): Promise<TaskInstance> {
        return fetch(`${API_URL}/tasks/${taskId}/status`, {
            method: 'PUT',
            body: JSON.stringify({
                status: newStatus
            }),
            headers: new Headers({"Content-Type": "application/json"})
        })
        .then(response => <Promise<TaskInstance>>response.json());
    },
    getTasks: function(): Promise<TaskInstance[]> {
        return fetch(`${API_URL}/tasks`)
            .then(response => response.json())
    },
    getTaskDefinitions: function(): Promise<TaskDefinition[]> {
        return fetch(`${API_URL}/task-definitions`)
            .then(response => response.json());
    }
}

export default TaskService;