namespace GraphQL.AspNet.Examples.CustomDirectives
{
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Examples.CustomDirectives.Directives;
    using GraphQL.AspNet.Examples.CustomDirectives.Model;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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

            services.AddGraphQL(o =>
            {
                // ************************************************
                // SAMPLE EXECUTION DIRECTIVE
                // ************************************************
                // add the @toSarcastic directive to the schema
                // so that it can be used by any callers executing
                // queries
                //
                // Note: Directives are automatically discovered if you
                //       you use .AddAssembly()
                //
                o.AddDirective<ToSarcasticDirective>();

                // ************************************************
                //  SAMPLE TYPE SYSTEM DIRECTIVE
                // ************************************************
                // Late-bind the @toUpper directive to the Donut.Name field definition
                // during schema generation.
                //
                // This directive can also be applied using the [ApplyDirective]
                // attribute directly to Donut.Name is the class definition.
                //
                // The .WithArguments() call is to demostrate how you would
                // pass arguments to a directive for invocation
                // this directive requires no arguments so we supply an
                // empty set of parameters.  This is also the default behavior
                // so this call is not strictly necessary unless you have parameters
                // to pass in.
                //
                o.ApplyDirective<ToUpperDirective>()
                    .WithArguments(new object[0])
                    .ToItems(x => x.IsField<Donut>("name"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(ALL_ORIGINS_POLICY);

            app.UseGraphQL();
        }
    }
}