using ReactiveUI;

namespace App.Views.CreateTeacher;

public class CreateTeacherViewModel : ViewModelBase, IRoutableViewModel
{
    public CreateTeacherViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string UrlPathSegment => "/create_teacher";
    public IScreen HostScreen { get; }
}