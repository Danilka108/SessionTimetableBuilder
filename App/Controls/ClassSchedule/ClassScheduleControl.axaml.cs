using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.ClassSchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ClassScheduleControl : ReactiveUserControl<ClassScheduleViewModel>
{
    public ClassScheduleControl()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}