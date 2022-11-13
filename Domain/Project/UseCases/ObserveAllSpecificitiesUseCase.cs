using Domain.Project.Models;

namespace Domain.Project.UseCases;

public class ObserveAllSpecificitiesUseCase
{
    private readonly IRepository<AudienceSpecificity> _specificitiesRepository;

    public ObserveAllSpecificitiesUseCase(IRepository<AudienceSpecificity> specificitiesRepository)
    {
        _specificitiesRepository = specificitiesRepository;
    }

    public IObservable<IEnumerable<IdentifiedModel<AudienceSpecificity>>> Handle()
    {
        return _specificitiesRepository.ObserveAll();
    }
}