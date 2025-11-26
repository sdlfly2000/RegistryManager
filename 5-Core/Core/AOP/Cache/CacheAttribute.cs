using ArxOne.MrAdvice.Advice;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.AOP.Cache;

public class CacheAttribute : Attribute, IMethodAsyncAdvice
{
    public string MasterKey { get; set; }
    public Type[] CachedTypes { get; set; }

    public CacheAttribute(string masterKey, params Type[] cachedTypes)
    {
        MasterKey = masterKey;
        CachedTypes = cachedTypes;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var serviceProvider = context.GetMemberServiceProvider();
        var memoryCache = serviceProvider?.GetRequiredService<IMemoryCache>();

        var cacheKeyUnique = MasterKey;

        foreach (var cachedType in CachedTypes)
        {
            cacheKeyUnique = string.Concat(cacheKeyUnique, "-", GetKeyValue(context.Arguments, cachedType));
        }
        
        await context.ProceedAsync();

    }

    private string? GetKeyValue(IList<Object> argements, Type cachedType)
    {
        var cachedArgument = argements.Single(arg => arg.GetType() == cachedType);

        if (cachedArgument is null)
        {
            return string.Empty;
        }

        var keyProperty = cachedArgument?.GetType().GetProperties().Single(prop =>
        {
            var cachedKeyAttribute = prop.GetCustomAttribute<CacheKeyAttribute>();
            return cachedKeyAttribute is not null;
        });

        return keyProperty?.GetValue(cachedArgument)?
                           .ToString();
    }
}
