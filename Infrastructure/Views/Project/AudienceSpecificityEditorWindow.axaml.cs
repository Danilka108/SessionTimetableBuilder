using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.AudienceSpecificityEditor;

public partial class
    AudienceSpecificityEditorWindow : ReactiveWindow<AudienceSpecificityEditorViewModel>
{
    public AudienceSpecificityEditorWindow()
    {
        this.WhenActivated
        (
            d =>
            {
                ViewModel!
                    .OpenMessageDialog
                    .RegisterHandler(DoOpenMessageDialog)
                    .DisposeWith(d);

                ViewModel!
                    .Close
                    .Subscribe(_ => Close())
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