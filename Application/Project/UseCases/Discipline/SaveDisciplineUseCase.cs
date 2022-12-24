using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

public class SaveClassroomUseCase
{
    private readonly IDisciplineGateway _gateway;

    public SaveClassroomUseCase(IDisciplineGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(int? id, string name, 
        IEnumerable<Domain.Project.ClassroomFeature> requirements, CancellationToken token)
    {
        await CheckNameToOriginality(id, name, token);

        if (id is { } notNullId)
        {
            var classroom = new Domain.Project.Discipline(notNullId, name, requirements);
            await _gateway.Update(classroom, token);
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
            throw new ClassroomNumberMustBeOriginalException();
    }
}

public class ClassroomNumberMustBeOriginalException : Exception
{
}