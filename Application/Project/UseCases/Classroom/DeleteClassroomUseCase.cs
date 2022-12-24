using Application.Project.Gateways;

namespace Application.Project.UseCases.Classroom;

public class DeleteClassroomUseCase
{
    private readonly IClassroomGateway _gateway;

    public DeleteClassroomUseCase(IClassroomGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(Domain.Project.Classroom classroom, CancellationToken token)
    {
        await _gateway.Delete(classroom, token);
    }
}