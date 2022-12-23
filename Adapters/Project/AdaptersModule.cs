using Adapters.Common.ViewModels;
using Autofac;

namespace Adapters.Project;

public class AdaptersModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MessageViewModel>(); 
        
        base.Load(builder);
    }
}