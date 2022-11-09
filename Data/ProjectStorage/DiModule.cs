using Autofac;

namespace Data.Db;

public class DiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterProjectStorageManager(builder);
        base.Load(builder);
    }

    private static void RegisterProjectStorageManager(ContainerBuilder builder)
    {
        builder
            .Register(c => new ProjectStorageProvider())
            .OnRelease(m => m.Dispose())
            .SingleInstance();
    }
}