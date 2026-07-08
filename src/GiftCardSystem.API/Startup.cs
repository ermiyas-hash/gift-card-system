using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Persistence;
using GiftCardSystem.Application.Services;

namespace GiftCardSystem.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<GiftCardDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IGiftCardService, GiftCardService>();
            services.AddScoped<IRedemptionService, RedemptionService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            services.AddScoped<ICustomerService, CustomerService>();

            // Controllers
            services.AddControllers();

            // Swagger/OpenAPI
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Gift Card Management System API",
                    Version = "v1",
                    Description = "API for managing gift cards, redemptions, and analytics"
                });
            });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gift Card API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
