using System.Reactive.Linq;
using Domain.Project.Models;
using Domain.Project.Repositories;
using Domain.Project.UseCases.BellTime;

namespace Domain.Project.UseCases;

public class ObserveAllAudiencesUseCase
{
    private readonly IAudienceRepository _audienceRepository;

    public ObserveAllAudiencesUseCase(IAudienceRepository audienceRepository)
    {
        _audienceRepository = audienceRepository;
    }

    public IObservable<IEnumerable<IdentifiedModel<Audience>>> Handle()
    {
        return _audienceRepository
            .ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Audience>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Audience>>>
                    (new ObserveAllAudiencesException("Failed to get all audiences", e))
            );
    }
}

public class ObserveAllAudiencesException : Exception
{
    internal ObserveAllAudiencesException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}