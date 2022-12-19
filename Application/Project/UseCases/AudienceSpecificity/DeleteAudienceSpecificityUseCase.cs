using Domain.Project.Repositories;

namespace Domain.Project.UseCases.AudienceSpecificity;

public class DeleteAudienceSpecificityUseCase
{
    private readonly IClassroomFeatureGateWay _specificityGateWay;

    public DeleteAudienceSpecificityUseCase
        (IClassroomFeatureGateWay specificityGateWay)
    {
        _specificityGateWay = specificityGateWay;
    }

    public async Task Handle(int id)
    {
        var token = CancellationToken.None;

        try
        {
            await _specificityGateWay.Delete(id, token);
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