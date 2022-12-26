using System.Reflection;
using Autofac;
using Storage;
using Module = Autofac.Module;

namespace Adapters.Project;

public class AdaptersProjectModule : Module
{
    public Func<StorageResource> StorageResourceFactory { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register(_ => new Storage.Storage(StorageResourceFactory.Invoke()))
            .SingleInstance();

        builder
            .Register(_ => new StorageInitializer(StorageResourceFactory.Invoke()))
            .InstancePerDependency()
            .ExternallyOwned();

        builder
            .RegisterType<ProjectStorageInitializer>()
            .ExternallyOwned();

        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("Gateway") && t.FullName.Contains("Project"))
            .AsImplementedInterfaces().AsSelf();
        //     
        // builder
        //     .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        //     .Where(t => t.FullName.EndsWith("Gateway") && t.FullName.Contains("Project"))
        // .AsSelf();

        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("ViewModel") && t.FullName.Contains("Project"));

        base.Load(builder);
    }
}