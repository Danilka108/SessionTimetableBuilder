using Autofac;

namespace Data;

public class DiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterDbModule(builder);
    }

    private void RegisterDbModule(ContainerBuilder builder)
    {
        builder.RegisterModule<Db.DiModule>();
    }
}