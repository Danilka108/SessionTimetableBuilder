using Application.Project.Gateways;

namespace Application.Project.UseCases.ClassroomFeature;

public class SaveClassroomFeatureUseCase
{
    private readonly IClassroomFeatureGateway _featureGateway;

    public SaveClassroomFeatureUseCase
        (IClassroomFeatureGateway featureGateway)
    {
        _featureGateway = featureGateway;
    }

    public async Task Handle(int? id, string description, CancellationToken token)
    {
        await CheckDescriptionToOriginality(description, token, id);

        if (id is { } notNullId)
            await Update(notNullId, description, token);
        else
            await Create(description, token);
    }

    private async Task CheckDescriptionToOriginality
        (string description, CancellationToken token, int? id = null)
    {
        var allFeatures = await _featureGateway.ReadAll(token);

        var featureWithSameDescription = allFeatures
            .FirstOrDefault
                (feature => description == feature.Description);

        if (featureWithSameDescription?.Id == id) return;

        if (featureWithSameDescription is { })
            throw new NotOriginalDescriptionException();
    }

    private async Task Create(string description, CancellationToken token)
    {
        try
        {
            await _featureGateway.Create(description, token);
        }
        catch (Exception e)
        {
            throw new CreateClassroomFeatureException();
        }
    }

    private async Task Update(int id, string description, CancellationToken token)
    {
        var feature = new Domain.Project.ClassroomFeature(id, description);
        try
        {
            await _featureGateway.Update(feature, token);
        }
        catch (Exception)
        {
            throw new UpdateClassroomFeatureException();
        }
    }
}

public class NotOriginalDescriptionException : Exception {}

public class CreateClassroomFeatureException : Exception {}

public class UpdateClassroomFeatureException : Exception {}