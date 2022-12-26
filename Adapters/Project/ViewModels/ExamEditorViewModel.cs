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
using Domain.Project;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Adapters.Project.ViewModels;

public class ExamEditorViewModel : BaseViewModel, IBrowserPage, IActivatableViewModel
{
    public delegate ExamEditorViewModel Factory(Exam? exam);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly SaveExamUseCase _saveUseCase;

    private readonly ILocalizedMessageConverter _localizedMessageConverter;


    private int? _examId;

    public ExamEditorViewModel
    (
        Exam? exam,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        MessageDialogViewModel.Factory messageDialogFactory,
        NumericFieldValidator.Factory numericFieldValidatorFactory,
        SaveExamUseCase saveUseCase,
        IDisciplineGateway disciplineGateway,
        ILecturerGateway lecturerGateway,
        IGroupGateway groupGateway,
        IClassroomGateway classroomGateway,
        ILocalizedMessageConverter localizedMessageConverter
    )
    {
        _examId = exam?.Id;

        Year = exam?.StartTime.Year.ToString() ?? string.Empty;
        Month = exam?.StartTime.Month.ToString() ?? string.Empty;
        Day = exam?.StartTime.Day.ToString() ?? string.Empty;
        Hour = exam?.StartTime.Hour.ToString() ?? string.Empty;
        Minute = exam?.StartTime.Minute.ToString() ?? string.Empty;

        _localizedMessageConverter = localizedMessageConverter;
        _saveUseCase = saveUseCase;
        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;

        Activator = new ViewModelActivator();
        AllClassrooms = new ObservableCollection<Classroom>();
        AllDisciplines = new ObservableCollection<Discipline>();
        AllGroups = new ObservableCollection<Group>();
        AllLecturers = new ObservableCollection<Lecturer>();

        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        this.WhenActivated(d =>
        {
            disciplineGateway
                .ObserveAll()
                .Catch<IEnumerable<Discipline>, Exception>(ex =>
                    CatchObservableExceptions<Discipline>(ex).ToObservable())
                .Subscribe(entities =>
                    IntersectAllEntitiesWith(entities, AllDisciplines, new Discipline.Comparer()))
                .DisposeWith(d);

            classroomGateway
                .ObserveAll()
                .Catch<IEnumerable<Classroom>, Exception>(ex =>
                    CatchObservableExceptions<Classroom>(ex).ToObservable())
                .Subscribe(entities =>
                    IntersectAllEntitiesWith(entities, AllClassrooms, new Classroom.Comparer()))
                .DisposeWith(d);

            lecturerGateway
                .ObserveAll()
                .Catch<IEnumerable<Lecturer>, Exception>(ex =>
                    CatchObservableExceptions<Lecturer>(ex).ToObservable())
                .Subscribe(entities =>
                    IntersectAllEntitiesWith(entities, AllLecturers, new Lecturer.Comparer()))
                .DisposeWith(d);

            groupGateway
                .ObserveAll()
                .Catch<IEnumerable<Group>, Exception>(ex =>
                    CatchObservableExceptions<Group>(ex).ToObservable())
                .Subscribe(entities =>
                    IntersectAllEntitiesWith(entities, AllGroups, new Group.Comparer()))
                .DisposeWith(d);
        });

        SelectedGroup = exam?.Group;
        if (SelectedGroup is not null) AllGroups.Add(SelectedGroup);

        SelectedClassroom = exam?.Classroom;
        if (SelectedClassroom is not null) AllClassrooms.Add(SelectedClassroom);

        SelectedDiscipline = exam?.Discipline;
        if (SelectedDiscipline is not null) AllDisciplines.Add(SelectedDiscipline);

        SelectedLecturer = exam?.Lecturer;
        if (SelectedLecturer is not null) AllLecturers.Add(SelectedLecturer);

        UpdatePageName();

        var isYearValid = this.WhenAnyValue(
            vm => vm.Year,
            numericFieldValidatorFactory.Invoke
        );
        var isMonthValid = this.WhenAnyValue(
            vm => vm.Month,
            numericFieldValidatorFactory.Invoke
        );
        var isDayValid = this.WhenAnyValue(
            vm => vm.Day,
            numericFieldValidatorFactory.Invoke
        );
        var isHourValid = this.WhenAnyValue(
            vm => vm.Hour,
            numericFieldValidatorFactory.Invoke
        );
        var isMinuteValid = this.WhenAnyValue(
            vm => vm.Minute,
            numericFieldValidatorFactory.Invoke
        );

        this.ValidationRule(vm => vm.Year, isYearValid);
        this.ValidationRule(vm => vm.Month, isMonthValid);
        this.ValidationRule(vm => vm.Day, isDayValid);
        this.ValidationRule(vm => vm.Hour, isHourValid);
        this.ValidationRule(vm => vm.Minute, isMinuteValid);

        Save = ReactiveCommand.CreateFromTask(DoSave, this.IsValid());

        Save
            .IsExecuting
            .ToPropertyEx(this, vm => vm.IsLoading);

        this.WhenActivated(d =>
        {
            this.WhenAnyValue(
                    vm => vm.SelectedDiscipline,
                    vm => vm.SelectedClassroom,
                    vm => vm.SelectedGroup,
                    vm => vm.SelectedLecturer
                )
                .Subscribe(_ => UpdatePageName())
                .DisposeWith(d);
        });
    }

