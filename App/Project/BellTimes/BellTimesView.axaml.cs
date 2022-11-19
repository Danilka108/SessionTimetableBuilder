using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Project.BellTimes;

public partial class BellTimesView : UserControl
{
    public BellTimesView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}