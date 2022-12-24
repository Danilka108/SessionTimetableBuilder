using Domain.Project;

namespace Adapters.Project.ViewModels;

public class ClassroomEditorViewModel : BaseViewModel
{
    public delegate ClassroomEditorViewModel Factory(Classroom? classroom);

    public ClassroomEditorViewModel(Classroom? classroom)
    {
        
    }
}