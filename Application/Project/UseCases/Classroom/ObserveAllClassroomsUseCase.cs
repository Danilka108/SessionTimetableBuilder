using System.Reactive.Linq;
using Domain.Project.Repositories;

namespace Application.Project.UseCases.classroom;

public class ObserveAllClassroomsUseCase
{
    private readonly IAudienceRepository _audienceRepository;

    public ObserveAllClassroomsUseCase(IAudienceRepository audienceRepository)
    {
        _audienceRepository = audienceRepository;
    }

    public IObservable<IEnumerable<IdentifiedModel<Models.Audience>>> Handle()
    {
        return _audienceRepository
            .ObserveAll()
            .Catch<IEnumerable<IdentifiedModel<Models.Audience>>, Exception>
            (
                e => Observable.Throw<IEnumerable<IdentifiedModel<Models.Audience>>>
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