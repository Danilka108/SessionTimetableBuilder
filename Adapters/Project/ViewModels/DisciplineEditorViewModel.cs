using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Discipline;
using Domain.Project;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class DisciplineEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate DisciplineEditorViewModel Factory(Discipline? discipline);

    private readonly int? _disciplineId;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveDisciplineUseCase _saveUseCase;

    public DisciplineEditorViewModel(
        Discipline? discipline,
        IClassroomFeatureGateway featureGateway,
        SaveDisciplineUseCase saveUseCase,
        MessageDialogViewModel.Factory messageDialogFactory,
        NotEmptyFieldValidator.Factory notEmptyFieldValidator
    )
    {
        Name = discipline?.Name ?? string.Empty;
        _disciplineId = discipline?.Id;

        SelectedRequirements =
            new ObservableCollection<ClassroomFeature>(discipline?.ClassroomRequirements ??
                                                       new ClassroomFeature[] { });
        AllRequirements = new ObservableCollection<ClassroomFeature>(SelectedRequirements);

        _saveUseCase = saveUseCase;
        _messageDialogFactory = messageDialogFactory;

        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        Finish = new Interaction<Unit, Unit>();

        var isNameValid = this
            .WhenAnyValue(vm => vm.Name, notEmptyFieldValidator.Invoke);
        this.ValidationRule(vm => vm.Name, isNameValid);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        var canBeClosed = Save.IsExecuting.Select(v => !v);
        Close = ReactiveCommand.CreateFromObservable(() => Finish.Handle(Unit.Default),
            canBeClosed);

        Save
            .IsExecuting
            .ToPropertyEx(this, vm => vm.IsLoading);

        this.WhenActivated(d =>
        {
            featureGateway
                .ObserveAll()
                .Catch<IEnumerable<ClassroomFeature>, Exception>(ex =>
                    CatchObservableExceptions(ex).ToObservable())
                .Subscribe(requirements =>
                    IntersectAllEntitiesWith(requirements, AllRequirements, new ClassroomFeature.Comparer()))
                .DisposeWith(d);
        });
    }

    private void IntersectAllEntitiesWith<T>(
        IEnumerable<T> updatedEntities, IList<T> allEntities, IEqualityComparer<T> comparer)
    {
        var itemsToRemove = from oldEntity in allEntities
            let doesNotContains = !updatedEntities.Contains(oldEntity, comparer)
            where doesNotContains
            select oldEntity;

        allEntities.RemoveMany(itemsToRemove);

        var entitiesToAdd =
            from newEntity in updatedEntities
            let doesNotContains = !allEntities.Contains(newEntity, comparer)
            where doesNotContains
            select newEntity;

        allEntities.AddRange(entitiesToAdd);
    }

    private async Task<IEnumerable<ClassroomFeature>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new ClassroomFeature[] { };
    }

    [Reactive] public string Name { get; set; }

    public ObservableCollection<ClassroomFeature> AllRequirements { get; }

    [ObservableAsProperty] public bool IsLoading { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ObservableCollection<ClassroomFeature> SelectedRequirements { get; }

    public ViewModelActivator Activator { get; }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            await _saveUseCase.Handle(_disciplineId, Name, SelectedRequirements.ToArray(), token);
            await Finish.Handle(Unit.Default);
        }
        catch (DisciplineReferencedByExamException e)
        {
            var message =
                new LocalizedMessage.Error.DisciplineReferencedByExam(e.Exam.Group.Name);
            await ShowErrorMessage(message);
        }
        catch (DisciplineNameMustBeOriginalException)
        {
            var message = new LocalizedMessage.Error.NameOfDisciplineMustBeOriginal();
            await ShowErrorMessage(message);
        }
        catch (DisciplineGatewayException)
        {
            var message = new LocalizedMessage.Error.StorageIsNotAvailable();
            await ShowErrorMessage(message);
        }
        catch (Exception)
        {
            var message = new LocalizedMessage.Error.UndefinedError();
            await ShowErrorMessage(message);
        }
    }

    private async Task ShowErrorMessage(LocalizedMessage message)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            message
        );

        await OpenMessageDialog.Handle(messageDialog);
    }
}