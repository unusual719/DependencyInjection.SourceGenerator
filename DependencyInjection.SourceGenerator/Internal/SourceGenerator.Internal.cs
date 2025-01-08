namespace DependencyInjection.SourceGenerator;

/// <summary> 内部调用类 </summary>
internal partial class SourceGenerator
{
    /// <summary> 需要排除的接口列表 </summary>
    internal static readonly string[] IgnoredInterfaces =
    [
        "System.IDisposable",
        "System.IAsyncDisposable"
    ];

    internal static readonly string ServiceAttributeTypeName = "Microsoft.Extensions.DependencyInjection.ServiceInjectAttribute";
    internal static readonly string ScopedTypeName = $"Microsoft.Extensions.DependencyInjection.IScopedDependency";
    internal static readonly string SingletonTypeName = $"Microsoft.Extensions.DependencyInjection.ISingletonDependency";
    internal static readonly string TransientTypeName = $"Microsoft.Extensions.DependencyInjection.ITransientDependency";

    internal static readonly List<string> LifeCycleTypeNames = new List<string>()
    {
        ScopedTypeName,
        SingletonTypeName,
        TransientTypeName
    };

    internal static readonly string OptionAttributeTypeName = "Microsoft.Extensions.DependencyInjection.OptionAttribute";

    internal static string GetSanitizedAssemblyName(Compilation compilation)
    {
        var assemblyName = compilation.AssemblyName ?? string.Empty;
        return new string(assemblyName.Where(IsAllowChar).ToArray());

        static bool IsAllowChar(char c) => '0' <= c && c <= '9' || 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z';
    }

    internal static Diagnostic CreateDiagnostic(string id, string messageFormat, Location? location)
    {
        return Diagnostic.Create(
            new DiagnosticDescriptor(
                id: "AUTODI0001",
                title: "服务注入生命周期标识",
                messageFormat: $"{messageFormat} .",
                category: "DependencyInjection",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true
            ),
            location
        );
    }

    internal static Dictionary<string, INamedTypeSymbol> GetTypeByMetadataNames(Compilation compilation)
    {
        Dictionary<string, INamedTypeSymbol> namedTypeSymbols = new();
        foreach (var lifeCycleTypeName in LifeCycleTypeNames)
        {
            var namedTypeSymbol = compilation.GetBestTypeByMetadataName(lifeCycleTypeName);
            if (namedTypeSymbol != null)
                namedTypeSymbols.Add(lifeCycleTypeName, namedTypeSymbol);
        }

        return namedTypeSymbols;
    }
}