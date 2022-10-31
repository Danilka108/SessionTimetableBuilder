using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.Teacher;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeacherControl : ReactiveUserControl<TeacherViewModel>
{
    public TeacherControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}