using Domain.Project.Repositories;

namespace Domain.Project.UseCases;

public class DeleteAudienceSpecificityUseCase
{
    private readonly IAudienceSpecificityRepository _specificityRepository;

    public DeleteAudienceSpecificityUseCase
        (IAudienceSpecificityRepository specificityRepository)
    {
        _specificityRepository = specificityRepository;
    }

    public async Task Handle(int id)
    {
        var token = CancellationToken.None;

        try
        {
            await _specificityRepository.Delete(id, token);
        }
        catch (Exception e)
        {
            throw new DeleteAudienceSpecificityException
                ("Failed to delete audience specificity", e);
        }
    }
}

public class DeleteAudienceSpecificityException : Exception
{
    internal DeleteAudienceSpecificityException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}