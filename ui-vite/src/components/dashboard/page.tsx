import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronDown, faChevronUp, faUsers, faX } from '@fortawesome/free-solid-svg-icons'
import { useEffect, useState } from 'react';
import dayjs from 'dayjs';
import { useIdleTimer } from 'react-idle-timer'

import { FamilyMember, TaskDefinition, TaskInstance, TaskStatus } from '@/models/task.model';
import TaskService from '@/services/task.service';

import './page.css';
import { TaskModal } from '../TaskModal';
import { CreateTaskModal } from '../CreateTaskModal';

// Bootstrap
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Accordion, Button, Collapse } from 'react-bootstrap';
import { TaskDefinitionList } from '../TaskDefinitionList';

export default function Dashboard() {

    const [familyMember, setFamilyMember] = useState<FamilyMember | null>(null);

    const [selectedTask, setSelectedTask] = useState<TaskInstance | null>(null);

    const [tasks, setTasks] = useState<TaskInstance[]>([]);
    const [taskDefinitions, setTaskDefinitions] = useState<TaskDefinition[]>([]);
    const [showDefinitions, setShowDefinitions] = useState<boolean>(false);

    const [showCreateTaskModal, setShowCreateTaskModal] = useState<boolean>(false);
    const [showCreateTaskInstanceModal, setShowCreateTaskInstanceModal] = useState<boolean>(false);

    let [upcomingTasks, setUpcomingTasks] = useState<TaskInstance[]>([]);
    let [todoTasks, setTodoTasks] = useState<TaskInstance[]>([]);
    let [inProgressTasks, setInProgressTasks] = useState<TaskInstance[]>([]);
    let [completedTasks, setCompletedTasks] = useState<{ [key: string]: TaskInstance[] }>({});

    const startOfWeek = dayjs().startOf('week');
    const endOfWeek = dayjs().endOf('week');

    // Reset the currently selected user after a period of inactivity
    useIdleTimer({
        timeout: 60*1000,
        onIdle: (event, timer) => {
            setFamilyMember(null);
        }
    });

    useEffect(() => {
        TaskService.getTasks(startOfWeek.toDate(), endOfWeek.toDate())
            .then(tasks => setTasks(tasks))
            .catch(() => alert("Error getting tasks"));

        TaskService.getTaskDefinitions()
            .then(definitions => setTaskDefinitions(definitions))
            .catch(() => alert("Error getting task definitions"));
    }, []);

    useEffect(() => {
        setUpcomingTasks(
            tasks.filter(x => x.status == TaskStatus.Upcoming)
        );
        setTodoTasks(
            tasks.filter(x => x.status == TaskStatus.ToDo)
        );
        setInProgressTasks(
            tasks.filter(x => x.status == TaskStatus.InProgress)
        );
        setCompletedTasks(
            tasks.filter(x => x.status == TaskStatus.Done)
                // Sort by date descending
                .sort((a, b) => a.completedAt!.valueOf() - b.completedAt!.valueOf())
                // Group by day
                .reduce((group, task) => {
                    const date = dayjs(task.completedAt).startOf('day');
                    const datehash = date.toISOString();
                    if (!(datehash in group))
                        group[datehash] = [];
                    group[datehash] = [
                        ...group[datehash],
                        task
                    ];
                    return group;
                }, {} as { [key: string]: TaskInstance[] })
        );

        if (selectedTask !== null)
            setSelectedTask(
                tasks.find(x => x.id == selectedTask.id) ?? null
            );
    }, [tasks]);

    const updateTask = (task: TaskInstance) => {
        // Updating an existing task
        if (task.id != null && tasks.some(x => x.id == task.id))
            setTasks(tasks.map(t => t.id == task.id ? task : t));
        // Adding a new task
        else
            setTasks([
                ...tasks.filter(x => x.id != null || x.definition.id != task.definition.id),
                task
            ]);
    };

    const reloadTasks = () => {
        TaskService.getTasks(startOfWeek.toDate(), endOfWeek.toDate())
            .then(tasks => setTasks(tasks))
            .catch(() => alert("Error getting tasks"));

        TaskService.getTaskDefinitions()
            .then(definitions => setTaskDefinitions(definitions))
            .catch(() => alert("Error getting task definitions"));

        setShowDefinitions(false);
    };

    const allFamilyMembers: FamilyMember[] = [
        {
            id: "asdf",
            name: "Ryan"
        },
        {
            id: "asdfg",
            name: "Callie"
        }
    ];

    return (
        <Container fluid={true}>
            <div>
                <Button variant="outline-secondary" className="" onClick={() => setShowDefinitions(!showDefinitions)}>
                    Tasks <FontAwesomeIcon icon={ showDefinitions ? faChevronUp : faChevronDown }></FontAwesomeIcon>
                </Button>
            </div>
            <Collapse in={showDefinitions}>
                <div className="mt-2">
                    <TaskDefinitionList
                        showCreateTaskModal={() => setShowCreateTaskModal(true)}
                        hideModal={() => setShowCreateTaskInstanceModal(false)}
                        definitions={taskDefinitions}
                        reloadTasks={reloadTasks} />
                    
                </div>
            </Collapse>
            <Row>
                <Col id="upcoming-column" className="task-column">
                    <div>
                        Upcoming Tasks
                    </div>
                    {
                        upcomingTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </Col>
                <Col id="todo-column" className="task-column">
                    <div>
                        To Do
                    </div>
                    {
                        todoTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </Col>
                <Col id="in-progress-column" className="task-column">
                    <div>
                        In Progress
                    </div>
                    {
                        inProgressTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </Col>
                <Col id="done-column" className="task-column">
                    <div>
                        Done
                    </div>
                    {
                        Object.keys(completedTasks)
                            .map(dateHash => <CompleteTaskSection key={dateHash}
                                                                date={dayjs(dateHash)}
                                                                tasks={completedTasks[dateHash]}
                                                                selectTask={(task) => setSelectedTask(task)} />
                            )
                    }
                </Col>
            </Row>
            <UserSelection familyMember={familyMember} setFamilyMember={setFamilyMember} allFamilyMembers={allFamilyMembers}></UserSelection>
            <TaskModal  task={selectedTask!}
                        showModal={selectedTask != null}
                        hideModal={() => setSelectedTask(null)}
                        updateTask={updateTask} />
            <CreateTaskModal showModal={showCreateTaskModal}
                            hideModal={() => setShowCreateTaskModal(false)}
                            reloadTasks={reloadTasks} />
            
        </Container>
    );
}

function CompleteTaskSection(props: { date: dayjs.Dayjs, tasks: TaskInstance[], selectTask: (task: TaskInstance) => void }) {
    const { date, tasks, selectTask } = props;
    return (
        <>
            <div className='border-b mb-2'>{ date.format('dddd MMM D') }</div>
            {
                tasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => selectTask(task)}></TaskCard>)
            }
        </>
    );
}

function TaskCard(props: { task: TaskInstance, selectTask: () => void }) {
    const { task, selectTask } = props;

    return (
        <div role="button" className="border rounded p-2" onClick={() => { selectTask() }}>
            { task.definition.shortDescription }
        </div>
    );
}





interface UserSelectionProps {
    familyMember: FamilyMember | null;
    setFamilyMember: (familyMember: FamilyMember) => void;
    allFamilyMembers: FamilyMember[];
}

function UserSelection(props: UserSelectionProps) {
    const { familyMember, setFamilyMember, allFamilyMembers } = props;

    const [showFamilyMembers, setShowAllFamilyMembers] = useState(false);

    const handleMenuClick = (event: any) => {
        setShowAllFamilyMembers(true);
    };

    const handleSelectFamilyMember = (member: FamilyMember) => {
        setFamilyMember(member);
        setShowAllFamilyMembers(false);
    };

    if (showFamilyMembers) {
        return (
            <div className="d-flex position-fixed bottom-0 end-0 mb-4 me-4 justify-end">
                {
                    allFamilyMembers.map((member: any) => (
                        <Button key={member.id} className="circle-button rounded-circle me-2" onClick={event => handleSelectFamilyMember(member)}>
                            {member.name}
                        </Button>
                    ))
                }
                <Button className="circle-button rounded-circle" onClick={event => setShowAllFamilyMembers(false)}>
                    <FontAwesomeIcon icon={faX}></FontAwesomeIcon>
                </Button>
            </div>
        )
    }
    else if (familyMember == null) {
        return (
            <Button className='position-fixed bottom-0 end-0 mb-4 me-4 circle-button rounded-circle' title="Choose Current Family Member" onClick={handleMenuClick}>
                <FontAwesomeIcon icon={faUsers}></FontAwesomeIcon>
            </Button>
        );
    }
    else {
        return (
            <Button className='position-fixed bottom-0 end-0 mb-4 me-4 circle-button rounded-circle' title={familyMember.name} onClick={handleMenuClick}>
                { familyMember.name }
            </Button>
        );
    }
   
}

