using Adapters.Project.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class LecturerEditorView : ReactiveUserControl<LecturerEditorViewModel>
{
    public LecturerEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}