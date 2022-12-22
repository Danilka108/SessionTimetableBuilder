using System.Reactive;
using System.Threading.Tasks;
using Adapters.Project.ViewModels;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

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