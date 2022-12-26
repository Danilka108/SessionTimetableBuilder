using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Adapters.Project.Browser;
using Application.Project.Gateways;
using Application.Project.UseCases.Discipline;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ExamCardViewModel : BaseViewModel
{
    public delegate ExamCardViewModel Factory(Exam? exam, IBrowser browser);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    private readonly IExamGateway _gateway;

    private readonly IBrowser _browser;

    private readonly Exam _exam;

    public ExamCardViewModel(
        Exam exam,
        IBrowser browser,
        IExamGateway gateway,
        ExamEditorViewModel.Factory editorFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        MessageDialogViewModel.Factory messageDialogFactory
    )
    {
        _exam = exam;
        _gateway = gateway;
        _browser = browser;

        GroupName = exam.Group.Name;
        DisciplineName = exam.Discipline.Name;

        _confirmDialogFactory = confirmDialogFactory;
        _messageDialogFactory = messageDialogFactory;

        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();

        Edit = ReactiveCommand.CreateFromObservable(() =>
            browser.Manager.Browse.Execute(editorFactory.Invoke(exam)));
        
        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    public string GroupName { get; }

    public string DisciplineName { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Letter.Delete,
            new LocalizedMessage.Question.DeleteExam()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _gateway.Delete(_exam, token);
            // _browser.Manager.CloseByPageName(_exam.)
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

    private async Task ShowErrorMessage(LocalizedMessage message)
    {
        var messageDialog = _messageDialogFactory.Invoke(
            LocalizedMessage.Letter.Error,
            message
        );

        await OpenMessageDialog.Handle(messageDialog);
    }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }
}