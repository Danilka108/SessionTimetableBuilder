using Domain;

namespace App.Project.AudienceSpecificity;

public class AudienceSpecificityViewModel : ViewModelBase
{
    public delegate AudienceSpecificityViewModel Factory(
        IdentifiedModel<Domain.Project.Models.AudienceSpecificity> specificity);

    public AudienceSpecificityViewModel(IdentifiedModel<Domain.Project.Models.AudienceSpecificity> specificity)
    {
        Description = specificity.Model.Description;
    }

    public string Description { get; }
}