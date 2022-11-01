using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.Groups;

// ReSharper disable once PartialTypeWithSinglePart
public partial class GroupsView : ReactiveUserControl<GroupsViewModel>
{
    public GroupsView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}