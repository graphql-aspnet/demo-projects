namespace GraphQL.AspNet.Examples.LoggingProvider
{
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Examples.LoggingProvider.Model;
    using GraphQL.AspNet.Examples.LoggingProvider.Provider;
    using GraphQL.AspNet.Interfaces.Schema;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.IO;

    public class Startup
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // apply an unrestricted cors policy for the demo services
            // to allow use on many of the tools for testing (graphiql, altair etc.)
            // Do not do this in production
            services.AddCors(options =>
            {
                options.AddPolicy(
                    ALL_ORIGINS_POLICY,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

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
            services.AddGraphQL(o =>
            {
                o.ApplyDirective<ToUpperDirective>()
                    .ToItems(x =>
                        x is IGraphField gf
                        && gf.Name == "name"
                        && gf.Parent is ITypedSchemaItem ti
                        && ti.ObjectType == typeof(Donut));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(ALL_ORIGINS_POLICY);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL();
        }
    }
}