using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Adapters.Project.Browser;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.Styling;
using DynamicData.Binding;
using Material.Icons;
using Material.Icons.Avalonia;
using ReactiveUI;

namespace Infrastructure.Project.Views;

public record BrowsingItem(IBrowserPage Page, IControl Control);

public class BrowserViewHost : ContentControl, IStyleable
{
    public static readonly StyledProperty<IBrowser?> BrowserProperty =
        AvaloniaProperty.Register<BrowserViewHost, IBrowser?>(nameof(Browser));

    public static readonly StyledProperty<IControl> DefaultPageProperty =
        AvaloniaProperty.Register<BrowserViewHost, IControl>(nameof(DefaultPage));

    public IBrowser? Browser
    {
        get => GetValue(BrowserProperty);
        set => SetValue(BrowserProperty, value);
    }

    public IControl DefaultPage
    {
        get => GetValue(DefaultPageProperty);
        set => SetValue(DefaultPageProperty, value);
    }

    Type IStyleable.StyleKey => typeof(ContentControl);

    public IViewLocator ViewLocator { get; set; }

    private ObservableCollection<BrowsingItem> Items { get; }

    private readonly TabStrip _strip;

    private readonly ContentControl _presentedContent = new();

    private readonly ReactiveCommand<IBrowserPage, Unit> _close;

    public BrowserViewHost()
    {
        Items = new ObservableCollection<BrowsingItem>();

        _close = ReactiveCommand.CreateFromObservable<IBrowserPage, Unit>(page =>
            Browser!.Manager.Close.Execute(page));

        _strip = new TabStrip
        {
            Items = Items,
            ItemTemplate = new FuncDataTemplate<BrowsingItem>((item, _) => new StackPanel
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
                        Classes = new Classes("Flat"),
                        Padding = new Thickness(10, 8),
                        Background = new SolidColorBrush
                        {
                            Color = Color.FromArgb(0, 0, 0, 0)
                        },
                        CommandParameter = item.Page,
                        Command = _close,
                        Content = new MaterialIcon
                        {
                            Width = 20,
                            Height = 20,
                            Kind = MaterialIconKind.Remove
                        }
                    }
                }
            })
        };

        Content = new StackPanel
        {
            Children =
            {
                _strip,
                _presentedContent,
            }
        };

        _strip.WhenAnyValue(strip => strip.SelectedIndex)
            .Subscribe(index => ShowPage(index));

        this.GetObservable(DefaultPageProperty)
            .Subscribe(_ => ShowPage());

        this.GetObservable(BrowserProperty)
            .SelectMany(browser =>
                browser?.Manager.BrowsingChanged ?? Observable.Empty<BrowsingChange>())
            .Subscribe(HandleChange);
    }

    private void ShowPage(int? index = null)
    {
        if (Items.Count == 0 || _strip.SelectedIndex < 0)
        {
            _presentedContent.Content = DefaultPage;
            return;
        }

        var itemToPresent = Items[index ?? _strip.SelectedIndex];
        _presentedContent.Content = itemToPresent.Control;
    }

    private void HandleChange(BrowsingChange change)
    {
        if (change is BrowsingChange.Add addChange)
        {
            var view = ResolveView(addChange.Page);
            var item = new BrowsingItem(addChange.Page, view);
            Items.Add(item);
        }

        if (change is BrowsingChange.Browse browseChange)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Page.PageName != browseChange.Page.PageName) continue;
                _strip.SelectedIndex = i;
            }
        }

        if (change is BrowsingChange.Remove removeChange)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i].Page.PageName != removeChange.Page.PageName) continue;
                Items.RemoveAt(i);
            }
        }

        if (change is BrowsingChange.BrowseDefault)
        {
            _strip.SelectedIndex = -1;
        }

        ShowPage();
    }

    private IControl ResolveView(IBrowserPage page)
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
}