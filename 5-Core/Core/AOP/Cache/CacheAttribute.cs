using ArxOne.MrAdvice.Advice;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Core.AOP.Cache;

public class CacheAttribute : Attribute, IMethodAsyncAdvice
{
    public string MasterKey { get; set; }
    public Type[] CachedTypes { get; set; }
    public Type ReturnType { get; set; }

    public CacheAttribute(string masterKey, Type returnType, params Type[] cachedTypes)
    {
        MasterKey = masterKey;
        CachedTypes = cachedTypes;
        ReturnType = returnType;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var serviceProvider = context.GetMemberServiceProvider();
        var memoryCache = serviceProvider?.GetRequiredService<IMemoryCache>();

        Debug.Assert(memoryCache is not null, "MemoryCache is null");

        var cacheKeyUnique = ComposeCacheKeyUnique(context);

        var valueFromCache = GetValueFromCache(memoryCache, cacheKeyUnique);

        if (valueFromCache is not null)
        {
            context.ReturnValue = Task.FromResult(valueFromCache);
            return;
        }
        
        await context.ProceedAsync();

        var returnValue = context.ReturnValue?.GetType()?
                            .GetProperty("Result")?
                            .GetValue(context.ReturnValue);
        var jsonValue = JsonSerializer.Serialize(returnValue);
        memoryCache.Set<string>(cacheKeyUnique, jsonValue);
    }

    #region Private Methods
    private object? GetValueFromCache(IMemoryCache memoryCache, string cacheKeyUnique)
    {
        if (memoryCache?.TryGetValue<string>(cacheKeyUnique, out var cachedValue) ?? false)
        {
            var valueObject = !string.IsNullOrEmpty(cachedValue)
                                            ? JsonSerializer.Deserialize(cachedValue, ReturnType)
                                            : null;
            return valueObject;
        }

        return null;
    }

    private string ComposeCacheKeyUnique(MethodAsyncAdviceContext context)
    {
        var cacheKeyUnique = MasterKey;

        foreach (var cachedType in CachedTypes)
        {
            cacheKeyUnique = string.Concat(cacheKeyUnique, "-", GetKeyValue(context.Arguments, cachedType));
        }

        return cacheKeyUnique;
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

    #endregion
}