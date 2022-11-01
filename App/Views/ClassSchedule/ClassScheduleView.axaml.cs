using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.ClassSchedule;

// ReSharper disable once PartialTypeWithSinglePart
public partial class ClassScheduleView : ReactiveUserControl<ClassScheduleViewModel>
{
    public ClassScheduleView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}