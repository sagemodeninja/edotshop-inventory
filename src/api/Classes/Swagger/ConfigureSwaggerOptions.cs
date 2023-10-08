using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EdotShop.InventoryServices.Classes;

/// <summary>
/// Configure swagger options.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly string _environment;

    /// <summary>
    /// Creates an instance of <see cref="ConfigureSwaggerOptions"/>.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="configuration"></param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider,
        IConfiguration configuration)
    {
        _provider = provider;
        _environment = configuration["ASPNETCORE_ENVIRONMENT"];
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var versionInfo = CreateVersionInfo(description);
            options.SwaggerDoc(description.GroupName, versionInfo);
        }
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"EdotShop Inventory Service API ({_environment})",
            Version = description.ApiVersion.ToString(),
            Description = ""
        };

        if (description.IsDeprecated)
        {
            info.Description += " [Deprecated]";
        }

        return info;
    }
}