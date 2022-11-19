using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using App.CommonControls.ConfirmWindow;
using App.CommonControls.MessageWindow;
using App.Project.AudienceSpecificityEditor;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.AudienceSpecificities;

public partial class AudienceSpecificitiesView : ReactiveUserControl<AudienceSpecificitiesViewModel>
{
    public AudienceSpecificitiesView()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .OpenEditor
                    .RegisterHandler(DoOpenEditorDialog)
                    .DisposeWith(d);
            }
        );

        InitializeComponent();
    }

    private async Task DoOpenEditorDialog
        (InteractionContext<AudienceSpecificityEditorViewModel, Unit> context)
    {
        var editorWindow = new AudienceSpecificityEditorWindow
        {
            DataContext = context.Input
        };

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
            await editorWindow.ShowDialog(window);

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}