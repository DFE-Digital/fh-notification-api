using AutoMapper;
using FamilyHubs.Notification.Core;
using FamilyHubs.Notification.Data.Interceptors;
using FamilyHubs.Notification.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.Notification.UnitTests;

public class BaseCreateDbUnitTest
{
    protected BaseCreateDbUnitTest()
    {
    }
    protected static ApplicationDbContext GetApplicationDbContext()
    {
        var options = CreateNewContextOptions();
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor();
        var mockApplicationDbContext = new ApplicationDbContext(options, auditableEntitySaveChangesInterceptor);

        return mockApplicationDbContext;
    }

    protected static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseInMemoryDatabase("NotificationDb")
               .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }

    protected IMapper GetMapper()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        return mapper;
    }
}
