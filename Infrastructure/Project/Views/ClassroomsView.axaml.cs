using Adapters.Project.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomsView : ReactiveUserControl<ClassroomsViewModel>
{
    public ClassroomsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}