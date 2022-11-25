using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.Teachers;

public partial class TeachersView : ReactiveUserControl<TeachersViewModel>
{
    public TeachersView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}