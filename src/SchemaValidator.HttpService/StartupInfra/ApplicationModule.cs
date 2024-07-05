using Autofac;
using SchemaValidator.Shared;

namespace SchemaValidator.HttpService.StartupInfra;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(Core).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        // TODO: uncomment EF modules below
        // builder
        //     .RegisterType<SchemaDbContextFactory>()
        //     .As<IEfDbContextFactory<SchemaDbContext>>()
        //     .InstancePerLifetimeScope();
        //
        // builder
        //     .RegisterType<SchemaDbAccessor>()
        //     .As<IEfDbContextAccessor<SchemaDbContext>>()
        //     .InstancePerLifetimeScope();
        // TODO: uncomment above

        //builder
        //    .RegisterType<EfUnitOfWork>()
        //    .As<IUnitOfWork>()
        //    .InstancePerLifetimeScope();

        //builder
        //    .RegisterType<SilverbackServiceBus>()
        //    .As<IServiceBus>()
        //    .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        // builder.RegisterType<TelemetryFactory>().As<ITelemetryFactory>().InstancePerLifetimeScope();
        // builder.RegisterType<MyCustomEventOtelTelemetry>().As<IMyCustomEventTelemetry>().InstancePerLifetimeScope();
        // builder.RegisterType<OtelMetrics>().As<OtelMetrics>().SingleInstance();
        // builder.RegisterType<OtelVariables>().As<OtelVariables>().SingleInstance();

    }
}