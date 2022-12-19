using System.Reactive.Linq;
using Domain.Project.Repositories;

namespace Domain.Project.UseCases.AudienceSpecificity;

public class ObserveAllAudienceSpecificitiesUseCase
{
    private readonly IClassroomFeatureGateWay _specificitiesGateWay;

    public ObserveAllAudienceSpecificitiesUseCase
        (IClassroomFeatureGateWay specificitiesGateWay)
    {
        _specificitiesGateWay = specificitiesGateWay;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.AudienceSpecificity>>> Handle()
    {
        return _specificitiesGateWay.ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Models.AudienceSpecificity>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Models.AudienceSpecificity>>>
                (
                    new ObserveAllAudienceSpecificitiesException
                        ("Failed to get all audience specificities", e)
                )
            );
    }
}

public class ObserveAllAudienceSpecificitiesException : Exception
{
    internal ObserveAllAudienceSpecificitiesException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}