using Domain;
using ProjectDomain.Models;

namespace ProjectDomain.UseCases;

public class ObserveAllSpecificitiesUseCase
{
    private readonly IRepository<AudienceSpecificity> _specifictiesRepository;

    public ObserveAllSpecificitiesUseCase(IRepository<AudienceSpecificity> specificitiesRepository)
    {
        _specifictiesRepository = specificitiesRepository;
    }

    public IObservable<AudienceSpecificity> Handle()
    {
        _specifictiesRepository.Observe()
    }
}