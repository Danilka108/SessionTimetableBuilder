using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Application.Project;

public class ApplicationProjectModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.Name.EndsWith("UseCase"))
            .AsSelf();

        base.Load(builder);
    }
}