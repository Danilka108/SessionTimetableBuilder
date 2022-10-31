using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.DailySchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class DailyScheduleControl : ReactiveUserControl<DailyScheduleViewModel>
{
    public DailyScheduleControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}