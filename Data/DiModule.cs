using Autofac;
using Database;

namespace Data;

public class DiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
    }

    private void RegisterAppDbManager(ContainerBuilder builder)
    {
        builder.Register(c => new DatabaseManager<>())
    }
}