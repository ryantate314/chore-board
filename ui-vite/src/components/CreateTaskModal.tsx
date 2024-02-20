import { useEffect, useState } from 'react';

import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Form from "react-bootstrap/Form";
import TaskService from '@/services/task.service';
import dayjs from 'dayjs';

export interface CreateTaskModalProps {
    showModal: boolean;
    hideModal: () => void;
    reloadTasks: () => void;
}

export enum Frequency {
    Daily = 3,
    Weekly = 2,
    Monthly = 1,
    Yearly = 0
};

export enum TaskCategory {
    Personal = 1,
    Family = 2
}

export interface FormData
{
    shortDescription: string | null;
    description: string | null;
    points: number | null;
    category: TaskCategory;
    doesRepeat: boolean;
    frequency: Frequency;
    daysOfWeek: string[];
    startDate: string;
    interval: number;
}

export function CreateTaskModal(props: CreateTaskModalProps) {

    const [ formData, setFormData ] = useState<FormData>({
        shortDescription: null,
        description: null,
        points: null,
        category: TaskCategory.Family,
        doesRepeat: false,
        frequency: Frequency.Daily,
        daysOfWeek: [],
        startDate: dayjs()
            .startOf('day')
            .format("YYYY-MM-DDTHH:mm"),
        interval: 1
    });
    const [ frequencyDisplayName, setFrequencyDisplayName ] = useState<string>();

    const setFormValue = (event: React.ChangeEvent<any>, newValue?: any) => {
        const fieldName = event.target.name;
        setFormData({
            ...formData,
            [fieldName]: newValue === undefined ? event.target.value : newValue
        } as FormData);
    };

    const toggleTaskCategory = () => {
        setFormData({
            ...formData,
            category: formData.category == TaskCategory.Family ? TaskCategory.Personal : TaskCategory.Family
        });
    };

    const handleClose = () => props.hideModal();
    
    const handleRecurringChange = () => {
        setFormData({
            ...formData,
            doesRepeat: !formData.doesRepeat
        })
    }
    const handleDayChange = (day: string) => {
        if (formData.daysOfWeek.indexOf(day) > -1) {
            setFormData({
                ...formData,
                daysOfWeek: formData.daysOfWeek.filter(x => x != day)
            });
        }
        else {
            setFormData({
                ...formData,
                daysOfWeek: [
                    ...formData.daysOfWeek,
                    day
                ]
            })
        }
    }

    const handleSubmit = async (evt: React.FormEvent) => {
        evt.preventDefault();

        await TaskService.createTaskDefinition(formData);

        props.reloadTasks();

        props.hideModal();
    };

    useEffect(() => {
        setFrequencyDisplayName(
            getFrequencyDisplayName(formData.frequency, formData.interval)
        );
    }, [formData.frequency, formData.interval])

    function getFrequencyDisplayName(frequency: Frequency, interval: number) {
        let pluralDisplayName = null;
        switch (frequency) {
            case Frequency.Daily:
                pluralDisplayName = "days";
                break;
            case Frequency.Weekly:
                pluralDisplayName = "weeks";
                break;
            case Frequency.Monthly:
                pluralDisplayName = "months";
                break;
            case Frequency.Yearly:
                pluralDisplayName = "years";
                break;
            default:
                return "";
        }
        if (interval == 1)
            return pluralDisplayName.substring(0, pluralDisplayName.length - 1);
        return pluralDisplayName;
    }

    const days = ["Mon", "Tues", "Wed", "Thur", "Fri", "Sat", "Sun"];

    return props.showModal ? <>
        {/* Background */}
        <Modal show={props.showModal} onHide={handleClose}>
            <Form onSubmit={handleSubmit}>
                {/* Content */}
                <Modal.Header closeButton={true}>
                    <Modal.Title>Create a Task</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Group className="mb-2">
                        <Form.Label htmlFor="newtask-shortdescription">Short Description</Form.Label>
                        <Form.Control type="input"
                            name="shortDescription"
                            id="newtask-shortdescription"
                            value={formData.shortDescription ?? ""}
                            onChange={setFormValue}
                            required={true}
                            maxLength={128} />
                    </Form.Group>
                    <Form.Group className="mb-2">
                        <Form.Label htmlFor="newtask-description">Description</Form.Label>
                        <Form.Control as="textarea" id="newtask-description" name="description" value={formData.description ?? ""} onChange={setFormValue} />
                    </Form.Group>
                    <Form.Group className="mb-2">
                        <Form.Label htmlFor="newtask-category">Is this a Family Task?</Form.Label>
                        <Form.Check type="switch" id="newtask-category" name="category" checked={formData.category == TaskCategory.Family} onChange={toggleTaskCategory} />
                    </Form.Group>
                    {
                        formData.category == TaskCategory.Family ?
                            <Form.Group className="mb-2">
                                <Form.Label htmlFor="newtask-points">Points</Form.Label>
                                <Form.Control type="number" name="points" id="newtask-points" value={formData.points ?? 0} onChange={setFormValue} min={1} />
                            </Form.Group>
                            : null
                    }
                    <Form.Group className="mb-2">
                        <Form.Label htmlFor="newtask-recurring">Does This Task Repeat?</Form.Label>
                        <Form.Check type="switch" id="newtask-recurring" checked={formData.doesRepeat} onChange={handleRecurringChange}></Form.Check>
                    </Form.Group>
                    {
                        formData.doesRepeat ? <>
                            <Form.Group className="mb-2">
                                <Form.Label htmlFor="newtask-frequency">Frequency</Form.Label>
                                <Form.Select name="frequency" onChange={evt => setFormValue(evt, +evt.target.value)} value={formData.frequency}>
                                    <option value={Frequency.Daily}>Daily</option>
                                    <option value={Frequency.Weekly}>Weekly</option>
                                    <option value={Frequency.Monthly}>Monthly</option>
                                    <option value={Frequency.Yearly}>Yearly</option>
                                </Form.Select>
                            </Form.Group>
                            {
                                formData.frequency == Frequency.Weekly ? <>
                                    <Form.Group className="mb-2">
                                        {
                                            days.map(day =>
                                                <Form.Check type="checkbox" key={day}
                                                    id={`newtask-day-${day}`}
                                                    label={day}
                                                    checked={formData.daysOfWeek.indexOf(day) > -1}
                                                    onChange={() => handleDayChange(day)}
                                                    inline />
                                            )
                                        }
                                    </Form.Group>
                                </> : null
                            }
                            <Form.Group className="mb-2">
                                <Form.Label htmlFor="newtask-interval">Interval</Form.Label>
                                <Form.Control type="number"
                                    id="newtask-interval"
                                    name="interval"
                                    onChange={evt => setFormValue(evt, +evt.target.value)}
                                    value={formData.interval}
                                    min={1}
                                    step={1} />
                                <Form.Text>Trigger every {formData.interval > 1 ? formData.interval : null} {frequencyDisplayName}</Form.Text>
                            </Form.Group>
                            <Form.Group className="mb-2">
                                <Form.Label htmlFor="newtask-startdate">Start Date</Form.Label>
                                <Form.Control type="datetime-local"
                                    id="newtask-startdate"
                                    name="startDate"
                                    value={formData.startDate}
                                    onChange={setFormValue} />
                            </Form.Group>
                        </>
                            : null
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button type="submit">Save</Button>
                    <Button onClick={handleClose}>Cancel</Button>
                </Modal.Footer>
            </Form>
        </Modal>
    </>
        : null;
}