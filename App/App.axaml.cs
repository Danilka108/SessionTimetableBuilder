using App.ViewModels;
using App.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace App;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // var builder = new ContainerBuilder();
        //
        // builder.RegisterModule<DataModule>();
        // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
        //
        // // var resolver = new AutofacDependencyResolver(builder);
        // // Locator.SetLocator();
        // var resolver = builder.UseAutofacDependencyResolver();
        //
        // // Locator.SetLocator(resolver);
        //
        // // Locator.CurrentMutable.InitializeSplat();
        // // Locator.CurrentMutable.InitializeReactiveUI();
        // //
        // // Locator.CurrentMutable.RegisterConstant<IActivationForViewFetcher>(new AvaloniaActivationForViewFetcher());
        // // Locator.CurrentMutable.RegisterConstant<IPropertyBindingHook>(new AutoDataTemplateBindingHook());
        // // RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        //
        // var container = builder.Build();
        // resolver.SetLifetimeScope(container);
        //
        // if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //     desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };

        base.OnFrameworkInitializationCompleted();
    }
}