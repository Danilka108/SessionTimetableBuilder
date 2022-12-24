using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

public class SaveDisciplineUseCase
{
    private readonly IDisciplineGateway _gateway;

    public SaveDisciplineUseCase(IDisciplineGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(int? id, string name, 
        IEnumerable<Domain.Project.ClassroomFeature> requirements, CancellationToken token)
    {
        await CheckNameToOriginality(id, name, token);

        if (id is { } notNullId)
        {
            var discipline = new Domain.Project.Discipline(notNullId, name, requirements);
            await _gateway.Update(discipline, token);
            return;
        }

        await _gateway.Create(name, requirements, token);
    }

    private async Task CheckNameToOriginality(int? id, string name, CancellationToken token)
    {
        var allDisciplines = await _gateway.ReadAll(token);
        var disciplineWithSameName =
            allDisciplines.FirstOrDefault(discipline => discipline.Name == name);

        if (disciplineWithSameName?.Id == id) return;

        if (disciplineWithSameName is { })
            throw new DisciplineNameMustBeOriginalException();
    }
}

public class DisciplineNameMustBeOriginalException : Exception
{
}