using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Wirenoid.Core;
using Wirenoid.Core.Interfaces;
using Wirenoid.Core.Models;
using Wirenoid.WebApi.DbItems;

namespace Wirenoid.WebApi
{
    public class Startup
    {
        private const string _wairenoidCorsPolicyName = "_wirenoidCORSPolicyName";
        private readonly OpenApiContact _apiContact = new()
        {
            Name = "Developed by Daniil Drozdov © DaniilDR",
            Email = "wirenoid-support@daniildr.ru",
            Url = new("https://github.com/daniildr/Wirenoid")
        };
        private readonly OpenApiLicense _apiLicense = new() 
        { 
            Name = "GNU Lesser General Public License",
            Url = new("https://www.fsf.org/")
        };
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CacheDbContext>(opt =>
                opt.UseInMemoryDatabase("LaunchedCache"));
            services.Configure<DockerHubSettings>(Configuration.GetSection("DockerHubSettings"));
            services.Configure<DockerSettings>(Configuration.GetSection("DockerSettings"));
            services.Configure<DockerImageSettings>(Configuration.GetSection("DockerImageSettings"));
            services.Configure<NetworkSettings>(Configuration.GetSection("NetworkSettings"));
            services.AddCors(options =>
            {
                options.AddPolicy(name: _wairenoidCorsPolicyName,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080");
                    });
            });
            services.AddSingleton<IDockerManager, WirenoidCore>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Wirenoid Web API", 
                    Version = "v1", 
                    Contact = _apiContact, 
                    License = _apiLicense, 
                    Description = "Web API for Wirenoid" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wirenoid.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(_wairenoidCorsPolicyName);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
