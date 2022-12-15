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
            (() => BrowserState.Browse.Execute(teacherEditorViewModelFactory.Invoke(teacher)));
    }

    public ReactiveCommand<Unit, Unit> Browse { get; }

    public ExplorerViewModel ExplorerViewModel { get; }

    public BrowserState BrowserState { get; }
}