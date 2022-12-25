using System;
using System.Collections.Generic;
using System.IO;
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

namespace Infrastructure.Project;

public class ProjectInitializer
{
    private readonly string _name;
    private readonly ILifetimeScope _parentDiScope;
    private readonly string _storageFullPath;

    public ProjectInitializer(string name, string directoryPath, ILifetimeScope parentDiScope)
    {
        _storageFullPath = Path.Join(directoryPath, name);
        _name = name;
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
            ViewModel = projectViewModelFactory.Invoke(_name)
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

        var features = new List<ClassroomFeature>();

        for (var i = 0; i < 30; i++)
        {
            var feature = await classroomFeatureGateway.Create(
                $"Test classroom feature {i}",
                CancellationToken.None
            );
            features.Add(feature);
        }

        var classroomGateway = diContext.Resolve<IClassroomGateway>();
        await classroomGateway.Create(108, 36, features.GetRange(0, 1), CancellationToken.None);

        var disciplineGateway = diContext.Resolve<IDisciplineGateway>();
        var a = await disciplineGateway.Create("programing", features.GetRange(0, 1),
            CancellationToken.None);

        var lecturerGateway = diContext.Resolve<ILecturerGateway>();
        await lecturerGateway.Create("Danil", "Churickov", "Igorevich", new Discipline[] { a },
            CancellationToken.None);
    }
}