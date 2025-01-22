using CommunityToolkit.Mvvm.Input;
using MVVM.Models;

namespace MVVM.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}