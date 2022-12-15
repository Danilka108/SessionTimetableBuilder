using System.Threading.Tasks;
using App.Project.DisciplineEditor;
using App.Project.ExplorerCard;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases.Discipline;

namespace App.Project.DisciplineCard;

public class DisciplineCardViewModel : ExplorerCardViewModel
{
    public delegate DisciplineCardViewModel Factory(IdentifiedModel<Discipline> discipline);

    private readonly DeleteDisciplineUseCase _deleteUseCase;
    private readonly IdentifiedModel<Discipline> _discipline;
    private readonly DisciplineEditorViewModel.Factory _editorViewModelFactory;

    public DisciplineCardViewModel
    (IdentifiedModel<Discipline> discipline, DeleteDisciplineUseCase deleteUseCase,
        DisciplineEditorViewModel.Factory editorViewModelFactory)
    {
        _editorViewModelFactory = editorViewModelFactory;
        _deleteUseCase = deleteUseCase;
        _discipline = discipline;

        Title = discipline.Model.Name;
        ConfirmDeleteMessage = $"Delete discipline with name '{discipline.Model.Name}'?";
    }

    protected override string ConfirmDeleteMessage { get; }
    public sealed override string Title { get; }

    protected override ViewModelBase ProvideEditorViewModel()
    {
        return _editorViewModelFactory.Invoke(_discipline);
    }

    protected override async Task TryDoDelete()
    {
        await _deleteUseCase.Handle(_discipline.Id);
    }
}