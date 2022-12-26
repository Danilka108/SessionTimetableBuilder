using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.Discipline;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class DisciplineCardViewModel : BaseViewModel
{
    public delegate DisciplineCardViewModel Factory(Discipline discipline);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly DeleteDisciplineUseCase _deleteUseCase;

    private readonly Discipline _discipline;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public DisciplineCardViewModel(
        Discipline discipline,
        DisciplineEditorViewModel.Factory editorFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        MessageDialogViewModel.Factory messageDialogFactory,
        DeleteDisciplineUseCase deleteUseCase
    )
    {
        Name = discipline.Name;

        _discipline = discipline;
        _deleteUseCase = deleteUseCase;
        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;

        OpenEditor = new Interaction<DisciplineEditorViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        Edit = ReactiveCommand.CreateFromObservable(() =>
            OpenEditor.Handle(editorFactory.Invoke(_discipline)));

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    public string Name { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    public Interaction<DisciplineEditorViewModel, Unit> OpenEditor { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Letter.Delete,
            new LocalizedMessage.Question.DeleteDiscipline()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_discipline, token);
        }
        catch (DisciplineReferencedByLecturerException e)
        {
            var message =
                new LocalizedMessage.Error.DisciplineReferencedByLecturer(e.Lecturer.FullName);
            await ShowErrorMessage(message);
        }
        catch (DisciplineReferencedByGroupException e)
        {
            var message =
                new LocalizedMessage.Error.DisciplineReferencedByGroup(e.Group.Name);
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