using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.Groups;

// ReSharper disable once PartialTypeWithSinglePart
public partial class GroupsControl : ReactiveUserControl<GroupsViewModel>
{
    public GroupsControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}