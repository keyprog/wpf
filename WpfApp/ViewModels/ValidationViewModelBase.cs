using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;

public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> propertyErrors = [];

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName is not null && propertyErrors.TryGetValue(propertyName, out var errors) ? errors: Array.Empty<string>();
    }

    protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e) => ErrorsChanged?.Invoke(this, e);

    protected void AddError(string error, string propertyName)
    {
        if (!propertyErrors.TryGetValue(propertyName, out var errors))
        {
            errors = new List<string>();
            propertyErrors.Add(propertyName, errors);
        }

        if (!errors.Contains(error))
        {
            errors.Add(error);
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            NotifyPropertyChanged(nameof(HasErrors));
        }
    }

    protected void ClearErrors(string propertyName)
    {
        if (propertyErrors.ContainsKey(propertyName))
        {
            propertyErrors.Remove(propertyName);
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
            NotifyPropertyChanged(nameof(HasErrors));
        }
    }
}