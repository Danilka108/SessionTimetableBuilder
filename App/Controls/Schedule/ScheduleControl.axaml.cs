using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.Schedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ScheduleControl : ReactiveUserControl<ScheduleViewModel>
{
    public ScheduleControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}