using Adapters.Preview;
using Adapters.Preview.ViewModels;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Infrastructure.Preview.Views;
using ReactiveUI;

namespace Infrastructure.Preview;

public class PreviewInitializer
{
    private readonly ILifetimeScope _parentDiScope; 
    
    public PreviewInitializer(ILifetimeScope parentDiScope)
    {
        _parentDiScope = parentDiScope;
    }

    public Window Initialize()
    {
        var previewDiScope = InitDiScope();

        var previewViewModel = previewDiScope.Resolve<PreviewViewModel>();

        var previewWindow = new PreviewWindow
        {
            ViewModel = previewViewModel,
            ParentDiScope = _parentDiScope
        };

        previewWindow.WhenActivated(d => previewDiScope.DisposeWith(d));

        return previewWindow;
    }
    
    private ILifetimeScope InitDiScope()
    {
        var currentDiScope = _parentDiScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterModule(new AdaptersPreviewModule());
        });

        return currentDiScope;
    }
}