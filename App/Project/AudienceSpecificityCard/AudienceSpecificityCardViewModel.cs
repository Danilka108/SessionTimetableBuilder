using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.CommonControls.ConfirmWindow;
using App.CommonControls.MessageWindow;
using App.Project.AudienceSpecificityEditor;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificityCard;

public class AudienceSpecificityCardViewModel : ViewModelBase
{
    public delegate AudienceSpecificityCardViewModel Factory
        (IdentifiedModel<AudienceSpecificity> specificity);

    private readonly DeleteAudienceSpecificityUseCase _deleteAudienceSpecificityUseCase;
    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;
    private readonly IdentifiedModel<AudienceSpecificity> _specificity;

    public AudienceSpecificityCardViewModel
    (
        IdentifiedModel<AudienceSpecificity> specificity,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory,
        DeleteAudienceSpecificityUseCase deleteAudienceSpecificityUseCase
    )
    {
        _deleteAudienceSpecificityUseCase = deleteAudienceSpecificityUseCase;
        _editorViewModelFactory = editorViewModelFactory;
        _specificity = specificity;

        OpenEditor = new Interaction<AudienceSpecificityEditorViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmWindowViewModel, bool>();
        OpenErrorMessageDialog = new Interaction<MessageWindowViewModel, Unit>();

        ShowEditor = ReactiveCommand.CreateFromTask(DoShowEditor);
        Delete = ReactiveCommand.CreateFromTask(DoDeleteSpecificity);
    }

    public Interaction<AudienceSpecificityEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<ConfirmWindowViewModel, bool> OpenConfirmDialog { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenErrorMessageDialog { get; }

    public ReactiveCommand<Unit, Unit> ShowEditor { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public string Description => _specificity.Model.Description;

    private async Task DoDeleteSpecificity()
    {
        var hasBeenConfirmed = await OpenConfirmDialog.Handle
        (
            new ConfirmWindowViewModel
                ("Confirm action", $"Delete '{Description}' audience specificity?", "Delete")
        );

        if (!hasBeenConfirmed) return;

        try
        {
            await _deleteAudienceSpecificityUseCase.Handle(_specificity.Id);
        }
        catch (Exception e)
        {
            await OpenErrorMessageDialog.Handle(new MessageWindowViewModel("Error", e.Message));
        }
    }

    private async Task DoShowEditor()
    {
        var viewModel = _editorViewModelFactory.Invoke
        (
            new IdentifiedModel<AudienceSpecificity>
                (_specificity.Id, new AudienceSpecificity(Description))
        );

        await OpenEditor.Handle(viewModel);
    }
}