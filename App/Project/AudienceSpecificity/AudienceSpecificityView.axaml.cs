using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.AudienceSpecificity;

public partial class AudienceSpecificityView : ReactiveUserControl<AudienceSpecificityViewModel>
{
    public AudienceSpecificityView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}