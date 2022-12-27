namespace GraphQL.AspNet.Examples.Authorization
{
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Examples.Authorization.Users;
    using GraphQL.AspNet.Security;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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

            // Configure http authentication against the in memory user store
            // This user store is for demo purposes only
            // ---------------------------
            services.AddIdentity<AppUser, AppRole>()
                .AddRoleStore<RoleStore>()
                .AddUserStore<UserStore>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = IdBasedAuthOptions.DefaultScheme;
                o.DefaultChallengeScheme = IdBasedAuthOptions.DefaultScheme;
            })
            .AddIdBasedAuthentication();

            services.AddControllers();
            services.AddGraphQL(options =>
            {
                options.AuthorizationOptions.Method = AuthorizationMethod.PerField;
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL();
        }
    }
}