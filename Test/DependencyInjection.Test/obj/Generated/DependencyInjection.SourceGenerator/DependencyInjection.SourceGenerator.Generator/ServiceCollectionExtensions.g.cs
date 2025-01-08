﻿// <auto-generated>
//     由 DependencyInjection.SourceGenerator 自动生成.
//     Assembly：DependencyInjection.Test.dll
// </auto-generated>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// ServiceCollectionExtensions_G.cs 服务自动注入到IOC扩展类
/// </summary>
public static class ServiceCollectionExtensions_G
{
    /// <summary>
    /// DependencyInjection.Test.dll 程序集中的所有标记类型添加到指定程序集的给定服务集合中
    /// </summary>
    /// <param name="services"> <see cref="IServiceCollection" /> </param> 
    /// <returns> <see cref="IServiceCollection" /> </returns>
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddDependencyInjectionTest(this Microsoft.Extensions.DependencyInjection.IServiceCollection services
        , Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        // Services
        services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(global::DependencyInjection.Test.IOneService), typeof(global::DependencyInjection.Test.OneService), ServiceLifetime.Singleton));
        services.TryAddEnumerable(ServiceDescriptor.Describe(typeof(global::DependencyInjection.Test.ITwoService1), typeof(global::DependencyInjection.Test.TwoService1), ServiceLifetime.Singleton));

        // Options
        services.AddOptions<global::DependencyInjection.Test.Test2Options>().Bind(configuration.GetSection("Test2")).ValidateDataAnnotations();

        return services;
    }
}