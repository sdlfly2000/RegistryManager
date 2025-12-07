using Core.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;
using RegistryCtl;

using var serviceProvider = ServiceRegister
                                .Register("RegistryCtl", "Application", "Domain", "Infra.Http")
                                .BuildServiceProvider();

var worker = serviceProvider.GetRequiredService<Worker>();

await worker.Execute(args);
