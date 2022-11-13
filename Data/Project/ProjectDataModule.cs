using Autofac;
using Data.Project.Repositories;
using Domain;
using Domain.Project.Models;
using Storage;

namespace Data.Project;

public class ProjectDataModule : Module
{
    public StorageMetadata Metadata { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterStorage(builder);

        RegisterRepositories(builder);

        base.Load(builder);
    }

    private void RegisterStorage(ContainerBuilder builder)
    {
        builder.Register(_ => new ProjectStorageInitializer(Metadata)).AsSelf();

        builder
            .Register(_ => new StorageProvider(Metadata))
            .AsSelf()
            .OnRelease(p => p.Dispose())
            .SingleInstance();
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder
            .RegisterType<BellTimeRepository>()
            .As<IRepository<BellTime>>();

        builder
            .RegisterType<AudienceSpecificityRepository>()
            .As<IRepository<AudienceSpecificity>>();

        builder
            .RegisterType<AudienceRepository>()
            .As<IRepository<Audience>>();

        builder
            .RegisterType<DisciplineRepository>()
            .As<IRepository<Discipline>>();

        builder
            .RegisterType<GroupRepository>()
            .As<IRepository<Group>>();

        builder
            .RegisterType<TeacherRepository>()
            .As<IRepository<Teacher>>();

        builder
            .RegisterType<ExamRepository>()
            .As<IRepository<Exam>>();
    }
}