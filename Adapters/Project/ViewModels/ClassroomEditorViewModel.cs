using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Classroom;
using Domain.Project;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;


public class ClassroomEditorViewModel : BaseViewModel, IActivatableViewModel
{
    public delegate ClassroomEditorViewModel Factory(Classroom? classroom);

    private string _capacity;

    private string _number;

    private readonly ObservableAsPropertyHelper<IEnumerable<ClassroomFeature>> _allFeatures;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveClassroomUseCase _saveUseCase;

    private readonly int? _classroomId;

    private readonly ObservableAsPropertyHelper<bool> _isLoading;

    public ClassroomEditorViewModel(
        Classroom? classroom,
        IClassroomFeatureGateway featureGateway,
        MessageDialogViewModel.Factory messageDialogFactory,
        SaveClassroomUseCase saveUseCase,
        NumericFieldValidator.Factory numericFieldValidator
    )
    {
        _classroomId = classroom?.Id;
        _capacity = classroom?.Capacity.ToString() ?? "";
        _number = classroom?.Number.ToString() ?? "";

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

        _isLoading = Save.IsExecuting.ToProperty(this, vm => vm.IsLoading);

        _allFeatures = featureGateway
            .ObserveAll()
            .FirstAsync()
            .Catch<IEnumerable<ClassroomFeature>, Exception>(ex =>
                CatchFeaturesObserving(ex).ToObservable())
            .ToProperty(this, vm => vm.AllFeatures);

        this.WhenActivated(d => { _allFeatures.DisposeWith(d); });
    }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            var number = int.Parse(Number);
            var capacity = int.Parse(Capacity);

            await _saveUseCase.Handle(number, capacity, _classroomId, SelectedFeatures.ToArray(), token);
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

    public bool IsLoading => _isLoading.Value;

    public string Capacity
    {
        get => _capacity;
        set => this.RaiseAndSetIfChanged(ref _capacity, value);
    }

    public string Number
    {
        get => _number;
        set
        {
            this.RaiseAndSetIfChanged(ref _number, value);
        }
    }

    public ObservableCollection<ClassroomFeature> SelectedFeatures { get; }
    public IEnumerable<ClassroomFeature> AllFeatures => _allFeatures.Value;
    public ReactiveCommand<Unit, Unit> Save { get; }
    public ReactiveCommand<Unit, Unit> Close { get; }
    public Interaction<Unit, Unit> Finish { get; }
    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }
    public ViewModelActivator Activator { get; }
}