using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace App.Views.Teachers;

// ReSharper disable once PartialTypeWithSinglePart
public partial class TeachersView : ReactiveUserControl<TeachersViewModel>
{
    public TeachersView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}