using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.Teacher;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeacherView : ReactiveUserControl<TeacherViewModel>
{
    public TeacherView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}