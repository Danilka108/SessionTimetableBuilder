using Domain;
using Domain.Project.Models;

namespace App.Project.TeacherEditor;

public class TeacherEditorViewModel : ViewModelBase
{
    public delegate TeacherEditorViewModel Factory(IdentifiedModel<Teacher>? teacher);

    public TeacherEditorViewModel(IdentifiedModel<Teacher>? teacher)
    {
    }
}