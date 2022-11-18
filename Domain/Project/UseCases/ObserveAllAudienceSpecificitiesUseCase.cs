using Domain.Project.Models;
using Domain.Project.Repositories;

namespace Domain.Project.UseCases;

public class ObserveAllAudienceSpecificitiesUseCase
{
    private readonly IAudienceSpecificityRepository _specificitiesRepository;

    public ObserveAllAudienceSpecificitiesUseCase
        (IAudienceSpecificityRepository specificitiesRepository)
    {
        _specificitiesRepository = specificitiesRepository;
    }

    public IObservable<IEnumerable<IdentifiedModel<AudienceSpecificity>>> Handle()
    {
        return _specificitiesRepository.ObserveAll();
    }
}