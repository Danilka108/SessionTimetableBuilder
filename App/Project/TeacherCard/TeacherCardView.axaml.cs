using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.TeacherCard;

public partial class TeacherCardView : ReactiveUserControl<TeacherCardViewModel>
{
    public TeacherCardView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}