export interface TaskModalProps {
    task: TaskInstance;
    showModal: boolean;
    hideModal: () => void;
    updateTask: (task: TaskInstance) => void;
}

export function TaskModal(props: TaskModalProps) {

    const {task, showModal, hideModal} = props;

    const handleBackgroundClick = (event: any) => {
        hideModal();
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
        fetch(`${process.env.API_URL}/tasks/${task.id}/status`, {
            method: 'PUT',
            body: JSON.stringify({
                status: status
            })
        })
        .then(response => response.json())
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
                        <div className="flex">
                            <button onClick={() => setTaskStatus(TaskStatus.Done)}>Mark Complete</button>
                        </div>
                        <p dangerouslySetInnerHTML={{__html: task.definition.description}}></p>
                        {/* Added Date */}
                        <small title={dayjs(task.addedDate).format("DD/MM/YY h:mm a")}>Added { dayjs.duration(dayjs(task.addedDate).diff(dayjs())).humanize(true) }.</small>
                    </main>
                </div>
            </div>
        </>
    ) : null;
}