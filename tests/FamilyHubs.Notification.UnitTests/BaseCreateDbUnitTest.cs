using AutoMapper;
using FamilyHubs.Notification.Core;
using FamilyHubs.Notification.Data.Interceptors;
using FamilyHubs.Notification.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace FamilyHubs.Notification.UnitTests;

public class BaseCreateDbUnitTest
{
    protected BaseCreateDbUnitTest()
    {
    }
    protected static ApplicationDbContext GetApplicationDbContext()
    {
        var options = CreateNewContextOptions();
        var mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();

        context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim("OrganisationId", "1"),
            new Claim("AccountId", "2"),
            new Claim("AccountStatus", "Active"),
            new Claim("Name", "John Doe"),
            new Claim("ClaimsValidTillTime", "2023-09-11T12:00:00Z"),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "john@example.com"),
            new Claim("PhoneNumber", "123456789")
        }, "test"));

        mockIHttpContextAccessor.Setup(h => h.HttpContext).Returns(context);
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockIHttpContextAccessor.Object);
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
