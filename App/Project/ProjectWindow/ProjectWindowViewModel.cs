using App.Project.AudienceSpecificities;

namespace App.Project.ProjectWindow;

public class ProjectWindowViewModel : ViewModelBase
{
    public ProjectWindowViewModel(AudienceSpecificitiesViewModel.Factory specificitiesViewModelFactory)
    {
        AudienceSpecificitiesViewModel = specificitiesViewModelFactory.Invoke();
    }

    public AudienceSpecificitiesViewModel AudienceSpecificitiesViewModel { get; }
}