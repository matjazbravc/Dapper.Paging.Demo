using Dapper.Paging.Demo.Middleware;
using Dapper.Razor.Demo.Extensions;
using Dapper.Razor.Demo.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Dapper.Paging.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services required for using options
            services.AddOptions();

            // Configure AppSettings
            services.AppSettingsConfiguration(Configuration);

            services.AddRazorPages();

            // Add services
            services.AddTransient<IPersonRepository, PersonRepository>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure Serilog support
            loggerFactory.AddSerilog();

            // Handling Errors Globally with the Custom Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseMvc();
        }
    }
}
