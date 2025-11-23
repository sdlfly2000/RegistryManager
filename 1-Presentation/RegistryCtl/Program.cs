using Common.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegistryCtl;
using Serilog;

var serviceCollection = new ServiceCollection();

var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

// Add Serilog Support
serviceCollection.AddSerilog(
    (configure) =>
        configure.ReadFrom.Configuration(configuration));

// Register Services
serviceCollection
    .RegisterDomain("RegistryCtl");

using var serviceProvider = serviceCollection.BuildServiceProvider();
var worker = serviceProvider.GetRequiredService<Worker>();

await worker.Execute();
