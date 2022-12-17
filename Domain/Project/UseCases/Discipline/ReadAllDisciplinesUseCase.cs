using Domain.Project.Repositories;

namespace Domain.Project.UseCases.Discipline;

public class ReadAllDisciplinesUseCase
{
    private readonly IDisciplineRepository _disciplineRepository;
    
    public ReadAllDisciplinesUseCase(IDisciplineRepository disciplineRepository)
    {
        _disciplineRepository = disciplineRepository;
    }

    public Task<IEnumerable<IdentifiedModel<Models.Discipline>>> Handle()
    {
        return _disciplineRepository.ReadAll(CancellationToken.None);
    }
}