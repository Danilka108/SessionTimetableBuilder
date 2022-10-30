using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.Groups;

// ReSharper disable once PartialTypeWithSinglePart
public partial class GroupsView : UserControl
{
    public GroupsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}