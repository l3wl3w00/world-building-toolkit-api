using Dal;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Test.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=worldbuilder;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                services.AddScoped(sp => new DbContextOptionsBuilder<WorldBuilderDbContext>()
                        .UseSqlServer(ConnectionString)
                        .UseApplicationServiceProvider(sp)
                        .Options);
            });

            var host = base.CreateHost(builder);

            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<WorldBuilderDbContext>()
                .Database.EnsureCreated();

            return host;
        }
    }
}
