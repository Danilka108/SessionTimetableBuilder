using Adapters.Project.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeaturesView : ReactiveUserControl<ClassroomFeaturesViewModel>
{
    public ClassroomFeaturesView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}