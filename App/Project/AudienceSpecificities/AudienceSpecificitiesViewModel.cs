using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using App.Project.AudienceSpecificity;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public class AudienceSpecificitiesViewModel : ViewModelBase
{
    public delegate AudienceSpecificitiesViewModel Factory();

    private readonly ObservableAsPropertyHelper<IEnumerable<AudienceSpecificityViewModel>> _specificities;

    public AudienceSpecificitiesViewModel(ObserveAllSpecificitiesUseCase observeAllSpecificitiesUseCase,
        AudienceSpecificityViewModel.Factory specificityViewModelFactory)
    {
        _specificities = observeAllSpecificitiesUseCase
            .Handle()
            .Select(specificities => specificities.Select(specificityViewModelFactory.Invoke))
            .ToProperty(this, vm => vm.Specificities);
    }

    public IEnumerable<AudienceSpecificityViewModel> Specificities => _specificities.Value;
}