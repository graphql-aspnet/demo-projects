namespace Firebase.AuthTest
{
    using GraphQL.AspNet.Configuration.Mvc;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;

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
            var firebaseProjectId = this.Configuration.GetValue<string>("firebaseProjectId");
            if (string.IsNullOrWhiteSpace(firebaseProjectId))
                throw new ArgumentException("Firebase Project Id must be set.", nameof(firebaseProjectId));

            // Add JWT Bearer Authentication and set it as the default authentication
            // method
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Authority should be your firebase instance
                options.Authority = "https://securetoken.google.com/" + firebaseProjectId;
                options.Audience = "http://localhost:5000";
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = false, // its a good idea to validate audience in production
                    ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
                };

                // other options to add
                // options.SaveToken = true;
                // options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization();
            services.AddGraphQL();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // make sure to call Authentication and Authorization before GraphQL
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphQL();
        }
    }
}