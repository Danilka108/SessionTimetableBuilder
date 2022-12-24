using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Discipline;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class DisciplineEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate DisciplineEditorViewModel Factory(Discipline? discipline);

    private readonly SaveDisciplineUseCase _saveUseCase;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly int? _disciplineId;

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
        
        var allRequirements = featureGateway
            .ObserveAll()
            .ToPropertyEx(this, vm => vm.AllRequirements);

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
            allRequirements
                .DisposeWith(d);
        });
    }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            await _saveUseCase.Handle(_disciplineId, Name, SelectedRequirements.ToArray(), token);
            await Finish.Handle(Unit.Default);
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
            LocalizedMessage.Header.Error,
            message
        );

        await OpenMessageDialog.Handle(messageDialog);
    }

    [Reactive] public string Name { get; set; }

    [ObservableAsProperty] public IEnumerable<ClassroomFeature> AllRequirements { get; }

    [ObservableAsProperty] public bool IsLoading { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ObservableCollection<ClassroomFeature> SelectedRequirements { get; }

    public ViewModelActivator Activator { get; }
}