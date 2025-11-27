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

    private IMemoryCache? _memoryCache;

    public CacheAttribute(string masterKey, Type returnType, params Type[] cachedTypes)
    {
        MasterKey = masterKey;
        CachedTypes = cachedTypes;
        ReturnType = returnType;
    }

    public async Task Advise(MethodAsyncAdviceContext context)
    {
        var serviceProvider = context.GetMemberServiceProvider();
        _memoryCache = serviceProvider?.GetRequiredService<IMemoryCache>();

        Debug.Assert(_memoryCache is not null, "MemoryCache is null");

        var cacheKeyUnique = ComposeCacheKeyUnique(context);

        var valueFromCache = GetValueFromCache(cacheKeyUnique);

        if (valueFromCache is not null)
        {
            context.ReturnValue = Task.FromResult(valueFromCache);
            return;
        }
        
        await context.ProceedAsync();

        PersistToCache(context.ReturnValue, cacheKeyUnique);
    }

    #region Private Methods

    private void PersistToCache(object? returnValue, string cacheKeyUnique)
    {
        var value = returnValue?.GetType()?
                    .GetProperty("Result")?
                    .GetValue(returnValue);
        var jsonValue = JsonSerializer.Serialize(value);

        _memoryCache?.Set<string>(cacheKeyUnique, jsonValue, TimeSpan.FromHours(1));
    }

    private object? GetValueFromCache(string cacheKeyUnique)
    {
        if (_memoryCache?.TryGetValue<string>(cacheKeyUnique, out var cachedValue) ?? false)
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