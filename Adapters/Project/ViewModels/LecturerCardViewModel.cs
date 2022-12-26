using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Application.Project.UseCases.Discipline;
using Application.Project.useCases.Lecturer;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class LecturerCardViewModel : BaseViewModel
{
    public delegate LecturerCardViewModel Factory(Lecturer lecturer, IBrowser browser);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly DeleteLecturerUseCase _deleteUseCase;

    private readonly Lecturer _lecturer;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly IBrowser _browser;

    public LecturerCardViewModel(
        Lecturer lecturer,
        IBrowser browser,
        MessageDialogViewModel.Factory messageDialogFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        DeleteLecturerUseCase deleteUseCase,
        LecturerEditorViewModel.Factory editorFactory)
    {
        _lecturer = lecturer;
        FullName = _lecturer.FullName;

        _browser = browser;
        _messageDialogFactory = messageDialogFactory;
        _confirmDialogFactory = confirmDialogFactory;
        _deleteUseCase = deleteUseCase;

        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        Edit = ReactiveCommand.CreateFromObservable(() =>
            browser.Manager.Browse.Execute(editorFactory.Invoke(_lecturer)));

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    public string FullName { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Letter.Delete,
            new LocalizedMessage.Question.DeleteLecturer()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_lecturer, token);
            await _browser.Manager.CloseByPageName.Execute(_lecturer.FullName);
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