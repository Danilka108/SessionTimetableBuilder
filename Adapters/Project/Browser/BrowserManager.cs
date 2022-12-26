using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Adapters.Project.Browser;

class GeneratedBrowserPage : IBrowserPage
{
    public GeneratedBrowserPage(string pageName)
    {
        PageName = pageName;
    }

    public string PageName { get; }
    
    public Task<bool> ConfirmPageClosingAsync()
    {
        return Task.FromResult(true);
    }
}

public class BrowserManager : IDisposable
{
    private readonly List<IBrowserPage> _pages;

    private readonly Subject<BrowsingChange> _changed;

    public BrowserManager()
    {
        _pages = new List<IBrowserPage>();
        _changed = new Subject<BrowsingChange>();

        Browse = ReactiveCommand.Create<IBrowserPage>(BrowsePage);

        Close = ReactiveCommand.CreateFromTask<IBrowserPage>(ClosePage);

        CloseByPageName = ReactiveCommand.CreateFromTask<string>(async pageName =>
        {
            await ClosePage(new GeneratedBrowserPage(pageName));
        });
    }

    private void BrowsePage(IBrowserPage page)
    {
        var samePage = _pages.FirstOrDefault(p => p.PageName.Trim() == page.PageName.Trim());

        if (samePage is null)
        {
            _pages.Add(page);
            _changed.OnNext(new BrowsingChange.Add(page));
        }

        _changed.OnNext(new BrowsingChange.Browse(page));
    }

    private async Task ClosePage(IBrowserPage page)
    {
        var pageToRemoveIndex = _pages.FindIndex(p => p.PageName == page.PageName);

        if (pageToRemoveIndex < 0) return;

        var confirmed = await page.ConfirmPageClosingAsync();

        if (!confirmed) return;

        _pages.RemoveAt(pageToRemoveIndex);
        _changed.OnNext(new BrowsingChange.Remove(page));

        if (_pages.Count == 0)
        {
            _changed.OnNext(new BrowsingChange.BrowseDefault());
        }
        else
        {
            _changed.OnNext(new BrowsingChange.Browse(_pages.Last()));
        }
    }

    public IObservable<BrowsingChange> BrowsingChanged => _changed.AsObservable();

    public ReactiveCommand<IBrowserPage, Unit> Browse { get; }

    public ReactiveCommand<IBrowserPage, Unit> Close { get; }

    public ReactiveCommand<string, Unit> CloseByPageName { get; }

    public void Dispose()
    {
        _changed.Dispose();
        Browse.Dispose();
        Close.Dispose();
    }
}