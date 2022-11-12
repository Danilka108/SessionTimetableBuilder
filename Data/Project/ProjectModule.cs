using Autofac;
using Data.Project.Entities;
using Data.Project.Repositories;
using Domain;

namespace Data.Project;

internal class ProjectModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterStorage(builder);

        RegisterRepositories(builder);

        base.Load(builder);
    }

    private static void RegisterStorage(ContainerBuilder builder)
    {
        builder.RegisterType<ProjectStorageMetadata>();

        builder.RegisterType<ProjectStorageInitializer>();

        builder
            .RegisterType<ProjectStorageProvider>()
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