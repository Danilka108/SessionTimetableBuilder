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

    private readonly int _id;
    private string _description;

    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;
    private readonly DeleteAudienceSpecificityUseCase _deleteUseCase;

    public AudienceSpecificityCardViewModel
    (
        IdentifiedModel<AudienceSpecificity> specificity,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory,
        DeleteAudienceSpecificityUseCase deleteUseCase
    )
    {
        _id = specificity.Id;
        Description = specificity.Model.Description;

        _editorViewModelFactory = editorViewModelFactory;
        _deleteUseCase = deleteUseCase;

        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmWindowViewModel, bool>();
        OpenEditor = new Interaction<AudienceSpecificityEditorViewModel, Unit>();

        ShowEditor = ReactiveCommand.CreateFromTask(DoShowEditor);
        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }

    private async Task DoShowEditor()
    {
        var specificity = new IdentifiedModel<AudienceSpecificity>
            (_id, new AudienceSpecificity(Description));
        var editorViewModel = _editorViewModelFactory.Invoke(specificity);

        await OpenEditor.Handle(editorViewModel);
    }

    private async Task DoDelete()
    {
        var confirmViewModel = new ConfirmWindowViewModel
        (
            "Confirm deletion",
            $"Delete '{Description}' audience specificity?",
            "Delete"
        );
        var hasBeenConfirmed = await OpenConfirmDialog.Handle(confirmViewModel);

        if (!hasBeenConfirmed) return;

        try
        {
            await _deleteUseCase.Handle(_id);
        }
        catch (Exception e)
        {
            var messageViewModel = new MessageWindowViewModel("Error", e.Message);
            await OpenMessageDialog.Handle(messageViewModel);
        }
    }

    public Interaction<AudienceSpecificityEditorViewModel, Unit> OpenEditor { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }
    public Interaction<ConfirmWindowViewModel, bool> OpenConfirmDialog { get; }

    public ReactiveCommand<Unit, Unit> ShowEditor { get; }

    public ReactiveCommand<Unit, Unit> Delete { get; }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
}