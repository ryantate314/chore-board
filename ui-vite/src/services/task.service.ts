import { TaskDefinition, TaskInstance, TaskStatus } from '../models/task.model';


const API_URL = import.meta.env.VITE_API_URL;

const TaskService = {
    updateStatus: function(taskId: string, newStatus: TaskStatus, familyMemberId?: string): Promise<TaskInstance> {
        return fetch(`${API_URL}/tasks/${taskId}/status`, {
            method: 'PUT',
            body: JSON.stringify({
                newStatus: newStatus,
                familyMemberId: familyMemberId
            }),
            headers: new Headers({"Content-Type": "application/json"})
        })
        .then(response => <Promise<TaskInstance>>response.json());
    },
    getTasks: function(startDate: Date, endDate: Date): Promise<TaskInstance[]> {
        const url = `${API_URL}/tasks?startDate=${encodeURIComponent(startDate.toISOString())}&endDate=${encodeURIComponent(endDate.toISOString())}`;
        return fetch(url)
            .then(response => response.json())
    },
    getTaskDefinitions: function(): Promise<TaskDefinition[]> {
        return fetch(`${API_URL}/task-definitions`)
            .then(response => response.json());
    },
    createTask: function(definitionId: string, instanceDate: Date, status?: string, familyMemberId?: string) : Promise<TaskInstance> {
        const url = `${API_URL}/task-definitions/${encodeURIComponent(definitionId)}/tasks`;
        return fetch(url, {
            method: 'POST',
            body: JSON.stringify({
                instanceDate: instanceDate,
                status: status,
                familyMemberId: familyMemberId
            }),
            headers: new Headers({"Content-Type": "application/json"})
        }).then(response => response.json());
    },
    createTaskDefinition: function(taskDefinition: any) {
        const url = `${API_URL}/task-definitions/`;
        return fetch(url, {
            method: 'POST',
            body: JSON.stringify(taskDefinition),
            headers: new Headers({"Content-Type": "application/json"})
        });
    }
}

export default TaskService;