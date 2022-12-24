using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Classroom;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class ClassroomEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate ClassroomEditorViewModel Factory(Classroom? classroom);

    private readonly int? _classroomId;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveClassroomUseCase _saveUseCase;

    public ClassroomEditorViewModel(
        Classroom? classroom,
        IClassroomFeatureGateway featureGateway,
        MessageDialogViewModel.Factory messageDialogFactory,
        SaveClassroomUseCase saveUseCase,
        NumericFieldValidator.Factory numericFieldValidator
    )
    {
        _classroomId = classroom?.Id;
        Capacity = classroom?.Capacity.ToString() ?? "";
        Number = classroom?.Number.ToString() ?? "";

        SelectedFeatures =
            new ObservableCollection<ClassroomFeature>(classroom?.Features ??
                                                       new ClassroomFeature[] { });

        _saveUseCase = saveUseCase;
        _messageDialogFactory = messageDialogFactory;

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        Activator = new ViewModelActivator();
        Finish = new Interaction<Unit, Unit>();

        var capacityIsValid =
            this.WhenAnyValue(vm => vm.Capacity,
                numericFieldValidator.Invoke);

        var numberIsValid =
            this.WhenAnyValue(vm => vm.Number,
                numericFieldValidator.Invoke);

        this.ValidationRule(vm => vm.Capacity, capacityIsValid);
        this.ValidationRule(vm => vm.Number, numberIsValid);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        var canBeClosed = Save.IsExecuting.Select(v => !v);
        Close = ReactiveCommand.CreateFromObservable<Unit, Unit>(Finish.Handle, canBeClosed);

        Save
            .IsExecuting
            .ToPropertyEx(this, vm => vm.IsLoading);

        var allFeatures = featureGateway
            .ObserveAll()
            .FirstAsync()
            .Catch<IEnumerable<ClassroomFeature>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .ToPropertyEx(this, vm => vm.AllFeatures);

        this.WhenActivated(d =>
        {
            Save.DisposeWith(d);
            allFeatures.DisposeWith(d);
        });
    }

    [ObservableAsProperty] public bool IsLoading { get; }

    [Reactive] public string Capacity { get; set; }

    [Reactive] public string Number { get; set; }

    public ObservableCollection<ClassroomFeature> SelectedFeatures { get; }

    [ObservableAsProperty] public IEnumerable<ClassroomFeature> AllFeatures { get; }
    public ReactiveCommand<Unit, Unit> Save { get; }
    public ReactiveCommand<Unit, Unit> Close { get; }
    public Interaction<Unit, Unit> Finish { get; }
    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }
    public ViewModelActivator Activator { get; }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            var number = int.Parse(Number);
            var capacity = int.Parse(Capacity);

            await _saveUseCase.Handle(number, capacity, _classroomId, SelectedFeatures.ToArray(),
                token);
            await Finish.Handle(Unit.Default);
        }
        catch (ClassroomNumberMustBeOriginalException)
        {
            var message = new LocalizedMessage.Error.NumberOfClassroomMustBeOriginal();
            await ShowErrorMessage(message);
        }
        catch (ClassroomGatewayException)
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

    private async Task<IEnumerable<ClassroomFeature>> CatchFeaturesObserving(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Header.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new ClassroomFeature[] { };
    }
}