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
        {
            var feature = new Domain.Project.ClassroomFeature(notNullId, description);
            await _featureGateway.Update(feature, token);
        }
        else
        {
            await _featureGateway.Create(description, token);
        }
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
}

public class NotOriginalDescriptionException : Exception
{
}