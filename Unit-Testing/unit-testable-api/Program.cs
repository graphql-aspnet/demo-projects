namespace GraphQL.AspNet.Examples.TestApi
{
    using GraphQL.AspNet.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddGraphQL();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthorization();
            app.UseGraphQL();
            app.Run();
        }
    }
}