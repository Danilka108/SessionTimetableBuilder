using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adapters.Project;
using Adapters.Project.ViewModels;
using Application.Project;
using Autofac;
using Avalonia.Controls.Mixins;
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

        await InitializeStorage(container);

        var projectViewModelFactory = container.Resolve<ProjectViewModel.Factory>();
        var projectWindow = new ProjectWindow
        {
            ViewModel = projectViewModelFactory.Invoke()
        };

        projectWindow.WhenActivated(d => { container.DisposeWith(d); });

        return projectWindow;
    }

    private async Task InitializeStorage(ILifetimeScope rootScope)
    {
        using var projectStorageInitializer = rootScope.Resolve<ProjectStorageInitializer>();
        await projectStorageInitializer.Initialize(CancellationToken.None);
    }

    private async Task CreateTestData(IComponentContext container)
    {
        var storageInitializer = container.Resolve<ProjectStorageInitializer>();

        await storageInitializer.Initialize(CancellationToken.None);

        // var audienceSpecificitiesRepository =
        //     container.Resolve<IAudienceSpecificityRepository>();
        //
        // var audienceRepository = container.Resolve<IAudienceRepository>();
        //
        // var disciplineRepository = container.Resolve<IDisciplineRepository>();
        //
        // var audienceSpecificities = new List<IdentifiedModel<AudienceSpecificity>>();
        //
        // for (var i = 0; i < 20; i++)
        // {
        //     var specificity = await audienceSpecificitiesRepository.Create
        //     (
        //         new AudienceSpecificity($"Specificity {i}"),
        //         CancellationToken.None
        //     );
        //
        //     audienceSpecificities.Add(specificity);
        // }
        //
        // for (var i = 0; i < 20; i++)
        //     await audienceRepository.Create
        //     (
        //         new Audience(i, 30, audienceSpecificities),
        //         CancellationToken.None
        //     );
        //
        // for (var i = 0; i < 20; i++)
        // {
        //     var a = new IdentifiedModel<AudienceSpecificity>[] { };
        //     await disciplineRepository.Create(new Discipline($"Discipline {i}", a),
        //         CancellationToken.None);
        // }
    }
}