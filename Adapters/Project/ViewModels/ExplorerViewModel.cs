using Adapters.ViewModels;
using ReactiveUI;

namespace Adapters.Project.ViewModels;

public enum ExploredSet
{
    ClassroomFeature  
}

public class ExplorerViewModel : BaseViewModel
{
    public delegate ExplorerViewModel Factory();

    private ExploredSet _exploredSet;

    public ExploredSet ExploredSet
    {
        get => _exploredSet;
        set => this.RaiseAndSetIfChanged(ref _exploredSet, value);
    }

    public ExplorerViewModel()
    {
        _exploredSet = ExploredSet.ClassroomFeature;
    }
}