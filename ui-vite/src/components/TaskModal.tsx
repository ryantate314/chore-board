import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useCallback, useEffect } from "react";

import { faCheck, faTrash } from '@fortawesome/free-solid-svg-icons'
import dayjs from "dayjs";
import { TaskInstance, TaskStatus } from "../models/task.model";
import taskService from "../services/task.service";

import Modal from "react-bootstrap/Modal";
import Button from "react-bootstrap/Button";

export interface TaskModalProps {
    task: TaskInstance;
    showModal: boolean;
    hideModal: () => void;
    updateTask: (task: TaskInstance) => void;
}

export function TaskModal(props: TaskModalProps) {
    const task = props.task;

    const handleHideModal = () => props.hideModal();

    const handleCompleteTaskClick = (event: any) => {
        event.stopPropagation();
        
        setTaskStatus(TaskStatus.Done);
    }; 

    const handleDeleteTask = (event: any) => {
        event.stopPropagation();

        setTaskStatus(TaskStatus.Deleted);
    }

    const escFunction = useCallback((event: any) => {
        if (event.key === "Escape") {
            props.hideModal();
        }
    }, []);
    
    useEffect(() => {
        document.addEventListener("keydown", escFunction, false);

        return () => {
            document.removeEventListener("keydown", escFunction, false);
        };
    }, [escFunction]);

    const setTaskStatus = async (status: TaskStatus) => {
        let updatedTask;
        // If this is a virtual task
        if (task.id === null)
            updatedTask = await taskService.createTask(props.task.definition.id, task.instanceDate, status)
                .catch(err => alert("Error corporializing task."));
        else
            updatedTask = await taskService.updateStatus(task.id, status)
                .catch(err => alert("Error updating task status."));

        if (updatedTask)
            props.updateTask(updatedTask);
    };

    return props.showModal ? <>
        <Modal show={props.showModal} onHide={handleHideModal}>
            <Modal.Header closeButton={true}>
                <Modal.Title>{ task.definition.shortDescription } - { dayjs(task.instanceDate).format("MM/DD/YY") }</Modal.Title>
            </Modal.Header>
            <Modal.Body className="p-4">
                {/* Swim Lane Buttons */}
                <div className="flex space-x-2 mb-2">
                    { task.status != TaskStatus.ToDo ?
                        <Button className='border rounded p-2' onClick={() => setTaskStatus(TaskStatus.ToDo)}>Move to TODO</Button>
                        : null
                    }
                    { task.status != TaskStatus.InProgress ?
                        <Button className='border rounded p-2' onClick={() => setTaskStatus(TaskStatus.InProgress)}>{ task.status == TaskStatus.Done ? 'Resume' : 'Start' } Task</Button>
                        : null
                    }
                </div>
                <p dangerouslySetInnerHTML={{__html: task.definition.description}}></p>
                {/* Added Date */}
                <small title={dayjs(task.createdAt).format("MM/DD/YY h:mm a")}>Added { dayjs.duration(dayjs(task.createdAt).diff(dayjs())).humanize(true) }.</small>
                <div>Task Definition: {task.definition.id}</div>
                <div>Instance ID: {task.id}</div>
            </Modal.Body>
            <Modal.Footer>
                {
                task.status != TaskStatus.Done ?
                    <Button  title="Mark Task Complete" onClick={handleCompleteTaskClick}>
                        <FontAwesomeIcon icon={faCheck}></FontAwesomeIcon>
                    </Button>
                    : null
                }
                <Button title="Delete Task" variant="danger" onClick={handleDeleteTask}>
                    <FontAwesomeIcon icon={faTrash}></FontAwesomeIcon>
                </Button>
            </Modal.Footer>
        </Modal>
    </> : null;
}