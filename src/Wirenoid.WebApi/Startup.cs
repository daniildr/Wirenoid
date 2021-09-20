using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Wirenoid.WebApi.DbItems;
using Wirenoid.Core;
using Wirenoid.Core.Models;

namespace Wirenoid.WebApi
{
    public class Startup
    {
        private const string _wairenoidCorsPolicyName = "_wirenoidCORSPolicyName";
        private readonly OpenApiContact _apiContact = new()
        {
            Name = "Daniil DR",
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
            services.Configure<CoreSettings>(Configuration.GetSection("CoreSettings"));
            services.AddCors(options =>
            {
                options.AddPolicy(name: _wairenoidCorsPolicyName,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080");
                    });
            });
            services.AddSingleton(typeof(WirenoidCore));
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