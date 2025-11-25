using Common.Core.DependencyInjection;
using Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.ServiceRegistration;

public class ServiceRegister
{
    public static ServiceProvider Register(params string[] assemblies)
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSerilog(
            (configure) =>
                configure.ReadFrom.Configuration(configuration));
        serviceCollection.RegisterDomain(assemblies);
        serviceCollection.AddHttpClient();
        serviceCollection.Configure<RegistryOption>(
            configuration.GetSection(RegistryOption.RegistrySection));
        return serviceCollection.BuildServiceProvider();
    }
}
