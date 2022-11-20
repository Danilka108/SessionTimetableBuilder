using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.CommonControls.ConfirmWindow;
using App.CommonControls.MessageWindow;
using ReactiveUI;

namespace App.Project.ExplorerCard;

public abstract class ExplorerCardViewModel : ViewModelBase
{
    public ExplorerCardViewModel()
    {
        OpenEditorDialog = new Interaction<ViewModelBase, Unit>();
        OpenConfirmDialog = new Interaction<ConfirmWindowViewModel, bool>();
        OpenMessageDialog = new Interaction<MessageWindowViewModel, Unit>();

        ShowEditor = ReactiveCommand.CreateFromTask(DoShowEditor);
        Delete = ReactiveCommand.CreateFromTask(DoDelete);
    }
    
    private async Task DoDelete()
    {
        var confirmViewModel = new ConfirmWindowViewModel
            ("Confirm action", ConfirmDeleteMessage, "Delete");

        var hasBeenConfirmed = await OpenConfirmDialog.Handle(confirmViewModel);

        if (!hasBeenConfirmed) return;

        try
        {
            await TryDoDelete();
        }
        catch (Exception e)
        {
            var messageViewModel = new MessageWindowViewModel("Error", e.Message);
            await OpenMessageDialog.Handle(messageViewModel);
        }
    }

    private async Task DoShowEditor()
    {
        await OpenEditorDialog.Handle(ProvideEditorViewModel());
    }

    protected abstract ViewModelBase ProvideEditorViewModel();
    protected abstract Task TryDoDelete();

    protected abstract string ConfirmDeleteMessage { get; }
    public abstract string Title { get; }

    public ReactiveCommand<Unit, Unit> ShowEditor { get; }
    public ReactiveCommand<Unit, Unit> Delete { get; }

    public Interaction<ViewModelBase, Unit> OpenEditorDialog { get; }
    public Interaction<ConfirmWindowViewModel, bool> OpenConfirmDialog { get; }
    public Interaction<MessageWindowViewModel, Unit> OpenMessageDialog { get; }
}