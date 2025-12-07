using Core.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Application.BDD.Support
{
    internal class SetupBDDTestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            return ServiceRegister.Register("Application", "Domain", "Infra.Http");
        }
    }
}
