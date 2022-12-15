using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Layout;
using Avalonia.Styling;
using DynamicData;
using DynamicData.Kernel;
using ReactiveUI;

namespace App.Project.Browser;

class DefaultBrowserPage : IBrowserPage
{
    public string Name => "DefaultPage";
}

public struct BrowsingItem
{
    public IBrowserPage Page { get; }

    public IControl Control { get; }

    public BrowsingItem(IBrowserPage page, IControl control)
    {
        Page = page;
        Control = control;
    }
}

public class BrowserViewHost : ContentControl, IActivatableView, IStyleable
{
    public static readonly StyledProperty<BrowserState?> BrowserProperty =
        AvaloniaProperty.Register<BrowserViewHost, BrowserState?>(nameof(Browser));

    public readonly static StyledProperty<IControl> DefaultPageProperty =
        AvaloniaProperty.Register<BrowserViewHost, IControl>(nameof(DefaultPage));

    public BrowserState? Browser
    {
        get => GetValue(BrowserProperty);
        set => SetValue(BrowserProperty, value);
    }

    public IControl DefaultPage
    {
        get => GetValue(DefaultPageProperty);
        set => SetValue(DefaultPageProperty, value);
    }

    public IViewLocator? ViewLocator { get; set; }

    private readonly AvaloniaList<BrowsingItem> _browsingItems;

    private readonly TabControl _tabControl;

    Type IStyleable.StyleKey => typeof(ContentControl);

    private int _defaultItemIndex;

    private static readonly FuncDataTemplate<BrowsingItem> ItemTemplate = new(
        (item, _) => new TextBlock { Text = item.Page.Name }
    );

    private static readonly FuncDataTemplate<BrowsingItem> ContentTemplate = new(
        (item, _) => item.Control
    );

    public BrowserViewHost()
    {
        _browsingItems = new AvaloniaList<BrowsingItem>();

        _tabControl = new TabControl
        {
            Items = _browsingItems,
            SelectedIndex = 0,
            ItemTemplate = ItemTemplate,
            ContentTemplate = ContentTemplate
        };

        Content = _tabControl;

        this.WhenActivated(d =>
        {
            this
                .GetObservable(BrowserProperty)
                .SelectMany(state =>
                    state?.Changed ??
                    Observable
                        .Empty<BrowserStateChange>()
                )
                .Subscribe(OnBrowserChanging)
                .DisposeWith(d);

            this
                .GetObservable(DefaultPageProperty)
                .Subscribe(OnDefaultControlUpdating)
                .DisposeWith(d);
        });
    }

    private void OnBrowserChanging(BrowserStateChange change)
    {
        switch (change)
        {
            case BrowserStateChange.Browse browseChange:
                BrowsePage(browseChange.Page);
                break;
            case BrowserStateChange.Close closeChange:
                ClosePage(closeChange.Page);
                break;
        }
    }

    private void OnDefaultControlUpdating(IControl defaultControl)
    {
        var defaultItem = new BrowsingItem(new DefaultBrowserPage(), defaultControl);

        var defaultPageIndex = _browsingItems
            .GetPages()
            .IndexOf(defaultItem.Page, new IBrowserPage.Comparer());

        if (defaultPageIndex > 0)
        {
            _defaultItemIndex = defaultPageIndex;
            _browsingItems[_defaultItemIndex] = defaultItem;
        }
        else
        {
            _browsingItems.Add(defaultItem);
            _defaultItemIndex = _browsingItems.Count - 1;
        }
    }

    private IControl ResolvePage(IBrowserPage page)
    {
        if (ViewLocator is null)
        {
            return new TextBlock
            {
                Text = "Error. ViewLocator is required!"
            };
        }

        var resolvedView = ViewLocator.ResolveView(page);

        if (resolvedView is not IControl resolvedControl)
        {
            return new TextBlock
            {
                Text = "Error. Failed to resolve view!"
            };
        }

        return resolvedControl;
    }

    private void BrowsePage(IBrowserPage page)
    {
        var maybeSamePair = _browsingItems
            .SkipAt(_defaultItemIndex)
            .GetPages()
            .FirstOrNull(page, new IBrowserPage.Comparer());

        if (maybeSamePair is null)
        {
            _browsingItems.Add(new BrowsingItem(page, ResolvePage(page)));
        }
        else
        {
            var itemToBrowseIndex =
                _browsingItems
                    .GetPages()
                    .IndexOf(page, new IBrowserPage.Comparer());

            var itemToBrowse = _browsingItems[itemToBrowseIndex];

            _browsingItems.RemoveAt(itemToBrowseIndex);
            _browsingItems.Add(itemToBrowse);
        }

        _tabControl.SelectedIndex = _browsingItems.Count - 1;
    }

    private void ClosePage(IBrowserPage page)
    {
        var maybeSamePair = _browsingItems
            .SkipAt(_defaultItemIndex)
            .GetPages()
            .FirstOrNull(page, new IBrowserPage.Comparer());

        if (maybeSamePair is null) return;
        
        
        var pageToRemoveIndex = _browsingItems.GetPages()
            .IndexOf(page, new IBrowserPage.Comparer());
        _browsingItems.RemoveAt(pageToRemoveIndex);
        
        _tabControl.SelectedIndex = _browsingItems.Count - 1;
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<T> SkipAt<T>(this IEnumerable<T> enumerable, int index)
    {
        var oldCollection = new List<T>(enumerable);
        var newCollection = new List<T>();

        newCollection.AddRange(oldCollection.Take(index));
        newCollection.AddRange(oldCollection.Skip(index + 1));

        return newCollection;
    }

    public static IEnumerable<IBrowserPage> GetPages(this IEnumerable<BrowsingItem> items)
    {
        return items.Select(item => item.Page);
    }

    public static IEnumerable<IControl> GetControls(this IEnumerable<BrowsingItem> items)
    {
        return items.Select(item => item.Control);
    }

    public static T? FirstOrNull<T>(this IEnumerable<T> enumerable, T itemToFind,
        EqualityComparer<T> comparer) where T : class
    {
        return enumerable.FirstOrDefault(item => comparer.Equals(item, itemToFind));
    }
}