using Autofac;
using Database;

namespace Data.Db;

public class DiModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new DatabaseManager<>())

        // base.Load(builder);
    }
}