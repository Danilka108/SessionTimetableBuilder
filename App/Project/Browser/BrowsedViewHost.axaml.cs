using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.Browser;

public class BrowsedViewHost : TemplatedControl, IActivatableView
{
    private const string TabControlName = "BrowsedViewHost_tabControl";

    public static readonly StyledProperty<BrowsingState> BrowsingStateProperty =
        AvaloniaProperty.Register<BrowsedViewHost, BrowsingState>
            (nameof(BrowsingState));


    private IEnumerable<IBrowserPage> _pages;
    public IEnumerable<IBrowserPage> Pages
    {
        set => _pages;
        get => RaiseAn
    }

    public BrowsingState BrowsingState
    {
        get => GetValue(BrowsingStateProperty);
        set => SetValue(BrowsingStateProperty, value);
    }

    public BrowsedViewHost()
    {

        DataContext = this;
        
                BrowsingStateProperty
                    .Changed
                    .SelectMany(e => e.NewValue.Value.Changed)
                    .ToProperty(this, vm => vm.Pages);
    }

    private void AddPage(IBrowserPage page)
    {
        var selectedIndex = TabControl.SelectedIndex;
        var updatedItems = new List<object>();

        foreach (var item in TabControl.Items)
        {
            updatedItems.Add(item);
        }
        updatedItems.Add(page);

        TabControl.Items = updatedItems;
        TabControl.SelectedIndex = selectedIndex;
    }
}