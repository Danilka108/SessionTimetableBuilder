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

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly DeleteClassroomUseCase _deleteUseCase;
    
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
    
    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Header.Delete,
            new LocalizedMessage.Question.DeleteClassroom()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_classroom, token);
        }
        catch (ClassroomGatewayException)
        {
            var messageDialog = _messageDialogFactory.Invoke(
                LocalizedMessage.Header.Error,
                new LocalizedMessage.Error.StorageIsNotAvailable()
            );

            await OpenMessageDialog.Handle(messageDialog);
        }
        catch (Exception)
        {
            var messageDialog = _messageDialogFactory.Invoke(
                LocalizedMessage.Header.Error,
                new LocalizedMessage.Error.UndefinedError()
            );

            await OpenMessageDialog.Handle(messageDialog);
        }
    }
    
    public int Number { get; }
    
    public int Capacity { get; }
    
    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ClassroomEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }
}