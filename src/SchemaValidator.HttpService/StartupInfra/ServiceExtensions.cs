using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SchemaValidator.HttpService.Shared;
using Serilog;
using Serilog.Exceptions;
using Serilog.Filters;

namespace SchemaValidator.HttpService.StartupInfra;

internal static class ServicesExtensions
{
    // public static IServiceCollection AddTelemetry(this IServiceCollection serviceCollection, string serviceName,
    //     string serviceVersion, IConfiguration configuration)
    // {
    //     var environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    //
    //     TelemetrySettings settings;
    //
    //     if (configuration.GetSection("OpenTelemetry") is var section && section.Exists())
    //     {
    //         settings = new TelemetrySettings(serviceName, serviceVersion,
    //             new TelemetryExporter(section["Type"] ?? string.Empty, section["PostEndpoint"] ?? string.Empty));
    //     }
    //     else
    //     {
    //         settings = new TelemetrySettings(serviceName, serviceVersion,
    //             new TelemetryExporter("console", ""));
    //     }
    //
    //     serviceCollection.AddSingleton(settings);
    //     serviceCollection.AddScoped(sp => new OtelTracingService(sp.GetService<TelemetrySettings>()));
    //
    //     Action<ResourceBuilder> configureResource = r => r
    //         .AddService(
    //             serviceName: settings.ServiceName,
    //             serviceVersion: settings.ServiceVersion,
    //             serviceInstanceId: System.Environment.MachineName)
    //         .AddAttributes(new Dictionary<string, object> { ["deployment.environment"] = environmentName });
    //
    //     serviceCollection
    //         .AddOpenTelemetry()
    //         .ConfigureResource(configureResource)
    //         .WithTracing(builder =>
    //         {
    //             builder
    //                 .SetSampler(new AlwaysOnSampler())
    //                 .AddSource(settings.ServiceName)
    //                 .AddSource("Silverback.Integration.Produce")
    //                 .AddHttpClientInstrumentation(o =>
    //                     o.FilterHttpWebRequest = request => !request.Address.AbsoluteUri.Contains("/raw"))
    //                 .AddAspNetCoreInstrumentation(opts =>
    //                 {
    //                     opts.EnrichWithHttpRequest = (a, r) => a?.AddTag("env", environmentName);
    //
    //                 });
    //             //.AddSource("Npgsql");
    //             builder.AddConsoleExporter();
    //             switch (settings.Exporter.Type.ToLower())
    //             {
    //
    //                 case "otlp":
    //                     builder.AddOtlpExporter(config =>
    //                     {
    //                         config.Endpoint = new Uri(settings.Exporter.Endpoint ?? string.Empty);
    //                         //config.ExportProcessorType = ExportProcessorType.Simple;
    //                         config.Protocol = OtlpExportProtocol.Grpc;
    //                     });
    //                     break;
    //                 default:
    //                     builder.AddConsoleExporter();
    //                     break;
    //             }
    //         })
    //         .WithMetrics(builder =>
    //         {
    //             // builder
    //             //     .ConfigureResource(configureResource)
    //             //     .AddMeter(new OtelMetrics().MeterName)
    //             //     .AddRuntimeInstrumentation()
    //             //     .AddAspNetCoreInstrumentation();
    //             // switch (settings.Exporter.Type.ToLower())
    //             // {
    //             //     case "otlp" :
    //             //         builder.AddOtlpExporter(config =>
    //             //         {
    //             //             config.PostEndpoint = new Uri(settings.Exporter.PostEndpoint ?? string.Empty);
    //             //         });
    //             //         break;
    //             //     default:
    //             //         builder.AddConsoleExporter();
    //             //         break;
    //             // }
    //         });
    //     return serviceCollection;
    // }
    //
    // public static IServiceCollection AddKafka(
    //     this IServiceCollection services,
    //     IConfiguration configuration)
    // {
    //     // IConfigurationSection kafkaSection = configuration.GetSection("Kafka");
    //     // var kafkaConfig = new KafkaConfig();
    //     // kafkaConfig.Connection = kafkaSection.GetSection("Connection").Get<KafkaConnectionConfig>()!;
    //     // services.AddSingleton(kafkaConfig);
    //     // services
    //     //     .AddSilverback()
    //     //     .WithConnectionToMessageBroker(options => options.AddKafka())
    //     //     .AddEndpointsConfigurator<KafkaEndpointsConfigurator>();
    //     return services;
    // }
    //
    public static IServiceCollection AddOpenApiSpecs(this IServiceCollection services)
    {
        services.AddOpenApiDocument();
        return services;
    }
    
    // public static IServiceCollection AddVersioning(this IServiceCollection services)
    // {
    //     services.AddApiVersioning(config =>
    //     {
    //         config.DefaultApiVersion = new ApiVersion(1, 0);
    //         config.AssumeDefaultVersionWhenUnspecified = true;
    //         config.ReportApiVersions = true;
    //     });
    //
    //     return services;
    // }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(
            o =>
                o.AddPolicy(
                    "default",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    }
                )
        );

        return services;
    }

    public static IServiceCollection AddHealth(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy(), new string[] { "ready" });
        // hcBuilder
        //     .AddNpgSql(
        //         configuration.GetConnectionString(Ambient.DatabaseConnectionName)!,
        //         name: "integration-store-check",
        //         tags: new string[] {"IntegrationStoreCheck", "health"});
        return services;
    }

    // public static IServiceCollection AddHttpClients(this IServiceCollection services)
    //     => services;
    //
    // public static IServiceCollection AddWorkersServices(this IServiceCollection services, IConfiguration configuration)
    // {
    //     return services;
    // }

    // public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    // {
    //     services
    //         .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //         .AddMicrosoftIdentityWebApi(configuration);
    //     return services;
    // }

    public static IServiceCollection AddLogs(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            // .Enrich.WithDatadogTraceId()
            // .Enrich.WithDatadogSpanId()
            .Enrich.WithExceptionDetails()
            //.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Hosting.Diagnostics"))
            //.Filter.ByExcluding(Matching.FromSource("Microsoft.Hosting.Lifetime"))
            .Filter.ByExcluding(
                Matching.FromSource("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager")
            )
            .CreateLogger();
        services.AddSingleton(Log.Logger);
        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddSingleton<IMemoryCache>(
            o =>
            {
                return new MemoryCache(new MemoryCacheOptions());
            });

        //services.AddMemoryCache();
        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        var assembly = Assembly.Load(typeof(Env).Assembly.ToString());
        services
            .AddControllers()
            .AddApplicationPart(assembly);
        return services;
    }

    public static IServiceCollection AddHttpGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<HttpGlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}