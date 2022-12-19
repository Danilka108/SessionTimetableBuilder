using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.UseCases.Discipline;

public class ReadAllDisciplinesUseCase
{
    private readonly IDisciplineGateway _gateway;
    
    public ReadAllDisciplinesUseCase(IDisciplineGateway gateway)
    {
        _gateway = gateway;
    }

    public Task<IEnumerable<Identified<Domain.Project.Discipline>>> Handle()
    {
        return _gateway.ReadAll(CancellationToken.None);
    }
}