using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.CreateTeacher;

public partial class CreateTeacherView : ReactiveUserControl<CreateTeacherViewModel>
{
    public CreateTeacherView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}