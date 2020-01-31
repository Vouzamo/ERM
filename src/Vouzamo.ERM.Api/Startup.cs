using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vouzamo.ERM.Api.Extensions;
using Vouzamo.ERM.Api.Managers;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.DTOs;
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

            services.AddSingleton<INotificationManager, NotificationManager>();
            services.AddElasticsearchProvider(ElasticsearchOptions.Default);

            services.AddGraph();

            services.AddTransient<IConverter, JsonSerializationConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGraph();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
