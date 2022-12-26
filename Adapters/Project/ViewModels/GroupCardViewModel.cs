using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Application.Project.UseCases.Group;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class GroupCardViewModel : BaseViewModel
{
    public delegate GroupCardViewModel Factory(Group group, IBrowser browser);

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly DeleteGroupUseCase _deleteUseCase;

    private readonly IBrowser _browser;

    private readonly Group _group;

    public GroupCardViewModel(
        Group group,
        IBrowser browser,
        MessageDialogViewModel.Factory messageDialogFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        DeleteGroupUseCase deleteUseCase,
        GroupEditorViewModel.Factory editorFactory
    )
    {
        Name = group.Name;
        StudentsNumber = group.StudentsNumber;
        _group = group;

        _browser = browser;
        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;
        _deleteUseCase = deleteUseCase;

        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
        Edit = ReactiveCommand.CreateFromObservable(() =>
            _browser.Manager.Browse.Execute(editorFactory.Invoke(_group)));
    }

    public string Name { get; }

    public int StudentsNumber { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Letter.Delete,
            new LocalizedMessage.Question.DeleteGroup()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_group, token);
            await _browser.Manager.CloseByPageName.Execute(_group.Name);
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