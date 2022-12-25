using Application.Project.Gateways;

namespace Application.Project.useCases.Lecturer;

public class DeleteLecturerUseCase
{
    private readonly ILecturerGateway _gateway;

    public DeleteLecturerUseCase(ILecturerGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(Domain.Project.Lecturer lecturer, CancellationToken token)
    {
        await _gateway.Delete(lecturer, token);
    }
}
