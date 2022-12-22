using System;
using Adapters.Project.ViewModels;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeatureEditorWindow : ReactiveWindow<ClassroomFeatureEditorViewModel>
{
    public ClassroomFeatureEditorWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.WhenActivated(d =>
        {
            // ViewModel!
            //     .Close
            //     .Subscribe(_ => Close())
            //     .DisposeWith(d);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}