    private void UpdatePageName()
    {
        PageName = _localizedMessageConverter.Convert(LocalizedMessage.Letter.Exam) + " " +
                   SelectedGroup?.Name +
                   " " + SelectedDiscipline?.Name;
    }

    private async Task<IEnumerable<T>> CatchObservableExceptions<T>(Exception _)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            new LocalizedMessage.Error.StorageIsNotAvailable()
        );

        await OpenMessageDialog.Handle(messageDialog);

        return new T[] { };
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

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    private async Task DoSave(CancellationToken token)
    {
        if (await ValidateDateTime() is not { } startTime) return;
        if (!await ValidateSelectedEntities()) return;

        try
        {
            var exam = await _saveUseCase.Handle(
                SelectedLecturer!,
                SelectedDiscipline!,
                SelectedClassroom!,
                SelectedGroup!,
                startTime,
                _examId,
                token);

            _examId = exam.Id;
        }
        catch (GroupDoesNotStudyDiscipline)
        {
            var message =
                new LocalizedMessage.Error.GroupDoesNotStudyDiscipline(SelectedGroup!.Name,
                    SelectedDiscipline!.Name);
            await ShowErrorMessage(message);
        }
        catch (LecturerDoesNotAcceptDiscipline)
        {
            var message =
                new LocalizedMessage.Error.LecturerDoesNotAcceptDiscipline(SelectedLecturer!.FullName,
                    SelectedDiscipline!.Name);
            await ShowErrorMessage(message);
        }
        catch (ClassroomDoesNotMeetsRequirements)
        {
            var message =
                new LocalizedMessage.Error.ClassroomDoesNotMeetsRequirements(
                    SelectedClassroom!.Number, SelectedDiscipline!.Name);
            await ShowErrorMessage(message);
        }
        catch (SameExamAlreadyExists e)
        {
            var message =
                new LocalizedMessage.Error.SameExamAlreadyExists(e.Exam.Group.Name,
                    e.Exam.Discipline.Name);
            await ShowErrorMessage(message);
        }
        catch (ExamGatewayException)
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

    private async Task<bool> ValidateSelectedEntities()
    {
        if (SelectedClassroom is null)
        {
            var message = new LocalizedMessage.Error.ClassroomIsNotSelected();
            await ShowErrorMessage(message);
            return false;
        }

        if (SelectedDiscipline is null)
        {
            var message = new LocalizedMessage.Error.DisciplineIsNotSelected();
            await ShowErrorMessage(message);
            return false;
        }

        if (SelectedGroup is null)
        {
            var message = new LocalizedMessage.Error.GroupIsNotSelected();
            await ShowErrorMessage(message);
            return false;
        }

        if (SelectedLecturer is null)
        {
            var message = new LocalizedMessage.Error.LecturerIsNotSelected();
            await ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    private async Task<DateTime?> ValidateDateTime()
    {
        var year = int.Parse(Year);
        var month = int.Parse(Month);
        var day = int.Parse(Day);
        var hour = int.Parse(Hour);
        var minute = int.Parse(Minute);

        try
        {
            return new DateTime(year, month, day, hour, minute, 0);
        }
        catch (Exception)
        {
            var message = new LocalizedMessage.Error.EnteredDateTimeIsNotValid();
            await ShowErrorMessage(message);
            return null;
        }
    }

    [ObservableAsProperty] public bool IsLoading { get; }

    [Reactive] public Lecturer? SelectedLecturer { get; set; }

    public ObservableCollection<Lecturer> AllLecturers { get; }

    [Reactive] public Group? SelectedGroup { get; set; }

    public ObservableCollection<Group> AllGroups { get; }

    [Reactive] public Classroom? SelectedClassroom { get; set; }

    public ObservableCollection<Classroom> AllClassrooms { get; }

    [Reactive] public Discipline? SelectedDiscipline { get; set; }

    public ObservableCollection<Discipline> AllDisciplines { get; }

    [Reactive] public string Year { get; set; }

    [Reactive] public string Month { get; set; }

    [Reactive] public string Day { get; set; }

    [Reactive] public string Hour { get; set; }

    [Reactive] public string Minute { get; set; }

    [Reactive] public string PageName { get; private set; }

    public async Task<bool> ConfirmPageClosingAsync()
    {
        var action = LocalizedMessage.Letter.Close;
        var message = new LocalizedMessage.Question.CloseExamEditor();
        var dialog = _confirmDialogFactory.Invoke(action, message);

        return await OpenConfirmDialog.Handle(dialog);
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