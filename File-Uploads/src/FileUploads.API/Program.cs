namespace GraphQL.Aspnet.Examples.MultipartRequest.API
{
    using GraphQL.Aspnet.Examples.MultipartRequest.API.Model;
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.ServerExtensions.MultipartRequests;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class Program
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // apply an unrestricted cors policy for the demo services
            // to allow use on many of the tools for testing (graphiql, altair etc.)
            // Do not do this in production
            builder.Services.AddCors(options =>
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


            AddUploadFolderConfiguration(builder);

            builder.Services.AddControllers();

            // ---------------------------------------------
            // register GraphQL and the server extension with the schema
            // ---------------------------------------------
            builder.Services.AddGraphQL(o =>
            {
                o.AddMultipartRequestSupport();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCors(ALL_ORIGINS_POLICY);
            app.UseGraphQL();
            app.MapControllers();
            app.Run();
        }

        /// <summary>
        /// Adds a custom configuration object that can be injected to tell a controller
        /// where it should save off files uploaded via graphql and how to create URLs to serve
        /// those files to users.
        /// </summary>
        /// <param name="builder">The host builder to .</param>
        private static void AddUploadFolderConfiguration(WebApplicationBuilder builder)
        {
            string urls = builder.Configuration.GetValue<string>("urls");
            var downloadFolder = new Uri(new Uri(urls), "downloads");

            var contentPathRoot = builder.Environment.ContentRootPath;
            var uploadFolder = Path.Combine(contentPathRoot, "UploadedFiles");
            var uploadConfiguration = new UploadConfiguration()
            {
                UploadFolderPath = uploadFolder,
                DownloadFolderUrlBase = downloadFolder.ToString(),
            };

            builder.Services.AddSingleton(uploadConfiguration);
        }
    }
}