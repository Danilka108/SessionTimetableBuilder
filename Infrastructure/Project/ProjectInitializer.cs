using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adapters.Project;
using Adapters.Project.ViewModels;
using Application.Project;
using Application.Project.Gateways;
using Autofac;
using Avalonia.Controls.Mixins;
using Domain.Project;
using Infrastructure.Project.Views;
using Infrastructure.Storage;
using ReactiveUI;
using Storage;

namespace Infrastructure.Project;

public class ProjectInitializer
{
    private readonly string _storageFullPath;

    public ProjectInitializer(string storageFullPath)
    {
        _storageFullPath = storageFullPath;
    }

    public async Task<ProjectWindow> Initialize()
    {
        // var resolver = new AutofacDependencyResolver(autofacBuilder);
        // Locator.SetLocator(resolver);
        // Locator.CurrentMutable.InitializeSplat();
        // Locator.CurrentMutable.InitializeReactiveUI();
        //
        // var container = autofacBuilder.Build();
        // var rootDiScope = container.BeginLifetimeScope();
        //
        // resolver.SetLifetimeScope(rootDiScope);

        var autofacBuilder = new ContainerBuilder();

        autofacBuilder.RegisterModule(new AdaptersProjectModule
        {
            StorageResourceFactory = () => new FileStorageResource(_storageFullPath)
        });
        autofacBuilder.RegisterModule(new ApplicationProjectModule());

        var container = autofacBuilder.Build();

        await CreateTestData(container);

        var projectViewModelFactory = container.Resolve<ProjectViewModel.Factory>();
        var projectWindow = new ProjectWindow
        {
            ViewModel = projectViewModelFactory.Invoke()
        };

        projectWindow.WhenActivated(d => { container.DisposeWith(d); });

        return projectWindow;
    }

    private async Task InitializeStorage(IComponentContext diContext)
    {
        using var projectStorageInitializer = diContext.Resolve<ProjectStorageInitializer>();
        await projectStorageInitializer.Initialize(CancellationToken.None);
    }

    private async Task CreateTestData(IComponentContext diContext)
    {
        await InitializeStorage(diContext);

        var classroomFeatureGateway = diContext.Resolve<IClassroomFeatureGateway>();

        for (var i = 0; i < 30; i++)
        {
            await classroomFeatureGateway.Create(
                new ClassroomFeature($"Test classroom feature {i}"),
                CancellationToken.None
            );
        }
    }
}