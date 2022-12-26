using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Adapters.Common.Validators;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Application.Project.UseCases.Exam;
using Application.Project.useCases.Lecturer;
using Domain.Project;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class LecturerEditorViewModel : BaseViewModel, IBrowserPage, IActivatableViewModel
{
    public delegate LecturerEditorViewModel Factory(Lecturer? lecturer);

    private readonly SaveLecturerUseCase _saveUseCase;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private int? _lecturerId;

    public LecturerEditorViewModel(
        Lecturer? lecturer,
        IDisciplineGateway disciplineGateway,
        NotEmptyFieldValidator.Factory notEmptyFieldValidatorFactory,
        SaveLecturerUseCase saveUseCase,
        MessageDialogViewModel.Factory messageDialogFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        ILocalizedMessageConverter localizedMessageConverter,
        IExamGateway examGateway
    )
    {
        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;
        _saveUseCase = saveUseCase;

        _lecturerId = lecturer?.Id;
        PageName = localizedMessageConverter.Convert(LocalizedMessage.Letter.Lecturer) + " " +
                   lecturer?.FullName;

        Name = lecturer?.Name ?? "";
        Surname = lecturer?.Surname ?? "";
        Patronymic = lecturer?.Patronymic ?? "";

        SelectedDisciplines =
            new ObservableCollection<Discipline>(lecturer?.Disciplines ?? new Discipline[] { });
        AllDisciplines = new ObservableCollection<Discipline>(SelectedDisciplines);

        Activator = new ViewModelActivator();

        this.WhenActivated(d =>
        {
            disciplineGateway
                .ObserveAll()
                .Catch<IEnumerable<Discipline>, Exception>(ex =>
                    CatchObservableExceptions(ex).ToObservable())
                .Subscribe(IntersectAllDisciplinesWith)
                .DisposeWith(d);
        });

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();

        var isNameValid = this.WhenAnyValue(
            vm => vm.Name,
            notEmptyFieldValidatorFactory.Invoke
        );

        var isSurnameValid = this.WhenAnyValue(
            vm => vm.Surname,
            notEmptyFieldValidatorFactory.Invoke
        );

        var isPatronymicValid = this.WhenAnyValue(
            vm => vm.Patronymic,
            notEmptyFieldValidatorFactory.Invoke
        );

        this.ValidationRule(vm => vm.Name, isNameValid);
        this.ValidationRule(vm => vm.Surname, isSurnameValid);
        this.ValidationRule(vm => vm.Patronymic, isPatronymicValid);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        this.WhenActivated(d =>
        {
            examGateway.ObserveAll()
                .Select(exams => exams.Where(exam => exam.Lecturer.Id == _lecturerId))
                .ToPropertyEx(this, vm => vm.LecturerExams)
                .DisposeWith(d);

            Save
                .IsExecuting
                .ToPropertyEx(this, vm => vm.IsLoading)
                .DisposeWith(d);

            this.WhenAnyValue(
                    vm => vm.Name,
                    vm => vm.Surname,
                    vm => vm.Patronymic
                )
                .Subscribe(tuple =>
                {
                    PageName = localizedMessageConverter.Convert(LocalizedMessage.Letter.Lecturer) +
                               " " + Lecturer.ProduceFullname(
                                   tuple.Item1,
                                   tuple.Item2,
                                   tuple.Item3
                               );
                })
                .DisposeWith(d);
        });
    }

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

    public async Task<bool> ConfirmPageClosingAsync()
    {
        var action = LocalizedMessage.Letter.Close;
        var message = new LocalizedMessage.Question.CloseLecturerEditor();
        var dialog = _confirmDialogFactory.Invoke(action, message);

        return await OpenConfirmDialog.Handle(dialog);
    }
    
    [ObservableAsProperty]
    public IEnumerable<Exam> LecturerExams { get; }

    [Reactive] public string Name { get; set; }

    [Reactive] public string Surname { get; set; }

    [Reactive] public string Patronymic { get; set; }

    [Reactive] public string PageName { get; private set; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public ObservableCollection<Discipline> SelectedDisciplines { get; }

    public ObservableCollection<Discipline> AllDisciplines { get; }

    [ObservableAsProperty] public bool IsLoading { get; }

    public ViewModelActivator Activator { get; }

    private async Task DoSave(CancellationToken token)
    {
        try
        {
            var lecturer = await _saveUseCase.Handle(_lecturerId, Name, Surname, Patronymic,
                SelectedDisciplines.ToList(), token);
            _lecturerId = lecturer.Id;
        }
        catch (LecturerReferencedByExamException e)
        {
            var message =
                new LocalizedMessage.Error.LecturerReferencedByExam(e.Exam.Group.Name,
                    e.Exam.Discipline.Name);
            await ShowErrorMessage(message);
        }
        catch (LecturerGatewayException)
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