using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Adapters.Project.Browser;

public class BrowserManager : IDisposable
{
    private readonly Subject<BrowsingChange> _changed;

    public IObservable<BrowsingChange> BrowsingChanged => _changed.AsObservable();

    public ReactiveCommand<IBrowserPage, Unit> Browse { get; }

    public ReactiveCommand<IBrowserPage, Unit> Close { get; }

    public BrowserManager()
    {
        _changed = new Subject<BrowsingChange>();

        Browse = ReactiveCommand.Create<IBrowserPage>(page =>
            _changed.OnNext(new BrowsingChange.Browse(page)));
        
        Close = ReactiveCommand.Create<IBrowserPage>(page =>
            _changed.OnNext(new BrowsingChange.Close(page)));
    }
    
    public void Dispose()
    {
        _changed.Dispose();
        Browse.Dispose();
        Close.Dispose();
    }
}