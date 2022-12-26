using System.Reactive;
using System.Threading.Tasks;
using Adapters.Common.ViewModels;
using Adapters.Project.ViewModels;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Infrastructure.Common.Views;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public partial class ClassroomFeaturesView : ReactiveUserControl<ClassroomFeaturesViewModel>
{
    public ClassroomFeaturesView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel!
                .OpenMessageDialog
                .RegisterHandler(DoOpenMessageDialog)
                .DisposeWith(d);
        });
    }

    private async Task DoOpenMessageDialog(InteractionContext<MessageDialogViewModel, Unit> context)
    {
        var viewModel = context.Input;

        var dialog = new MessageDialogWindow
        {
            DataContext = viewModel
        };

        await dialog.ShowDialog(ProjectWindow.GetCurrent());

        context.SetOutput(Unit.Default);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}