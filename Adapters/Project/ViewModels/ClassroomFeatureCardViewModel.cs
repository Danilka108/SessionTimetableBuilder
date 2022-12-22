using System.Reactive;
using System.Reactive.Linq;
using Adapters.ViewModels;
using Domain.Project;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public class ClassroomFeatureCardViewModel : BaseViewModel
{
    public delegate ClassroomFeatureCardViewModel Factory(Identified<ClassroomFeature> feature);
    
    public ClassroomFeatureCardViewModel(
        Identified<ClassroomFeature> feature,
        ClassroomFeatureEditorViewModel.Factory editorFactory
    )
    {
        OpenEditor = new Interaction<ClassroomFeatureEditorViewModel, Unit>();

        Description = feature.Entity.Description;

        Edit = ReactiveCommand.CreateFromTask(async () =>
        {
            await OpenEditor.Handle(editorFactory.Invoke(feature.Id));
        });
    }
    
    public string Description { get; }

    public ReactiveCommand<Unit, Unit> Edit { get; }

    public Interaction<ClassroomFeatureEditorViewModel, Unit> OpenEditor { get; }
}