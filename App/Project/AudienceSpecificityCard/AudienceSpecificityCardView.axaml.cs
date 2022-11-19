using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using App.CommonControls.ConfirmWindow;
using App.CommonControls.MessageWindow;
using App.Project.AudienceSpecificityEditor;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.AudienceSpecificityCard;

public partial class AudienceSpecificityCardView : ReactiveUserControl<AudienceSpecificityCardViewModel>
{
    public AudienceSpecificityCardView()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .OpenEditor
                    .RegisterHandler(DoOpenEditorDialog)
                    .DisposeWith(d);

                ViewModel!
                    .OpenConfirmDialog
                    .RegisterHandler(DoOpenConfirmDialog)
                    .DisposeWith(d);

                ViewModel!
                    .OpenMessageDialog
                    .RegisterHandler(DoOpenErrorMessageDialog)
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

    private async Task DoOpenConfirmDialog(InteractionContext<ConfirmWindowViewModel, bool> context)
    {
        var confirmWindow = new ConfirmWindow
        {
            DataContext = context.Input
        };

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
        {
            var result = await confirmWindow.ShowDialog<bool?>(window);
            context.SetOutput(result ?? false);
        }
        else
        {
            context.SetOutput(false);
        }
    }

    private async Task DoOpenErrorMessageDialog
        (InteractionContext<MessageWindowViewModel, Unit> context)
    {
        var messageWindow = new MessageWindow
        {
            DataContext = context.Input
        };

        if (ProjectWindow.ProjectWindow.GetCurrent() is { } window)
            await messageWindow.ShowDialog(window);

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}