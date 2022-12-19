using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Project.ProjectWindow;
using Autofac;
using Avalonia.Controls.Mixins;
using Data.Project;
using Domain;
using Domain.Project;
using Domain.Project.Models;
using Domain.Project.Repositories;
using ProjectPresentation;
using ReactiveUI;
using Storage;

namespace App.Project;

public class ProjectInitializer
{
    private readonly StorageMetadata _storageMetadata;

    public ProjectInitializer(StorageMetadata storageMetadata)
    {
        _storageMetadata = storageMetadata;
    }

    public async Task<ProjectWindow.ProjectWindow> Initialize()
    {
        var diContainer = InitializeDiContainer();

        await CreateTestData(diContainer);

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
        var window = new ProjectWindow.ProjectWindow
        {
            DataContext = windowViewModel
        };

        window.WhenActivated(d => diContainer.DisposeWith(d));

        return window;
    }

    private async Task CreateTestData(IComponentContext container)
    {
        var storageInitializer = container.Resolve<ProjectStorageInitializer>();

        await storageInitializer.Initialize(CancellationToken.None);

        var audienceSpecificitiesRepository =
            container.Resolve<IAudienceSpecificityRepository>();

        var audienceRepository = container.Resolve<IAudienceRepository>();

        var disciplineRepository = container.Resolve<IDisciplineRepository>();

        var audienceSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();

        for (var i = 0; i < 20; i++)
        {
            var specificity = await audienceSpecificitiesRepository.Create
            (
                new AudienceSpecificity($"Specificity {i}"),
                CancellationToken.None
            );

            audienceSpecificities.Add(specificity);
        }

        for (var i = 0; i < 20; i++)
            await audienceRepository.Create
            (
                new Audience(i, 30, audienceSpecificities),
                CancellationToken.None
            );

        for (var i = 0; i < 20; i++)
        {
            var a = new IdentifiedModel<AudienceSpecificity>[] { };
            await disciplineRepository.Create(new Discipline($"Discipline {i}", a),
                CancellationToken.None);
        }
    }

    private ILifetimeScope InitializeDiContainer()
    {
        var autofacBuilder = new ContainerBuilder();

        autofacBuilder.RegisterModule
        (
            new ProjectDataModule
            {
                Metadata = _storageMetadata
            }
        );

        autofacBuilder.RegisterModule<ProjectDomainModule>();

        autofacBuilder.RegisterModule<ProjectPresentationModule>();

        var container = autofacBuilder.Build();
        return container.BeginLifetimeScope();
    }
}

public class InitializeProjectException : Exception
{
    internal InitializeProjectException(string msg, Exception innerException) : base
        (msg, innerException)
    {
    }
}