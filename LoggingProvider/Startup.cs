namespace GraphQL.AspNet.Examples.LoggingProvider
{
    using System.IO;
    using GraphQL.AspNet.Configuration.Mvc;
    using GraphQL.AspNet.Examples.LoggingProvider.Provider;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

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
            // -------------------------------------------
            // The file where the logs will be written
            // Change this to a more permanent location
            // -------------------------------------------
            var fileName = Path.GetTempFileName();

            // register the log provider
            // ------
            // also don't forget to register a log level setting in `appsettings.json`
            // for "GraphQL.AspNet"
            services.AddLogging(options =>
            {
                options.AddProvider(new JsonLogFileProvider(fileName));
            });

            services.AddControllers();

            // make sure to register graphql AFTER adding logging
            services.AddGraphQL();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL();
        }
    }
}