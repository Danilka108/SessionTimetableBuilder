using Application.Project.Gateways;

namespace Application.Project.UseCases.Discipline;

public class DeleteDisciplineUseCase
{
    private readonly IDisciplineGateway _gateway;

    public DeleteDisciplineUseCase(IDisciplineGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(Domain.Project.Discipline discipline, CancellationToken token)
    {
        await _gateway.Delete(discipline, token);
    }
}