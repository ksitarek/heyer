using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Json;

public static class Json
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        Converters = { new JsonStringEnumConverter() }
    };

    public static IServiceCollection ConfigureJson(this IServiceCollection services) =>
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy =
                SerializerOptions.PropertyNamingPolicy;

            options.SerializerOptions.DefaultIgnoreCondition =
                SerializerOptions.DefaultIgnoreCondition;

            foreach (var converter in SerializerOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }
        });

    public static T? Deserialize<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json, SerializerOptions);

    public static object? Deserialize(this string json, Type type) =>
        JsonSerializer.Deserialize(json, type, SerializerOptions);

    public static string Serialize<T>(this T value) =>
        JsonSerializer.Serialize(value, SerializerOptions);
}