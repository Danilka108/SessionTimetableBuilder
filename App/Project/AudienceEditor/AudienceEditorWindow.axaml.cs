using System;
using System.Reactive;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Avalonia;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.AudienceEditor;

public partial class AudienceEditorWindow : ReactiveWindow<AudienceEditorViewModel>
{
    public AudienceEditorWindow()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .Close
                    .Subscribe(_ => Close())
                    .DisposeWith(d);

                ViewModel!
                    .OpenMessageDialog
                    .RegisterHandler(DoOpenMessageDialog)
                    .DisposeWith(d);
            }
        );

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private async Task DoOpenMessageDialog(InteractionContext<MessageWindowViewModel, Unit> context)
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