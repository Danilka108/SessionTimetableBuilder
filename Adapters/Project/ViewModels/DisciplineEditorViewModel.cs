using Domain.Project;

namespace Adapters.Project.ViewModels;

public class DisciplineEditorViewModel : BaseViewModel
{
    public delegate DisciplineEditorViewModel Factory(Discipline? discipline);
    
    public DisciplineEditorViewModel(Discipline? discipline)
    {}
}