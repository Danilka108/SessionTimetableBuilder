using System.Reactive;
using System.Reactive.Linq;
using App.Project;
using ReactiveUI;
using Storage;

namespace App.Preview.PreviewWindow;

public class PreviewWindowViewModel : ViewModelBase
{
    public PreviewWindowViewModel()
    {
        ShowProjectWindow = new Interaction<ProjectInitializer, Unit>();

        OpenProject = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                var metadata = new StorageMetadata("", "TestProjectStorage");
                await ShowProjectWindow.Handle(new ProjectInitializer(metadata));
            }
        );
    }

    public ReactiveCommand<Unit, Unit> OpenProject { get; }

    public Interaction<ProjectInitializer, Unit> ShowProjectWindow { get; }
}