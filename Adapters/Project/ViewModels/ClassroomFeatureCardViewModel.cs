using System.Reactive;
using System.Reactive.Linq;
using Adapters.Common.ViewModels;
using Adapters.ViewModels;
using Application.Project.Gateways;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureCardViewModel : BaseViewModel
{
    public delegate ClassroomFeatureCardViewModel Factory(Identified<ClassroomFeature> feature);

    private readonly ConfirmDialogViewModel.Factory _confirmDialogFactory;

    private readonly IClassroomFeatureGateway _gateway;

    private readonly Identified<ClassroomFeature> _feature;

    private readonly MessageDialogViewModel.Factory _messageDialogFactory;

    public ClassroomFeatureCardViewModel(
        Identified<ClassroomFeature> feature,
        ClassroomFeatureEditorViewModel.Factory editorFactory,
        ConfirmDialogViewModel.Factory confirmDialogFactory,
        MessageDialogViewModel.Factory messageDialogFactory,
        IClassroomFeatureGateway gateway
    )
    {
        _feature = feature;
        Description = feature.Entity.Description;

        OpenMessageDialog = new Interaction<MessageDialogViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmDialogViewModel, bool>();
        OpenEditor = new Interaction<ClassroomFeatureEditorViewModel, Unit>();

        _gateway = gateway;
        _messageDialogFactory = messageDialogFactory;
        _confirmDialogFactory = confirmDialogFactory;

        Edit = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenEditor.Handle(editorFactory.Invoke(feature));
        });

        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    private async Task DoDelete(CancellationToken token)
    {
        var confirmed = await OpenConfirmDialog.Handle(_confirmDialogFactory.Invoke(
            "Delete",
            $"Delete '{Description}' classroom feature?")
        );

        if (!confirmed) return;

        try
        {
            await _gateway.Delete(_feature.Id, token);
        }
        catch (Exception e)
        {
            await OpenMessageDialog.Handle(_messageDialogFactory.Invoke("Error", e.Message));
        }
    }

    public string Description { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ClassroomFeatureEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<ConfirmDialogViewModel, bool> OpenConfirmDialog { get; }
    
    public Interaction<MessageDialogViewModel, Unit> OpenMessageDialog { get; }
}