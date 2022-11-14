using System;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Project.Explorer;

public partial class ExplorerView : ReactiveUserControl<ExplorerViewModel>
{
    public ExplorerView()
    {
        this.WhenActivated(d => { ViewModel!.NavigateToExploredItem.Subscribe().DisposeWith(d); });

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}