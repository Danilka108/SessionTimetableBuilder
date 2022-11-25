using Domain.Project.Repositories;

namespace Domain.Project.UseCases.AudienceSpecificity;

public class SaveAudienceSpecificityUseCase
{
    private readonly IAudienceSpecificityRepository _specificityRepository;

    public SaveAudienceSpecificityUseCase
        (IAudienceSpecificityRepository specificityRepository)
    {
        _specificityRepository = specificityRepository;
    }

    public async Task Handle
        (Models.AudienceSpecificity specificity, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckDescriptionToOriginality(specificity.Description, token, id);

        if (id is { } notNullId)
            await Update(specificity, notNullId, token);
        else
            await Create(specificity, token);
    }

    private async Task CheckDescriptionToOriginality
        (string description, CancellationToken token, int? id = null)
    {
        var allSpecificities = await _specificityRepository.ReadAll(token);

        var specificityWithSameDescription = allSpecificities
            .FirstOrDefault
                (specificity => description == specificity.Model.Description);

        if (specificityWithSameDescription?.Id == id) return;

        if (specificityWithSameDescription is { })
            throw new SaveAudienceSpecificityException
                ("Description of audience specificity must be original");
    }

    private async Task Create
        (Models.AudienceSpecificity specificity, CancellationToken token)
    {
        try
        {
            await _specificityRepository.Create(specificity, token);
        }
        catch (Exception e)
        {
            throw new SaveAudienceSpecificityException
                ("Failed to create new audience specificity", e);
        }
    }

    private async Task Update
        (Models.AudienceSpecificity specificity, int id, CancellationToken token)
    {
        try
        {
            var identifiedSpecificity = new IdentifiedModel<Models.AudienceSpecificity>(id, specificity);

            await _specificityRepository.Update
                (identifiedSpecificity, token);
        }
        catch (Exception e)
        {
            throw new SaveAudienceSpecificityException
            (
                $"Failed to update audience specificity with '{id}'",
                e
            );
        }
    }
}

public class SaveAudienceSpecificityException : Exception
{
    internal SaveAudienceSpecificityException
        (string msg, Exception innerException) : base(msg, innerException)
    {
    }

    internal SaveAudienceSpecificityException(string msg) : base(msg)
    {
    }
}