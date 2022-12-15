using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using ReactiveUI;

namespace App.Project.Browser;

public abstract class BrowserStateChange
{
    private BrowserStateChange()
    {
    }

    // public sealed class Browse : BrowserStateChange
    // {
    //     public IBrowserPage Page { get; }
    //
    //     public Browse(IBrowserPage page)
    //     {
    //         Page = page;
    //     }
    // }
    //
    // public sealed class BrowseDefault : BrowserStateChange
    // {
    //     public BrowseDefault()
    //     {
    //     }
    // }
    //
    // public sealed class Add : BrowserStateChange
    // {
    //     public IBrowserPage Page { get; }
    //
    //     public Add(IBrowserPage page)
    //     {
    //         Page = page;
    //     }
    // }
    //
    // public sealed class Remove : BrowserStateChange
    // {
    //     public IBrowserPage Page { get; }
    //
    //     public Remove(IBrowserPage page)
    //     {
    //         Page = page;
    //     }
    // }

    public sealed class Browse : BrowserStateChange
    {
        public IBrowserPage Page { get; }

        public Browse(IBrowserPage page)
        {
            Page = page;
        }
    }

    public sealed class Close : BrowserStateChange
    {
        public IBrowserPage Page { get; }

        public Close(IBrowserPage page)
        {
            Page = page;
        }
    }
}

public class BrowserState : IDisposable
{
    private readonly List<IBrowserPage> _visits;

    private readonly Subject<BrowserStateChange> _changed;

    public IObservable<BrowserStateChange> Changed => _changed.AsObservable();

    public ReactiveCommand<IBrowserPage, Unit> Browse { get; }

    public ReactiveCommand<IBrowserPage, Unit> Close { get; }

    public BrowserState()
    {
        _changed = new Subject<BrowserStateChange>();
        _visits = new List<IBrowserPage>();

        Browse = ReactiveCommand.Create<IBrowserPage>(page =>
            _changed.OnNext(new BrowserStateChange.Browse(page)));
        
        Close = ReactiveCommand.Create<IBrowserPage>(page =>
            _changed.OnNext(new BrowserStateChange.Close(page)));
    }

    // private void BrowsePage(IBrowserPage page)
    // {
    //     var pageDoesNotPresented = _visits.IndexOf(page, new IBrowserPage.Comparer()) < 0;
    //
    //     if (pageDoesNotPresented)
    //     {
    //         _visits.Add(page);
    //         _changed.OnNext(new BrowserStateChange.Add(page));
    //     }
    //     else
    //     {
    //         _visits.Remove(page);
    //         _visits.Add(page);
    //     }
    //
    //     _changed.OnNext(new BrowserStateChange.Browse(page));
    // }
    //
    // private void ClosePage(IBrowserPage page)
    // {
    //     var pageIndex = _visits.IndexOf(page, new IBrowserPage.Comparer());
    //     if (pageIndex < 0) throw new NotImplementedException();
    //
    //     _visits.RemoveAt(pageIndex);
    //     _changed.OnNext(new BrowserStateChange.Remove(page));
    //
    //     try
    //     {
    //         var prevVisitedPage = _visits.Last();
    //         _changed.OnNext(new BrowserStateChange.Browse(prevVisitedPage));
    //     }
    //     catch (Exception)
    //     {
    //         _changed.OnNext(new BrowserStateChange.BrowseDefault());
    //     }
    // }

    public void Dispose()
    {
        _changed.Dispose();
        Browse.Dispose();
        Close.Dispose();
    }
}