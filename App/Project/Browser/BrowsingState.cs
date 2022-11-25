using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace App.Project.Browser;

public class BrowsingState
{
    private readonly BehaviorSubject<IEnumerable<IBrowserPage>> _pages;

    public IObservable<IEnumerable<IBrowserPage>> Changed => _pages.AsObservable();

    public BrowsingState()
    {
        _pages = new BehaviorSubject<IEnumerable<IBrowserPage>>(new List<IBrowserPage>());
    }

    public void Open
        (IBrowserPage page) =>
        _pages.OnNext(new List<IBrowserPage>(_pages.Value) { page });

    public void Close(string pageId)
    {
        var updatedPages = new List<IBrowserPage>(_pages.Value);
        
        var pageIndex = updatedPages.FindIndex(p => p.PageId == pageId);
        if (pageIndex > 0) updatedPages.RemoveAt(pageIndex);
        
        _pages.OnNext(updatedPages);
    }
}