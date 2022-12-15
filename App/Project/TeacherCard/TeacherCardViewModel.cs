using System.Reactive;
using Domain;
using Domain.Project.Models;
using ReactiveUI;

namespace App.Project.TeacherCard;

public class TeacherCardViewModel : ViewModelBase
{
    public delegate TeacherCardViewModel Factory(IdentifiedModel<Teacher> teacher);

    public TeacherCardViewModel(IdentifiedModel<Teacher> teacher)
    {
        ShowEditor = ReactiveCommand.Create(() => Unit.Default);
        Title = teacher.Model.Name;
    }

    public ReactiveCommand<Unit, Unit> ShowEditor { get; }

    public string Title { get; }
}