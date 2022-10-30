using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Ui.DailySchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class DailyScheduleView : UserControl
{
    public DailyScheduleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}