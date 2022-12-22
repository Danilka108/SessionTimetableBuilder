using System;
using System.Reactive;
using System.Threading.Tasks;
using Adapters.Project.ViewModels;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeatureCardView : ReactiveUserControl<ClassroomFeatureCardViewModel>
{
    public ClassroomFeatureCardView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel!
                .OpenEditor
                .RegisterHandler(DoOpenEditor)
                .DisposeWith(d);
        });
    }

    private async Task DoOpenEditor(
        InteractionContext<ClassroomFeatureEditorViewModel, Unit> context)
    {
        var editor = new ClassroomFeatureEditorWindow
        {
            DataContext = context.Input
        };

        var projectWindow = ProjectWindow.GetCurrent();
        await editor.ShowDialog(projectWindow);

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}