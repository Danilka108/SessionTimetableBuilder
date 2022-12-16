using App.Project.Browser;
using Domain;
using Domain.Project.Models;

namespace App.Project.TeacherEditor;

public class TeacherEditorViewModel : ViewModelBase, IBrowserPage
{
    public delegate TeacherEditorViewModel Factory(IdentifiedModel<Teacher>? teacher, int i);

    public TeacherEditorViewModel(IdentifiedModel<Teacher>? teacher, int i)
    {
        Name = teacher?.Model.Surname + i ?? "";
    }

    public string Name { get; }
}