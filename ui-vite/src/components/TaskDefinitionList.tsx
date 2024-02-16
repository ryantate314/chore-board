import { TaskDefinition, TaskStatus } from "@/models/task.model";
import TaskService from "@/services/task.service";
import { useEffect, useState } from "react";
import { Button, Col, Form, Modal, Row } from "react-bootstrap";

export interface TaskDefinitionListProps {
    hideModal: () => void;
    definitions: TaskDefinition[];
    reloadTasks: () => void;
    showCreateTaskModal: () => void;
}

export function TaskDefinitionList(props: TaskDefinitionListProps) {
    const { definitions, showCreateTaskModal } = props;

    const createTaskInstance = (id: string) => {
        TaskService.createTask(id, new Date(), TaskStatus.ToDo)
            .then((task) => {
                props.reloadTasks();
                props.hideModal();
            })
            .catch(() => {
                alert("Error creating task.");
            });
    };

    const handleClose = () => props.hideModal();

    const [searchTerm, setSearchTerm] = useState("");
    const [searchResults, setSearchResults] = useState<TaskDefinition[]>([]);

    useEffect(() => {
        const results =
            searchTerm == ""
                ? definitions
                : definitions.filter((x) =>
                    x.shortDescription
                        .toLocaleLowerCase()
                        .includes(searchTerm.toLocaleLowerCase())
                );
        setSearchResults(results);
    }, [definitions, searchTerm]);

    return (
        <>
            <div className="d-flex mb-4">
                <Button variant="outline-primary" onClick={() => showCreateTaskModal()}>
                    New Task
                </Button>
                <Form.Group className="ms-2">
                    <Form.Control
                        type="input"
                        name="shortDescription"
                        id="newtask-shortdescription"
                        placeholder="Search"
                        value={searchTerm}
                        onChange={(evnt) => setSearchTerm(evnt.target.value)}
                        autoComplete="off"
                    />
                </Form.Group>
            </div>
            <div className="d-flex">
                {searchResults.map((x) => (
                    <InstanceButton
                        key={x.id}
                        definition={x}
                        onClick={(id: string) => createTaskInstance(id)}
                    ></InstanceButton>
                ))}
                {searchResults.length > 0 ? null : <p>No Search Results</p>}
            </div>
        </>
    );
}

function InstanceButton(props: { definition: TaskDefinition; onClick: any }) {
    const { definition, onClick } = props;
    return (
        <div className="me-2">
            <Button
                className="mb-1"
                variant="outline-primary"
                onClick={() => onClick(definition.id)}
            >
                {definition.shortDescription}
            </Button>
        </div>
    );
}
