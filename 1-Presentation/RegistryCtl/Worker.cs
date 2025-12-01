using Common.Core.DependencyInjection;

namespace RegistryCtl
{
    [ServiceLocate(default, ServiceType.Scoped)]
    public class Worker
    {
        public async Task Execute(string[] args)
        {
            // Implementation goes here
        }
    }
}
