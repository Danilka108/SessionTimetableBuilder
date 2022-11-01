using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.DailySchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class DailyScheduleView : ReactiveUserControl<DailyScheduleViewModel>
{
    public DailyScheduleView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}