using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Application.Project.Gateways;
using Application.Project.UseCases.ClassroomFeature;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureCardViewModel : BaseViewModel
{
    public delegate ClassroomFeatureCardViewModel Factory(ClassroomFeature feature);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly DeleteClassroomFeatureUseCase _deleteUseCase;

    private readonly ClassroomFeature _feature;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomFeatureCardViewModel(
        ClassroomFeature feature,
        ClassroomFeatureEditorViewModel.Factory editorFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        MessageDialogViewModel.Factory messageDialogFactory,
        DeleteClassroomFeatureUseCase deleteUseCase
    )
    {
        _feature = feature;
        Description = feature.Description;

        _deleteUseCase = deleteUseCase;

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenEditor = new Interaction<ClassroomFeatureEditorViewModel, Unit>();

        _messageDialogFactory = messageDialogFactory;
        _confirmDialogFactory = confirmDialogFactory;

        Edit = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenEditor.Handle(editorFactory.Invoke(feature));
        });

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    public string Description { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ClassroomFeatureEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmDialog = _confirmDialogFactory.Invoke(
            LocalizedMessage.Header.Delete,
            new LocalizedMessage.Question.DeleteClassroomFeature()
        );

        var confirmed = await OpenConfirmDialog.Handle(confirmDialog);
        if (!confirmed) return;

        try
        {
            await _deleteUseCase.Handle(_feature, token);
        }
        catch (ClassroomFeatureReferencedByClassroomException e)
        {
            var message =
                new LocalizedMessage.Error.ClassroomFeatureReferencedByClassroom(e.Classroom
                    .Number);

            await ShowErrorMessage(message);
        }
        catch (ClassroomFeatureReferencedByDisciplineException e)
        {
            var message =
                new LocalizedMessage.Error.ClassroomFeatureReferencedByDiscipline(e.Discipline
                    .Name);

            await ShowErrorMessage(message);
        }
        catch (ClassroomFeatureGatewayException)
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
}