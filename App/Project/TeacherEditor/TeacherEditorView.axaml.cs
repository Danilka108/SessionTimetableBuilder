using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Project.TeacherEditor;

public partial class TeacherEditorView : ReactiveUserControl<TeacherEditorViewModel>
{
    public TeacherEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}