using System.Collections.Generic;
using System.Reactive;
using App.Project.Browser;
using App.Project.Explorer;
using App.Project.TeacherEditor;
using Domain;
using Domain.Project.Models;
using ReactiveUI;

namespace App.Project.ProjectWindow;

public class ProjectWindowViewModel : ViewModelBase, IBrowser
{
    private int i = 0;
    
    public ProjectWindowViewModel
    (
        ExplorerViewModel.Factory explorerViewModelFactory,
        TeacherEditorViewModel.Factory teacherEditorViewModelFactory
    )
    {
        ExplorerViewModel = explorerViewModelFactory.Invoke();
        BrowserState = new BrowserState();

        var teacher = new IdentifiedModel<Teacher>
        (
            0,
            new Teacher("Test", "Test", "Test", new List<IdentifiedModel<Discipline>>())
        );

        Browse = ReactiveCommand.CreateFromObservable
            (() =>
            {
                var a = BrowserState.Browse.Execute(teacherEditorViewModelFactory.Invoke(teacher, i));
                i++;
                return a;
            });
    }

    public ReactiveCommand<Unit, Unit> Browse { get; }

    public ExplorerViewModel ExplorerViewModel { get; }

    public BrowserState BrowserState { get; }
}