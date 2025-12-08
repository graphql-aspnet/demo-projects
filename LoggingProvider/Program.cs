using GraphQL.AspNet.Configuration;
using GraphQL.AspNet.Examples.LoggingProvider.Model;
using GraphQL.AspNet.Examples.LoggingProvider.Provider;
using GraphQL.AspNet.Interfaces.Schema;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

const string ALL_ORIGINS_POLICY = "_allOrigins";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

// -------------------------------------------
// The file where the logs will be written
// Change this to a more permanent location
// -------------------------------------------
string fileName = Path.GetTempFileName();

// register the log provider
// ------
// also don't forget to register a log level setting in `appsettings.json`
// for "GraphQL.AspNet"
builder.Services.AddLogging(options =>
{
    options.AddProvider(new JsonLogFileProvider(fileName));
});

builder.Services.AddControllers();

// make sure to register graphql AFTER adding logging
builder.Services.AddGraphQL(o =>
{
    o.ApplyDirective<ToUpperDirective>()
        .ToItems(x =>
            x is IGraphField gf
            && gf.Name == "name"
            && gf.Parent is ITypedSchemaItem ti
            && ti.ObjectType == typeof(Donut));
});

WebApplication app = builder.Build();

app.UseRouting();

app.UseCors(ALL_ORIGINS_POLICY);

app.UseAuthorization();

app.MapControllers();

app.UseGraphQL();

app.Run();