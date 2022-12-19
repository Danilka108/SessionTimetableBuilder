using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapter;
using App.Project.Browser;
using App.Project.TeacherCard;
using App.Project.TeacherEditor;
using Domain.Project.useCases.Teacher;
using ReactiveUI;

namespace Adapter.Project.ViewModels;

public class TeachersViewModel : BaseViewModel, IRoutableViewModel, IActivatableViewModel
{
    public delegate TeachersViewModel Factory(IScreen screen, IBrowser browser,
        IObservable<Unit> creating);

    private readonly ObservableAsPropertyHelper<IEnumerable<TeacherCardViewModel>> _cards;

    public TeachersViewModel
    (
        IScreen screen,
        IBrowser browser,
        IObservable<Unit> creating,
        TeacherCardViewModel.Factory cardViewModelFactory,
        TeacherEditorViewModel.Factory editorViewModelFactory,
        ObserveAllTeachersUseCase observeAllUseCase
    )
    {
        Activator = new ViewModelActivator();
        HostScreen = screen;
        Create = ReactiveCommand.Create(() => Unit.Default);

        _cards = observeAllUseCase
            .Handle()
            .Select
            (
                teachers =>
                    teachers.Select(teacher => cardViewModelFactory.Invoke(teacher, browser))
            )
            .ToProperty(this, vm => vm.Cards);

        this.WhenActivated(d =>
        {
            creating
                .SelectMany(_ =>
                {
                    return browser.Manager.Browse.Execute
                    (
                        editorViewModelFactory.Invoke(null, browser)
                    );
                })
                .Subscribe()
                .DisposeWith(d);
        });
    }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public IEnumerable<TeacherCardViewModel> Cards => _cards.Value;

    public string UrlPathSegment => "/Teachers";

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; }
}