using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.Schedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ScheduleView : UserControl
{
    public ScheduleView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}