using Core.ServiceRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Test;

public abstract class BaseIntegrationTest
{
    protected IServiceProvider _serviceProvider;

    protected BaseIntegrationTest()
    {
        _serviceProvider = ServiceRegister.Register("Domain", "Infra.Http").BuildServiceProvider();
    }
}
