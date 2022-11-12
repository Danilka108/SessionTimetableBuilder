using Autofac;
using Data.Project;

namespace Data;

public class DataModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ProjectModule>();

        base.Load(builder);
    }
}