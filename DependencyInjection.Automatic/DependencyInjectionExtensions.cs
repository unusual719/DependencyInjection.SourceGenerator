namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    private static readonly string[] IgnoredAssemblies = new string[]
    {
        "dotnet-",
        "Microsoft.",
        "System",
        "Windows",
        "mscorlib",
        "netstandard",
        "Newtonsoft",
        "DependencyInjection.SourceGenerator",
        "DependencyInjection.Automatic"
    };

    private static string GetSanitizedAssemblyName(string assemblyName)
    {
        return new string(assemblyName.Where(IsAllowChar).ToArray());

        static bool IsAllowChar(char c) =>
            '0' <= c && c <= '9' || 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z';
    }

    /// <summary> SourceGenerator 自动注入 </summary>
    /// <param name="services"> </param>
    /// <param name="configuration"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration? configuration = null, DependencyInjectionOptions? options = null)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        configuration ??= services.BuildServiceProvider().GetService<IConfiguration>();
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        // 需忽略的程序集
        var ignoredAssemblies = IgnoredAssemblies.ToList();
        if (options != null && options.IgnoredAssemblies != null)
        {
            ignoredAssemblies.AddRange(options.IgnoredAssemblies);
        }

        var assemblies = DependencyContext
            .Default?.GetDefaultAssemblyNames()
            .Where(c => c.Name != null && !ignoredAssemblies.Any(x => c.Name.StartsWith(x)))
            .Select(Assembly.Load)
            .ToArray();

        var types = assemblies.SelectMany(m => m.GetTypes()).Where(c => c.Name == "ServiceCollectionExtensions_G").ToArray();

        foreach (var type in types)
        {
            var assemblyName = GetSanitizedAssemblyName(type.Assembly.GetName().Name);
            var method = type.GetMethods().FirstOrDefault(c => c.Name.StartsWith($"Add{assemblyName}"));
            method?.Invoke(null, new object[] { services, configuration });
        }

        return services;
    }

    /// <summary> SourceGenerator 自动注入 </summary>
    /// <param name="services"> </param>
    /// <param name="configuration"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration, Action<DependencyInjectionOptions> configureOptions)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        var options = new DependencyInjectionOptions();
        configureOptions.Invoke(options);

        return AddDependencyInjection(services, configuration, options);
    }

    /// <summary> SourceGenerator 自动注入 </summary>
    /// <param name="services"> </param>
    /// <param name="configuration"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, Action<DependencyInjectionOptions> configureOptions)
    {
        if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        var options = new DependencyInjectionOptions();
        configureOptions.Invoke(options);

        return AddDependencyInjection(services, null, options);
    }
}