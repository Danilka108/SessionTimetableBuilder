using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Adapters.Common;

public class AdaptersCommonModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("ViewModel") && t.FullName.Contains("Common"));
        
        builder
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(t => t.FullName.EndsWith("Validator") && t.FullName.Contains("Common"));
        
        base.Load(builder);
    }
}