using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls.Mixins;
using ProjectData;
using ReactiveUI;
using Storage;

namespace ProjectPresentation;

public class ProjectInitializer
{
    private readonly StorageMetadata _storageMetadata;

    public ProjectInitializer(StorageMetadata storageMetadata)
    {
        _storageMetadata = storageMetadata;
    }

    public async Task<ProjectWindow> Initialize()
    {
        var diContainer = InitializeDiContainer();

        var projectStorageInitializer = diContainer.Resolve<ProjectStorageInitializer>();
        try
        {
            await projectStorageInitializer.Initialize(CancellationToken.None);
        }
        catch (Exception e)
        {
            await diContainer.DisposeAsync();
            throw new InitializeProjectException("Failed to initialize storage", e);
        }

        var windowViewModel = diContainer.Resolve<ProjectWindowViewModel>();
        var window = new ProjectWindow
        {
            DataContext = windowViewModel
        };
        window.WhenActivated(d => diContainer.DisposeWith(d));

        return window;
    }

    private ILifetimeScope InitializeDiContainer()
    {
        var autofacBuilder = new ContainerBuilder();

        autofacBuilder.RegisterModule(new ProjectDataModule
        {
            Metadata = _storageMetadata
        });

        autofacBuilder.RegisterType<ProjectWindowViewModel>();

        var container = autofacBuilder.Build();
        return container.BeginLifetimeScope();
    }
}

public class InitializeProjectException : Exception
{
    internal InitializeProjectException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}