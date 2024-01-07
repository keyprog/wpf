using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ViewModels;

interface INotifyTaskCompletion
{
    event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    string ErrorMessage { get; }
    AggregateException Exception { get; }
    Exception InnerException { get; }
    bool IsCanceled { get; }
    bool IsCompleted { get; }
    bool IsFaulted { get; }
    bool IsSuccessfullyCompleted { get; }
    TaskStatus Status { get; }
}

public sealed class NotifyTaskCompletion<TResult> : INotifyTaskCompletion, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public NotifyTaskCompletion(Task<TResult> task)
    {
        Task = task;
        if (!task.IsCanceled)
        {
            var _ = WatchTaskAsync(task);
        }
    }

    private async Task WatchTaskAsync(Task tast)
    {
        try
        {
            await task;
        }
        catch
        {
        }
        var propertyChanged = PropertyChanged;
        if (propertyChanged == null)
            return;

        propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
        if (task.IsCanceled)
        {

            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
        }
        else if (task.IsFaulted)
        {

            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
        }
        else
        {

            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
        }

    }

    public void Refresh(Task<TResult> refreshTask)
    {
        Task = refreshTask;
        var propertyChanged = PropertyChanged;
        if (propertyChanged == null)
            return;

        propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));

        if(!refreshTask.IsCompleted)
        {
            var _ = WatchTaskAsync(refreshTask);
        }
    }

    public Task<TResult> Task { get; private set; }
    public TResult Result { get => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult); }
    public TaskStatus Status => Task.Status;

    public bool IsCompleted => Task.IsCompleted;
    public bool IsNotCompleted => !Task.IsCompleted;
    public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
    public bool IsCanceled => Task.IsCanceled;
    public bool IsFaulted => Task.IsFaulted;
    public AggregateException Exception => Task.Exception;
    public Excption InnerException => Exception?.InnerException;
    public string ErrorMessage => InnterException?.Message;
}