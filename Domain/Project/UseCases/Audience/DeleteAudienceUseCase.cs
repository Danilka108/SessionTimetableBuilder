using Domain.Project.Repositories;

namespace Domain.Project.UseCases;

public class DeleteAudienceUseCase
{
    private readonly IAudienceRepository _audienceRepository;

    public DeleteAudienceUseCase(IAudienceRepository audienceRepository)
    {
        _audienceRepository = audienceRepository;
    }

    public async Task Handle(int id)
    {
        try
        {
            var token = CancellationToken.None;
            await _audienceRepository.Delete(id, token);
        }
        catch (Exception e)
        {
            throw new DeleteAudienceException("Failed to delete audience", e);
        }
    }
}

public class DeleteAudienceException : Exception
{
    internal DeleteAudienceException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}