using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Classroom;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomCardViewModel : BaseViewModel
{
    public delegate ClassroomCardViewModel Factory(Classroom classroom);

    private readonly Classroom _classroom;

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly DeleteClassroomUseCase _deleteUseCase;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomCardViewModel(
        Classroom classroom,
        DeleteClassroomUseCase deleteUseCase,
        MessageDialogViewModel.Factory messageDialogFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        ClassroomEditorViewModel.Factory editorFactory)
    {
        _classroom = classroom;
        _deleteUseCase = deleteUseCase;

        Number = _classroom.Number;
        Capacity = _classroom.Capacity;

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenEditor = new Interaction<ClassroomEditorViewModel, Unit>();

        _messageDialogFactory = messageDialogFactory;
        _confirmDialogFactory = confirmDialogFactory;

        Edit = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenEditor.Handle(editorFactory.Invoke(_classroom));
        });

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    public int Number { get; }

    public int Capacity { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ClassroomEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Letter.Delete,
            new LocalizedMessage.Question.DeleteClassroom()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_classroom, token);
        }
        catch (ClassroomReferencedByExamException e)
        {
            await ShowErrorMessage(
                new LocalizedMessage.Error.ClassroomReferencedByExam(e.Exam.Group.Name,
                    e.Exam.Discipline.Name));
        }
        catch (ClassroomGatewayException)
        {
            await ShowErrorMessage(new LocalizedMessage.Error.StorageIsNotAvailable());
        }
        catch (Exception)
        {
            await ShowErrorMessage(new LocalizedMessage.Error.UndefinedError());
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