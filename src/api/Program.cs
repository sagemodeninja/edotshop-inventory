using System.Reflection;
using EdotShop.Services.Inventory;
using EdotShop.Services.Inventory.Classes;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

string CorsPolicy = nameof(CorsPolicy);

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<ServiceContext>(options => {
    var connectionName = nameof(ServiceContext);
    var connectionString = configuration.GetConnectionString(connectionName);

    options.UseSqlServer(connectionString);
});

var allowedOrigins = configuration["AllowedHosts"].Split(";", StringSplitOptions.RemoveEmptyEntries);
services.AddCors(options => {
    options.AddPolicy(
        name: CorsPolicy,
        builder => {
            builder.WithOrigins(allowedOrigins)
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

services.AddControllers()
        .AddOData(options => {
            options.Count().Select().Filter().Expand().SetMaxTop(1000);
            options.RouteOptions.EnableKeyAsSegment = true;
            options.RouteOptions.EnableKeyInParenthesis = false;
            options.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
            options.RouteOptions.EnableQualifiedOperationCall = false;
            options.RouteOptions.EnableUnqualifiedOperationCall = true;
        });

services.AddApiVersioning(options => {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;  
        })
        .AddOData(options => options.AddRouteComponents("api/v{version:apiVersion}"))
        .AddODataApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
services.ConfigureOptions<ConfigureSwaggerOptions>();
services.AddSwaggerGen(options => {
    // Patches
    options.OperationFilter<SwaggerDefaultValues>();
    options.CustomOperationIds(e => null);
    
    // Documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlCommentPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => {
        options.PreSerializeFilters.Add((swagger, request) => {
            var server = new OpenApiServer
            {
                Url = $"{request.Scheme}://{request.Host.Value}"
            };
            swagger.Servers = new List<OpenApiServer> { server };
        });
    });

    app.UseSwaggerUI(options => {
        // Endpoints
        var groups = app.DescribeApiVersions()
                        .Select(desc => desc.GroupName);

        foreach (var group in groups)
        {
            var url = $"/swagger/{group}/swagger.json";
            var name = group.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    app.UseODataRouteDebug();
}

// Vulnerability patches...
app.Use(async (context, next) => {
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

    // Cache-control
    context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
    {
        NoCache = true,
        NoStore = true,
        MustRevalidate = true
    };
    context.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

    await next();
});

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.MapControllers();
app.Run();
