using Application.Project.Gateways;

namespace Application.Project.UseCases.ClassroomFeature;

public class DeleteClassroomFeatureUseCase
{
    private readonly IClassroomFeatureGateway _featureGateway;

    public DeleteClassroomFeatureUseCase
        (IClassroomFeatureGateway featureGateway)
    {
        _featureGateway = featureGateway;
    }

    public async Task Handle(Domain.Project.ClassroomFeature feature, CancellationToken token)
    {
        try
        {
            await _featureGateway.Delete(feature, token);
        }
        catch (Exception e)
        {
            throw new DeleteClassroomFeatureException("Failed to delete classroom feature", e);
        }
    }
}

public class DeleteClassroomFeatureException : Exception
{
    internal DeleteClassroomFeatureException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}