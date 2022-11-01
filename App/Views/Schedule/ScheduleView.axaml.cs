using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.Schedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ScheduleView : ReactiveUserControl<ScheduleViewModel>
{
    public ScheduleView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}