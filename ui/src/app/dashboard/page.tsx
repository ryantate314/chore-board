'use client'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronDown, faUsers, faX, faCheck } from '@fortawesome/free-solid-svg-icons'
import { useCallback, useEffect, useState } from 'react';
import dayjs from 'dayjs';
import { useIdleTimer } from 'react-idle-timer'

import duration from 'dayjs/plugin/duration'
import relativeTime from 'dayjs/plugin/relativeTime'
import { FamilyMember, TaskDefinition, TaskInstance, TaskStatus } from '@/models/task.model';
import TaskService from '@/services/task.service';

dayjs.extend(duration);
dayjs.extend(relativeTime);
export default function Dashboard() {

    const [familyMember, setFamilyMember] = useState<FamilyMember | null>(null);

    const [selectedTask, setSelectedTask] = useState<TaskInstance | null>(null);

    const [tasks, setTasks] = useState<TaskInstance[]>([]);
    const [taskDefinitions, setTaskDefinitions] = useState<TaskDefinition[]>([]);

    let [upcomingTasks, setUpcomingTasks] = useState<TaskInstance[]>([]);
    let [todoTasks, setTodoTasks] = useState<TaskInstance[]>([]);
    let [inProgressTasks, setInProgressTasks] = useState<TaskInstance[]>([]);
    let [completedTasks, setCompletedTasks] = useState<{ [key: string]: TaskInstance[] }>({});

    // Reset the currently selected user after a period of inactivity
    useIdleTimer({
        timeout: 60*1000,
        onIdle: (event, timer) => {
            setFamilyMember(null);
        }
    });

    useEffect(() => {
        TaskService.getTasks()
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
        setTasks(
            tasks.map(t => t.id == task.id ? task : t)
        );
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
        <main className="w-full">
            <div id="quick-task-definitions" className="flex">
                <div className="w-5">
                    <FontAwesomeIcon icon={faChevronDown}></FontAwesomeIcon>
                </div>
                {
                    taskDefinitions.map(def => <QuickTaskDefinition key={def.id} taskDefinition={def}></QuickTaskDefinition>)
                }
            </div>
            <div className="flex justify-stretch w-full">
                <div id="upcoming-column" className="task-column flex-1">
                    <div>
                        Upcoming Tasks
                    </div>
                    {
                        upcomingTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </div>
                <div id="todo-column" className="task-column flex-1">
                    <div>
                        To Do
                    </div>
                    {
                        todoTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </div>
                <div id="in-progress-column" className="task-column flex-1">
                    <div>
                        In Progress
                    </div>
                    {
                        inProgressTasks.map(task => <TaskCard key={task.id} task={task} selectTask={() => setSelectedTask(task)}></TaskCard>)
                    }
                </div>
                <div id="done-column" className="task-column flex-1">
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
                </div>
            </div>
            <UserSelection familyMember={familyMember} setFamilyMember={setFamilyMember} allFamilyMembers={allFamilyMembers}></UserSelection>
            <TaskModal  task={selectedTask!}
                        showModal={selectedTask != null}
                        hideModal={() => setSelectedTask(null)}
                        updateTask={updateTask} />
        </main>
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
        <div className="p-2 border rounded cursor-pointer" onClick={() => { console.log("Task clicked"); selectTask() }}>
            { task.definition.shortDescription }
        </div>
    );
}

function QuickTaskDefinition(props: any) {
    return (
        <div>
            { props.taskDefinition.shortDescription }
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
            <div className="flex fixed bottom-10 right-10 justify-end w-full space-x-2">
                {
                    allFamilyMembers.map((member: any) => (
                        <button key={member.id} className="w-24 h-24 rounded-full bg-blue-500 text-[24px]" onClick={event => handleSelectFamilyMember(member)}>
                            {member.name}
                        </button>
                    ))
                }
                <button className="w-24 h-24 rounded-full bg-blue-400 text-[24px]" onClick={event => setShowAllFamilyMembers(false)}>
                    <FontAwesomeIcon icon={faX}></FontAwesomeIcon>
                </button>
            </div>
        )
    }
    else if (familyMember == null) {
        return (
            <button className='w-24 h-24 rounded-full bg-blue-500 fixed bottom-10 right-10 text-[24px]' title="Choose Current Family Member" onClick={handleMenuClick}>
                <FontAwesomeIcon icon={faUsers}></FontAwesomeIcon>
            </button>
        );
    }
    else {
        return (
            <button className='w-24 h-24 rounded-full bg-blue-500 fixed bottom-10 right-10 text-[24px]' title={familyMember.name} onClick={handleMenuClick}>
                { familyMember.name }
            </button>
        );
    }
   
}

interface TaskModalProps {
    task: TaskInstance;
    showModal: boolean;
    hideModal: () => void;
    updateTask: (task: TaskInstance) => void;
}

function TaskModal(props: TaskModalProps) {

    const {task, showModal, hideModal, updateTask} = props;

    const handleBackgroundClick = (event: any) => {
        hideModal();
    };

    const handleCompleteTaskClick = (event: any) => {
        event.stopPropagation();
        
        setTaskStatus(TaskStatus.Done);
    }; 

    const escFunction = useCallback((event: any) => {
        if (event.key === "Escape") {
          hideModal();
        }
    }, []);
    
    useEffect(() => {
        document.addEventListener("keydown", escFunction, false);

        return () => {
            document.removeEventListener("keydown", escFunction, false);
        };
    }, [escFunction]);

    const setTaskStatus = (status: TaskStatus) => {
        hideModal(); // Hide first to avoid race condition

        TaskService.updateStatus(task.id, status)
            .then(task => updateTask(task))
            .catch(err => alert("Error updating task status."));
    };

    return showModal ? (
        <>
            {/* Background */}
            <div className='fixed left-0 top-0 w-full h-full bg-neutral-900/50' onClick={handleBackgroundClick}>
                {/* Content */}
                <div className='w-3/4 bg-blue-900 ml-auto mr-auto modal-content mt-8 rounded' onClick={event => event.stopPropagation()}>
                    <header className='p-4 flex bg-blue-500 rounded-t'>
                        <h4>{ task.definition.shortDescription }</h4>
                        <button className="ml-auto" title="Close Task" onClick={() => hideModal()}>
                            <FontAwesomeIcon icon={faX}></FontAwesomeIcon>
                        </button>
                    </header>
                    <main className="p-4">
                        {/* Swim Lane Buttons */}
                        <div className="flex space-x-2 mb-2">
                            { task.status != TaskStatus.ToDo ?
                                <button className='border rounded p-2' onClick={() => setTaskStatus(TaskStatus.ToDo)}>Move to TODO</button>
                                : null
                            }
                            { task.status != TaskStatus.InProgress ?
                                <button className='border rounded p-2' onClick={() => setTaskStatus(TaskStatus.InProgress)}>{ task.status == TaskStatus.Done ? 'Resume' : 'Start' } Task</button>
                                : null
                            }
                        </div>
                        <p dangerouslySetInnerHTML={{__html: task.definition.description}}></p>
                        {/* Added Date */}
                        <small title={dayjs(task.createdAt).format("DD/MM/YY h:mm a")}>Added { dayjs.duration(dayjs(task.createdAt).diff(dayjs())).humanize(true) }.</small>
                    </main>
                </div>
                {
                    task.status != TaskStatus.Done ?
                        <button className='w-24 h-24 rounded-full bg-yellow-500 fixed bottom-10 right-10 text-[24px]' title="Mark Task Complete" onClick={handleCompleteTaskClick}>
                            <FontAwesomeIcon icon={faCheck}></FontAwesomeIcon>
                        </button>
                        : null
                }
                
            </div>
        </>
    ) : null;
}