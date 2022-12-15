using App.Project.Browser;
using Domain;
using Domain.Project.Models;

namespace App.Project.TeacherEditor;

public class TeacherEditorViewModel : ViewModelBase, IBrowserPage
{
    public delegate TeacherEditorViewModel Factory(IdentifiedModel<Teacher>? teacher);

    public TeacherEditorViewModel(IdentifiedModel<Teacher>? teacher)
    {
        Name = teacher?.Model.Surname ?? "";
    }

    public string Name { get; }
}