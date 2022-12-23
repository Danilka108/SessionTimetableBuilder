using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adapters.Project;
using Adapters.Project.ViewModels;
using Application.Project;
using Application.Project.Gateways;
using Autofac;
using Avalonia.Controls;
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
    private readonly ILifetimeScope _parentDiScope;

    public ProjectInitializer(string storageFullPath, ILifetimeScope parentDiScope)
    {
        _storageFullPath = storageFullPath;
        _parentDiScope = parentDiScope;
    }

    public async Task<Window> Initialize()
    {
        var projectDiScope = _parentDiScope.BeginLifetimeScope(builder =>
        {
            builder.RegisterModule(new AdaptersProjectModule
            {
                StorageResourceFactory = () => new FileStorageResource(_storageFullPath)
            });
            
            builder.RegisterModule(new ApplicationProjectModule());
        });

        try
        {
            await CreateTestData(projectDiScope);
        }
        catch (Exception)
        {
            projectDiScope.Dispose();
            throw;
        }

        var projectViewModelFactory = projectDiScope.Resolve<ProjectViewModel.Factory>();
        var projectWindow = new ProjectWindow
        {
            DataContext = projectViewModelFactory.Invoke()
        };

        projectWindow.WhenActivated(d => { projectDiScope.DisposeWith(d); });

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