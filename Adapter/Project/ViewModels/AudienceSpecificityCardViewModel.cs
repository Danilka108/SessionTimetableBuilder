using System.Threading.Tasks;
using App.Project.AudienceSpecificityEditor;
using App.Project.ExplorerCard;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.AudienceSpecificity;

namespace App.Project.AudienceSpecificityCard;

public class AudienceSpecificityCardViewModel : ExplorerCardViewModel
{
    public delegate AudienceSpecificityCardViewModel Factory
        (IdentifiedModel<AudienceSpecificity> specificity);

    private readonly DeleteAudienceSpecificityUseCase _deleteUseCase;

    private readonly AudienceSpecificityEditorViewModel.Factory _editorViewModelFactory;

    private readonly int _id;

    public AudienceSpecificityCardViewModel
    (
        IdentifiedModel<AudienceSpecificity> specificity,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory,
        DeleteAudienceSpecificityUseCase deleteUseCase
    )
    {
        _id = specificity.Id;
        ConfirmDeleteMessage = $"Delete audience '{specificity.Model.Description}' specificity?";
        Title = specificity.Model.Description;

        _editorViewModelFactory = editorViewModelFactory;
        _deleteUseCase = deleteUseCase;
    }

    protected override string ConfirmDeleteMessage { get; }
    public override string Title { get; }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        var specificity = new IdentifiedModel<AudienceSpecificity>
            (_id, new AudienceSpecificity(Title));

        return _editorViewModelFactory.Invoke
            (specificity);
    }

    protected override async Task TryDoDelete()
    {
        await _deleteUseCase.Handle(_id);
    }
}