using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace ProjectPresentation;

public class ProjectPresentationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where
            (
                t =>
                {
                    return t.FullName!.Contains("Project") && t.FullName!.EndsWith("ViewModel");
                }
            )
            .AsSelf();

        base.Load(builder);
    }
}