using Adapters.Project.Browser;
using Domain.Project;
using ReactiveUI.Fody.Helpers;

namespace Adapters.Project.ViewModels;

public class LecturerEditorViewModel : BaseViewModel, IBrowserPage
{
    public delegate LecturerEditorViewModel Factory(Lecturer? lecturer);

    public LecturerEditorViewModel(Lecturer? lecturer)
    {
        PageName = lecturer?.FullName ?? "";
    }

    [Reactive]
    public string PageName { get; private set; }
}