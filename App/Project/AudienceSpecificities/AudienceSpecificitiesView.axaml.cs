using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.AudienceSpecificities;

public partial class AudienceSpecificitiesView : ReactiveUserControl<AudienceSpecificitiesViewModel>
{
    public AudienceSpecificitiesView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}