using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vouzamo.ERM.Api.Extensions;
using Vouzamo.ERM.Api.Managers;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Microsoft.IdentityModel.Logging;
using Vouzamo.ERM.Providers.Elasticsearch.DI;

namespace Vouzamo.ERM.Api
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
            services.AddSystemTextJsonSerializer();

            services.AddControllers();

            services.AddTransient<IConverter, JsonSerializationConverter>();
            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddElasticsearchProvider(ElasticsearchOptions.Default);

            services.AddGraph();

            services.AddCors();

            IdentityModelEventSource.ShowPII = true;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    options.Audience = Configuration["Jwt:Audience"];
                    options.Authority = Configuration["Jwt:Authority"];
                    options.SaveToken = true;
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseGraph();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
