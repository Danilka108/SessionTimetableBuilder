using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using App.Project.TeacherCard;
using Domain.Project.useCases.Teacher;
using ReactiveUI;

namespace App.Project.Teachers;

public class TeachersViewModel : ViewModelBase
{
    public delegate TeachersViewModel Factory();

    private readonly ObservableAsPropertyHelper<IEnumerable<TeacherCardViewModel>> _cards;

    public TeachersViewModel
    (
        TeacherCardViewModel.Factory cardViewModelFactory,
        ObserveAllTeachersUseCase observeAllUseCase
    )
    {
        Create = ReactiveCommand.Create(() => Unit.Default);

        _cards = observeAllUseCase
            .Handle()
            .Select
            (
                teachers => teachers.Select(cardViewModelFactory.Invoke)
            )
            .ToProperty(this, vm => vm.Cards);
    }

    public ReactiveCommand<Unit, Unit> Create { get; }

    public IEnumerable<TeacherCardViewModel> Cards => _cards.Value;
}