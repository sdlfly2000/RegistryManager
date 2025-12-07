using Common.Core.DependencyInjection;
using Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.ServiceRegistration;

public class ServiceRegister
{
    public static IServiceCollection Register(params string[] assemblies)
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSerilog(
            (configure) =>
                configure.ReadFrom.Configuration(configuration));
        if (assemblies.Length > 0)
        {
            serviceCollection.RegisterDomain(assemblies);
        }
        serviceCollection.AddHttpClient();
        serviceCollection.Configure<RegistryOption>(
            configuration.GetSection(RegistryOption.RegistrySection));
        serviceCollection.AddMemoryCache();
        return serviceCollection;
    }
}
