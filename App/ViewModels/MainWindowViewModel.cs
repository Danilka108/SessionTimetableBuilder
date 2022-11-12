using System.Reactive;
using System.Reactive.Linq;
using ProjectPresentation;
using ReactiveUI;
using Storage;

namespace App.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public MainWindowViewModel()
    {
        ShowProjectWindow = new Interaction<ProjectInitializer, Unit>();

        OpenProject = ReactiveCommand.CreateFromTask(async () =>
        {
            var metadata = new StorageMetadata("", "TestProjectStorage");
            await ShowProjectWindow.Handle(new ProjectInitializer(metadata));
        });
    }

    public ReactiveCommand<Unit, Unit> OpenProject { get; }

    public Interaction<ProjectInitializer, Unit> ShowProjectWindow { get; }
}