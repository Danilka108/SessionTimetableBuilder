using Domain.Project.Repositories;

namespace Domain.Project.UseCases.Audience;

public class SaveAudienceUseCase
{
    private readonly IAudienceRepository _audienceRepository;

    public SaveAudienceUseCase(IAudienceRepository audienceRepository)
    {
        _audienceRepository = audienceRepository;
    }

    public async Task Handle(Models.Audience audience, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckNumberToOriginality(token, audience.Number, id);

        if (id is { } notNullId)
        {
            await Update(audience, notNullId, token);
            return;
        }

        await Create(audience, token);
    }

    private async Task CheckNumberToOriginality(CancellationToken token, int number, int? id = null)
    {
        var allAudiences = await _audienceRepository.ReadAll(token);
        var audienceWithSaveNumber = allAudiences.FirstOrDefault(a => a.Model.Number == number);

        if (audienceWithSaveNumber?.Id == id) return;

        if (audienceWithSaveNumber is { })
            throw new SaveAudienceException("Number of audience must be original");
    }

    private async Task Create(Models.Audience audience, CancellationToken token)
    {
        try
        {
            await _audienceRepository.Create(audience, token);
        }
        catch (Exception e)
        {
            throw new SaveAudienceException
                ($"Failed to create audience with number '{audience.Number}'", e);
        }
    }

    private async Task Update(Models.Audience audience, int id, CancellationToken token)
    {
        try
        {
            await _audienceRepository.Update(new IdentifiedModel<Models.Audience>(id, audience), token);
        }
        catch (Exception e)
        {
            throw new SaveAudienceException
                ($"Failed to update audience with number '{audience.Number}'", e);
        }
    }
}

public class SaveAudienceException : Exception
{
    internal SaveAudienceException(string msg) : base(msg)
    {
    }

    internal SaveAudienceException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}