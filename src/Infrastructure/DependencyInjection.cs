using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Infrastructure.Files;
using FusionIT.TimeFusion.Infrastructure.Identity;
using FusionIT.TimeFusion.Infrastructure.Persistence;
using FusionIT.TimeFusion.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FusionIT.TimeFusion.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<FusionTimeDbContext>(options =>
                    options.UseInMemoryDatabase("FusionIT.TimeFusionDb"));
            }
            else
            {
                services.AddDbContext<FusionTimeDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(FusionTimeDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<FusionTimeDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<FusionTimeDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, FusionTimeDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
}