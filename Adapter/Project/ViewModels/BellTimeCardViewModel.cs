using System.Threading.Tasks;
using App.Project.BellTimeEditor;
using App.Project.ExplorerCard;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.BellTime;

namespace App.Project.BellTimeCard;

public class BellTimeCardViewModel : ExplorerCardViewModel
{
    public delegate BellTimeCardViewModel Factory(IdentifiedModel<BellTime> bellTime);

    private readonly IdentifiedModel<BellTime> _bellTime;

    private readonly DeleteBellTimeUseCase _deleteUseCase;
    private readonly BellTimeEditorViewModel.Factory _editorViewModelFactory;

    public BellTimeCardViewModel
    (
        IdentifiedModel<BellTime> bellTime,
        DeleteBellTimeUseCase deleteUseCase,
        BellTimeEditorViewModel.Factory editorViewModelFactory
    )
    {
        _editorViewModelFactory = editorViewModelFactory;
        _deleteUseCase = deleteUseCase;
        _bellTime = bellTime;

        Title =
            (bellTime.Model.Hour < 10 ? "0" : "") + bellTime.Model.Hour +
            ":" +
            (bellTime.Model.Minute < 10 ? "0" : "") + bellTime.Model.Minute;

        ConfirmDeleteMessage = $"Delete bell time '{Title}'?";
    }

    protected override string ConfirmDeleteMessage { get; }
    public sealed override string Title { get; }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(_bellTime);
    }

    protected override async Task TryDoDelete()
    {
        await _deleteUseCase.Handle(_bellTime.Id);
    }
}