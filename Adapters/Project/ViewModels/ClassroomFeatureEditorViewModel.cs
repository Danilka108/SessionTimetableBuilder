using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.ClassroomFeature;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate ClassroomFeatureEditorViewModel Factory(ClassroomFeature? feature);

    private readonly int? _featureId;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveClassroomFeatureUseCase _saveUseCase;

    public ClassroomFeatureEditorViewModel(
        ClassroomFeature? feature,
        SaveClassroomFeatureUseCase saveUseCase,
        MessageDialogViewModel.Factory messageDialogFactory,
        NotEmptyFieldValidator.Factory notEmptyFieldValidator
    )
    {
        _featureId = feature?.Id;
        Description = feature?.Description ?? "";

        Activator = new ViewModelActivator();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        Finish = new Interaction<Unit, Unit>();

        _messageDialogFactory = messageDialogFactory;
        _saveUseCase = saveUseCase;

        var descriptionIsNotEmpty =
            this.WhenAnyValue(vm => vm.Description,
                notEmptyFieldValidator.Invoke);

        this.ValidationRule(vm => vm.Description,
            descriptionIsNotEmpty);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        var canBeClosed = Save
            .IsExecuting
            .Select(v => !v);

        Close = ReactiveCommand.CreateFromTask(async () => { await Finish.Handle(Unit.Default); },
            canBeClosed);

        Save
            .IsExecuting
            .ToPropertyEx(this, vm => vm.IsLoading);

        this.WhenActivated(d => { Save.DisposeWith(d); });
    }

    [Reactive] public string Description { get; set; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<Unit, Unit> Finish { get; }

    [ObservableAsProperty] public bool IsLoading { get; }

    public ViewModelActivator Activator { get; }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            await _saveUseCase.Handle(_featureId, Description, token);
            await Finish.Handle(Unit.Default);
        }
        catch (ClassroomFeatureGatewayException)
        {
            await ShowErrorMessage(new LocalizedMessage.Error.StorageIsNotAvailable());
        }
        catch (NotOriginalDescriptionException)
        {
            var message = new LocalizedMessage.Error.DescriptionOfClassroomFeatureMustBeOriginal();
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
}