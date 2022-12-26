using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Application.Project.UseCases.Group;
using Domain.Project;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class GroupEditorViewModel : BaseViewModel, IBrowserPage, IActivatableViewModel
{
    public delegate GroupEditorViewModel Factory(Group? group);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveGroupUseCase _saveUseCase;

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private int? _groupId;

    public GroupEditorViewModel
    (
        Group? group,
        NotEmptyFieldValidator.Factory notEmptyFieldValidator,
        NumericFieldValidator.Factory numericFieldValidator,
        SaveGroupUseCase saveUseCase,
        MessageDialogViewModel.Factory messageDialogFactory,
        IDisciplineGateway disciplineGateway,
        ILocalizedMessageConverter localizedMessageConverter,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        IExamGateway examGateway
    )
    {
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        _groupId = group?.Id;
        Name = group?.Name ?? string.Empty;
        StudentsNumber = group?.StudentsNumber.ToString() ?? string.Empty;
        PageName =
            localizedMessageConverter.Convert(LocalizedMessage.Letter.Group) + " " + group?.Name;

        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;
        _saveUseCase = saveUseCase;

        Activator = new ViewModelActivator();

        AllDisciplines =
            new ObservableCollection<Discipline>(group?.Disciplines ?? new Discipline[] { });
        SelectedDisciplines =
            new ObservableCollection<Discipline>(group?.Disciplines ?? new Discipline[] { });

        this.WhenActivated(d =>
        {
            disciplineGateway
                .ObserveAll()
                .Catch<IEnumerable<Discipline>, Exception>(ex =>
                    CatchObservableExceptions(ex).ToObservable())
                .Subscribe(IntersectAllDisciplinesWith)
                .DisposeWith(d);
        });

        var isNameValid = this.WhenAnyValue(
            vm => vm.Name,
            notEmptyFieldValidator.Invoke
        );

        var isStudentsNumberValid = this.WhenAnyValue(
            vm => vm.StudentsNumber,
            numericFieldValidator.Invoke
        );

        this.ValidationRule(vm => vm.Name, isNameValid);
        this.ValidationRule(vm => vm.StudentsNumber, isStudentsNumberValid);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        var isLoading = Save
            .IsExecuting
            .ToPropertyEx(this, vm => vm.IsLoading);

        this.WhenActivated(d =>
        {
            examGateway
                .ObserveAll()
                .Select(exams => exams.Where(exam => exam.Group.Id == _groupId))
                .ToPropertyEx(this, vm => vm.GroupExams)
                .DisposeWith(d);

            this.WhenAnyValue(vm => vm.Name)
                .Subscribe(name =>
                    PageName = localizedMessageConverter.Convert(LocalizedMessage.Letter.Group) +
                               " " + name)
                .DisposeWith(d);

            isLoading.DisposeWith(d);
        });
    }

    public async Task<bool> ConfirmPageClosingAsync()
    {
        var action = LocalizedMessage.Letter.Close;
        var message = new LocalizedMessage.Question.CloseGroupEditor();
        var dialog = _confirmDialogFactory.Invoke(action, message);

        return await OpenConfirmDialog.Handle(dialog);
    }

    [Reactive] public string StudentsNumber { get; set; }

    [Reactive] public string Name { get; set; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ObservableCollection<Discipline> AllDisciplines { get; }

    public ObservableCollection<Discipline> SelectedDisciplines { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    [ObservableAsProperty] public bool IsLoading { get; }

    [Reactive] public string PageName { get; private set; }

    [ObservableAsProperty] public IEnumerable<Exam> GroupExams { get; }

    private async Task<IEnumerable<Discipline>> CatchObservableExceptions(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new Discipline[] { };
    }

    private void IntersectAllDisciplinesWith(
        IEnumerable<Discipline> updatedDisciplines)
    {
        var comparer = new Discipline.Comparer();
        var itemsToRemove = from oldDiscipline in AllDisciplines
            let doesNotContains = !updatedDisciplines.Contains(oldDiscipline, comparer)
            where doesNotContains
            select oldDiscipline;

        AllDisciplines.RemoveMany(itemsToRemove);

        var disciplinesToAdd =
            from newDiscipline in updatedDisciplines
            let doesNotContains = !AllDisciplines.Contains(newDiscipline, comparer)
            where doesNotContains
            select newDiscipline;

        AllDisciplines.AddRange(disciplinesToAdd);
    }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            var studentsNumber = int.Parse(StudentsNumber);
            var group = await _saveUseCase.Handle(Name, studentsNumber,
                SelectedDisciplines.ToArray(), _groupId, token);
            _groupId = group.Id;
        }
        catch (GroupReferencedByExamException e)
        {
            var message =
                new LocalizedMessage.Error.GroupReferencedByExam(e.Exam.Discipline.Name);
            await ShowErrorMessage(message);
        }
        catch (GroupNameMustBeOriginalException)
        {
            var message = new LocalizedMessage.Error.NameOfGroupMustBeOriginal();
            await ShowErrorMessage(message);
        }
        catch (GroupGatewayException)
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

    public ViewModelActivator Activator { get; }
}