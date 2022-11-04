using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace App.Views.AddDisciplineWindow;

// ReSharper disable once PartialTypeWithSinglePart
public partial class AddDisciplineWindowView : ReactiveWindow<AddDisciplineWindowViewModel>
{
    public AddDisciplineWindowView()
    {
        AvaloniaXamlLoader.Load(this);

#if DEBUG
        this.AttachDevTools();
#endif
    }
}