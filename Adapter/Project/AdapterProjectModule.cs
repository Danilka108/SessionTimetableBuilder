using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Adapter.Project;

public class AdapterProjectModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<Storage.Storage>()
            .OnRelease(s => s.Dispose());
        
        builder
            .RegisterType<ProjectStorageInitializer>()
            .OnRelease(s => s.Dispose());
        
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("Gateway") && t.FullName.Contains("Project"))
            .AsImplementedInterfaces();

        base.Load(builder);
    }
}