using System.Threading.Tasks;
using App.Project.AudienceEditor;
using App.Project.ExplorerCard;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.Audience;

namespace App.Project.AudienceCard;

public class AudienceCardViewModel : ExplorerCardViewModel
{
    public delegate AudienceCardViewModel Factory(IdentifiedModel<Audience> audience);

    private readonly IdentifiedModel<Audience> _audience;

    private readonly DeleteAudienceUseCase _deleteAudienceUseCase;
    private readonly AudienceEditorViewModel.Factory _editorViewModelFactory;

    public AudienceCardViewModel
    (
        IdentifiedModel<Audience> audience,
        DeleteAudienceUseCase deleteAudienceUseCase,
        AudienceEditorViewModel.Factory editorViewModelFactory
    )
    {
        _editorViewModelFactory = editorViewModelFactory;
        _deleteAudienceUseCase = deleteAudienceUseCase;
        _audience = audience;

        ConfirmDeleteMessage = $"Delete audience with number '{_audience.Model.Number}'?";
        Title = $"№ {_audience.Model.Number}";
    }

    protected override string ConfirmDeleteMessage { get; }
    public override string Title { get; }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(_audience);
    }

    protected override Task TryDoDelete()
    {
        return _deleteAudienceUseCase.Handle(_audience.Id);
    }
}