using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection.Metadata.Ecma335;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Styling;
using Avalonia.Svg;
using DynamicData;
using DynamicData.Kernel;
using ReactiveUI;

namespace App.Project.Browser;

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
    public static readonly StyledProperty<BrowserManager?> BrowserProperty =
        AvaloniaProperty.Register<BrowserViewHost, BrowserManager?>(nameof(Browser));

    public static readonly StyledProperty<IControl> DefaultPageProperty =
        AvaloniaProperty.Register<BrowserViewHost, IControl>(nameof(DefaultPage));

    public BrowserManager? Browser
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

    private readonly ReactiveCommand<IBrowserPage, Unit> _close;

    private Image CloseButtonIcon => new()
    {
        Width = 18,
        Height = 18,
        Source = new SvgImage
        {
            Source = SvgSource.Load("/Assets/close_icon.svg", new Uri("avares://App"))
        }
    };

    private FuncDataTemplate<BrowsingItem> TabItemTemplate => new(
        (item, _) => new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            Children =
            {
                new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 6, 2),
                    Classes = new Classes("TitleMedium"),
                    [!TextBlock.TextProperty] = new Binding("Page.PageName"),
                },
                new Button
                {
                    Background = new SolidColorBrush
                    {
                        Color = Color.FromArgb(0, 0, 0, 0)
                    },
                    CommandParameter = item.Page,
                    Command = _close,
                    Content = CloseButtonIcon
                }
            }
        }
    );

    private FuncDataTemplate<BrowsingItem> TabContentTemplate => new(
        (item, _) => item.Control
    );

    public BrowserViewHost()
    {
        _close = ReactiveCommand.Create<IBrowserPage>(ClosePage);

        _browsingItems = new AvaloniaList<BrowsingItem>();

        _tabControl = new TabControl
        {
            Items = _browsingItems,
            ItemTemplate = TabItemTemplate,
            ContentTemplate = TabContentTemplate
        };

        this.WhenActivated(d =>
        {
            this
                .GetObservable(BrowserProperty)
                .SelectMany(manager =>
                    manager?.BrowsingChanged ??
                    Observable
                        .Empty<BrowsingChange>()
                )
                .Subscribe(OnBrowserChanging)
                .DisposeWith(d);

            this
                .GetObservable(DefaultPageProperty)
                .Subscribe(_ => DisplayLastPage())
                .DisposeWith(d);
        });
    }

    private void OnBrowserChanging(BrowsingChange change)
    {
        switch (change)
        {
            case BrowsingChange.Browse browseChange:
                BrowsePage(browseChange.Page);
                break;
            case BrowsingChange.Close closeChange:
                ClosePage(closeChange.Page);
                break;
        }
    }

    private void DisplayLastPage()
    {
        if (_browsingItems.Count == 0)
        {
            Content = DefaultPage;
            return;
        }

        Content = _tabControl;
        _tabControl.SelectedIndex = _browsingItems.Count - 1;
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
        Content = _tabControl;

        var maybeSamePair = _browsingItems
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

        DisplayLastPage();
    }

    private void ClosePage(IBrowserPage page)
    {
        Content = _tabControl;

        var pageToRemoveIndex = _browsingItems.GetPages()
            .IndexOf(page, new IBrowserPage.Comparer());

        if (pageToRemoveIndex < 0) return;

        _browsingItems.RemoveAt(pageToRemoveIndex);

        DisplayLastPage();
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<IBrowserPage> GetPages(this IEnumerable<BrowsingItem> items)
    {
        return items.Select(item => item.Page);
    }

    public static T? FirstOrNull<T>(this IEnumerable<T> enumerable, T itemToFind,
        EqualityComparer<T> comparer) where T : class
    {
        return enumerable.FirstOrDefault(item => comparer.Equals(item, itemToFind));
    }
}