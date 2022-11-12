using Autofac;

namespace Data.ProjectStorage;

public class ProjectStorageModule : Module
{
    private ContainerBuilder _builder;

    protected override void Load(ContainerBuilder builder)
    {
        _builder = builder;

        RegisterMetadata();
        RegisterInitializer();
        RegisterProvider();

        base.Load(builder);
    }

    private void RegisterMetadata()
    {
        _builder.RegisterType<ProjectStorageMetadata>();
    }

    private void RegisterInitializer()
    {
        _builder.RegisterType<ProjectStorageInitializer>();
    }

    private void RegisterProvider()
    {
        _builder
            .RegisterType<ProjectStorageProvider>()
            .OnRelease(p => p.Dispose())
            .SingleInstance();
    }
}