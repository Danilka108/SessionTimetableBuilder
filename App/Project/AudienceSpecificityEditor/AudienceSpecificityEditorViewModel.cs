using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.CommonControls.MessageWindow;
using Domain;
using Domain.Project.Models;
using Domain.Project.UseCases;
using ReactiveUI;

namespace App.Project.AudienceSpecificityEditor;

public class AudienceSpecificityEditorViewModel : ViewModelBase, IActivatableViewModel
{
    public delegate AudienceSpecificityEditorViewModel Factory
        (IdentifiedModel<AudienceSpecificity>? specificity);

    private int? _id;

    private string _description;

    private ObservableAsPropertyHelper<bool> _canBeSaved { get; }

    private ObservableAsPropertyHelper<bool> _isLoading { get; }

    private SaveAudienceSpecificityUseCase _saveAudienceSpecificityUseCase { get; }

    public AudienceSpecificityEditorViewModel
    (
        IdentifiedModel<AudienceSpecificity>? specificity,
        SaveAudienceSpecificityUseCase saveAudienceSpecificityUseCase
    )
    {
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();
        Activator = new ViewModelActivator();

        Close = ReactiveCommand.Create
        (
            () =>
            {
            }
        );

        _saveAudienceSpecificityUseCase = saveAudienceSpecificityUseCase;
        _id = specificity?.Id;
        _description = specificity?.Model.Description ?? "";

        var canBeSaved = this
            .WhenAnyValue(vm => vm.Description)
            .Select(description => description.Length > 0);

        Save = ReactiveCommand
            .CreateFromTask
            (
                async () => await _saveAudienceSpecificityUseCase.Handle
                    (new AudienceSpecificity(_description), _id),
                canBeSaved
            );

        _canBeSaved = canBeSaved
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, vm => vm.CanBeSaved);

        _isLoading = Save
            .IsExecuting
            .ObserveOn(RxApp.MainThreadScheduler)
            .ToProperty(this, vm => vm.IsLoading);

        this.WhenActivated
        (
            d =>
            {
                Save
                    .SelectMany(_ => Close.Execute())
                    .Subscribe()
                    .DisposeWith(d);

                Save
                    .ThrownExceptions
                    .SelectMany
                    (
                        e => OpenMessageDialog.Handle
                            (new MessageWindowViewModel("Error", e.Message))
                    )
                    .Subscribe()
                    .DisposeWith(d);
            }
        );
    }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public bool IsLoading => _isLoading.Value;

    public bool CanBeSaved => _canBeSaved.Value;

    public ReactiveCommand<Unit, Unit> Close { get; }

    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }

    public ViewModelActivator Activator { get; }
}