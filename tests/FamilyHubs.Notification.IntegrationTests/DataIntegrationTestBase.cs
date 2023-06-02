using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FamilyHubs.Notification.Core;
using FamilyHubs.Notification.Data.Interceptors;
using FamilyHubs.Notification.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace FamilyHubs.Notification.IntegrationTests;

public class DataIntegrationTestBase
{
    public IMapper Mapper { get; }
    public ApplicationDbContext TestDbContext { get; }
    public static NullLogger<T> GetLogger<T>() => new NullLogger<T>();

    public DataIntegrationTestBase()
    {
        var serviceProvider = CreateNewServiceProvider();

        TestDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        Mapper = serviceProvider.GetRequiredService<IMapper>();

        InitialiseDatabase();
    }

    protected static ServiceProvider CreateNewServiceProvider()
    {
        var serviceDirectoryConnection = $"Data Source=sd-{Random.Shared.Next().ToString()}.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;Recursive Triggers=True;Default Timeout=30;Pooling=True";

        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor();

        return new ServiceCollection().AddEntityFrameworkSqlite()
            .AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseSqlite(serviceDirectoryConnection, opt =>
                {
                    opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString());
                });
            })
            .AddSingleton(auditableEntitySaveChangesInterceptor)
            .AddAutoMapper((serviceProvider, cfg) =>
            {
                var auditProperties = new[] { "CreatedBy", "Created", "LastModified", "LastModified" };
                cfg.AddProfile<AutoMappingProfiles>();
                cfg.AddCollectionMappers();
                cfg.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);
                cfg.ShouldMapProperty = pi => !auditProperties.Contains(pi.Name);
            }, typeof(AutoMappingProfiles))
            .BuildServiceProvider();
    }

    private void InitialiseDatabase()
    {
        TestDbContext.Database.EnsureDeleted();
        TestDbContext.Database.EnsureCreated();
    }
}
