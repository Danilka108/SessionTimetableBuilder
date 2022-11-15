using System;
using System.Reactive;
using App.Project.AudienceSpecificityEditor;
using Domain;
using Domain.Project.Models;
using ReactiveUI;

namespace App.Project.AudienceSpecificityCard;

public class AudienceSpecificityCardViewModel : ViewModelBase
{
    public delegate AudienceSpecificityCardViewModel Factory
        (IdentifiedModel<AudienceSpecificity> specificity);

    public AudienceSpecificityCardViewModel
    (
        IdentifiedModel<AudienceSpecificity> specificity,
        AudienceSpecificityEditorViewModel.Factory editorViewModelFactory
    )
    {
        OpenEditor = new Interaction<AudienceSpecificityEditorViewModel, Unit>();

        Show = ReactiveCommand.CreateFromObservable
        (
            () => OpenEditor.Handle(editorViewModelFactory.Invoke(specificity))
        );
        Description = specificity.Model.Description;
    }

    public Interaction<AudienceSpecificityEditorViewModel, Unit> OpenEditor { get; }

    public ReactiveCommand<Unit, Unit> Show { get; }

    public string Description { get; }
}