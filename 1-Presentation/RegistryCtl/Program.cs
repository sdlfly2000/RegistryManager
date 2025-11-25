using Core.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;
using RegistryCtl;

using var serviceProvider = ServiceRegister.Register("RegistryCtl");

var worker = serviceProvider.GetRequiredService<Worker>();

await worker.Execute();
