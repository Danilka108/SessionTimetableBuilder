using Domain.Project.Models;

namespace Domain.Project.UseCases;

public class ObserveAllAudienceSpecificitiesUseCase
{
    private readonly IRepository<AudienceSpecificity> _specificitiesRepository;

    public ObserveAllAudienceSpecificitiesUseCase(IRepository<AudienceSpecificity> specificitiesRepository)
    {
        _specificitiesRepository = specificitiesRepository;
    }

    public IObservable<IEnumerable<IdentifiedModel<AudienceSpecificity>>> Handle()
    {
        return _specificitiesRepository.ObserveAll();
    }
}