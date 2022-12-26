using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Adapters.Preview;

public class AdaptersPreviewModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("Gateway") && t.FullName.Contains("Preview"))
            .AsImplementedInterfaces();

        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("ViewModel") && t.FullName.Contains("Preview"));

        base.Load(builder);
    }
}