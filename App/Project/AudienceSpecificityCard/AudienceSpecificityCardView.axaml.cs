using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using App.Project.AudienceSpecificityEditor;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.AudienceSpecificityCard;

public partial class
    AudienceSpecificityCardView : ReactiveUserControl<AudienceSpecificityCardViewModel>
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

        if (ProjectWindow.ProjectWindow.GetCurrent() is {} window)
        {
            await editorWindow.ShowDialog(window);
        }

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}