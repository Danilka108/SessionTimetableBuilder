using Application.Project.Gateways;
using Domain.Project;

namespace Application.Project.UseCases.ClassroomFeature;

public class SaveClassroomFeatureUseCase
{
    private readonly IClassroomFeatureGateway _featureGateway;

    public SaveClassroomFeatureUseCase
        (IClassroomFeatureGateway featureGateway)
    {
        _featureGateway = featureGateway;
    }

    public async Task Handle
        (Domain.Project.ClassroomFeature feature, int? id = null)
    {
        var token = CancellationToken.None;

        await CheckDescriptionToOriginality(feature.Description, token, id);

        if (id is { } notNullId)
            await Update(feature, notNullId, token);
        else
            await Create(feature, token);
    }

    private async Task CheckDescriptionToOriginality
        (string description, CancellationToken token, int? id = null)
    {
        var allFeatures = await _featureGateway.ReadAll(token);

        var featureWithSameDescription = allFeatures
            .FirstOrDefault
                (feature => description == feature.Entity.Description);

        if (featureWithSameDescription?.Id == id) return;

        if (featureWithSameDescription is { })
            throw new SaveClassroomFeatureException
                ("Description of classroom feature must be original");
    }

    private async Task Create
        (Domain.Project.ClassroomFeature feature, CancellationToken token)
    {
        try
        {
            await _featureGateway.Create(feature, token);
        }
        catch (Exception e)
        {
            throw new SaveClassroomFeatureException
                ("Failed to create new classroom feature", e);
        }
    }

    private async Task Update
        (Domain.Project.ClassroomFeature feature, int id, CancellationToken token)
    {
        try
        {
            var identifiedFeature =
                new Identified<Domain.Project.ClassroomFeature>(id, feature);

            await _featureGateway.Update
                (identifiedFeature, token);
        }
        catch (Exception e)
        {
            throw new SaveClassroomFeatureException
            (
                $"Failed to update classroom feature with '{id}'",
                e
            );
        }
    }
}

public class SaveClassroomFeatureException : Exception
{
    internal SaveClassroomFeatureException
        (string msg, Exception innerException) : base(msg, innerException)
    {
    }

    internal SaveClassroomFeatureException(string msg) : base(msg)
    {
    }
}