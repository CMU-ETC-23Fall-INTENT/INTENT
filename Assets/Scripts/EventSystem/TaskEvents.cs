using System;

public class TaskEvents
{
    public event Action<string> OnTaskAvailable;

    public void TaskAvailable(string taskId)
    {
        OnTaskAvailable?.Invoke(taskId);
    }

    public event Action<string> OnTaskStarted;

    public void TaskStarted(string taskId)
    {
        OnTaskStarted?.Invoke(taskId);
    }
    public event Action<string> OnTaskCompleted;

    public void TaskCompleted(string taskId)
    {
        OnTaskCompleted?.Invoke(taskId);
    }
    

    public event Action<Task> OnTaskStatusChanged;

    public void TaskStatusChanged(Task task)
    {
        OnTaskStatusChanged?.Invoke(task);
    }
}

