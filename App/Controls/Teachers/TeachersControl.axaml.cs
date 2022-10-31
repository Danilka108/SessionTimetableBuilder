using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Controls.Teachers;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeachersControl : ReactiveUserControl<TeachersViewModel>
{
    public TeachersControl()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}