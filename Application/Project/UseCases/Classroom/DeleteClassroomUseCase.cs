
namespace Application.Project.UseCases.Classroom;

public class DeleteClassroomUseCase
{
    private readonly I _gateway;

    public DeleteClassroomUseCase(IAudienceRepository gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(int id)
    {
        try
        {
            var token = CancellationToken.None;
            await _gateway.Delete(id, token);
        }
        catch (Exception e)
        {
            throw new DeleteAudienceException("Failed to delete audience", e);
        }
    }
}

public class DeleteAudienceException : Exception
{
    internal DeleteAudienceException(string msg, Exception innerException) : base(msg,
        innerException)
    {
    }
}