using System;

// This class is used to define task events that can be subscribed to by other classes
public class TaskEvents
{
    // This event is used when the task is available to be interacted with
    public event Action<string> OnTaskAvailable;

    public void TaskAvailable(string taskId)
    {
        OnTaskAvailable?.Invoke(taskId);
    }


    // This event is used when the task is started  
    public event Action<string> OnTaskStarted;

    public void TaskStarted(string taskId)
    {
        OnTaskStarted?.Invoke(taskId);
    }


    // This event is used when the task is completed
    public event Action<string> OnTaskCompleted;

    public void TaskCompleted(string taskId)
    {
        OnTaskCompleted?.Invoke(taskId);
    }
    

    // This event is used when the task status is changed
    public event Action<Task> OnTaskStatusChanged;

    public void TaskStatusChanged(Task task)
    {
        OnTaskStatusChanged?.Invoke(task);
    }
}

