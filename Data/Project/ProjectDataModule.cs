using System.Reflection;
using Autofac;
using Storage;
using Module = Autofac.Module;

namespace Data.Project;

public class ProjectDataModule : Module
{
    public StorageMetadata Metadata { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterStorage(builder);

        RegisterRepositories(builder);

        base.Load(builder);
    }

    private void RegisterStorage(ContainerBuilder builder)
    {
        builder.Register(_ => new ProjectStorageInitializer(Metadata))
            .AsSelf();

        builder
            .Register(_ => new StorageProvider(Metadata))
            .AsSelf()
            .OnRelease(p => p.Dispose())
            .SingleInstance();
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("Repository") && t.FullName.Contains("Project"))
            .AsImplementedInterfaces();
    }
}