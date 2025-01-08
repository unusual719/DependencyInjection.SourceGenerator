#pragma warning disable RS0030

namespace DependencyInjection.SourceGenerator;

// Copy from： https://github.com/dotnet/roslyn/blob/d2ff1d83e8fde6165531ad83f0e5b1ae95908289/src/Workspaces/SharedUtilitiesAndExtensions/Compiler/Core/Extensions/ISymbolExtensions.cs#L28-L73

// Copy from： https://github.com/dotnet/roslyn/blob/d2ff1d83e8fde6165531ad83f0e5b1ae95908289/src/Workspaces/SharedUtilitiesAndExtensions/Compiler/Core/Extensions/CompilationExtensions.cs#L11-L68
internal static class CompilationExtensions
{
    /// <summary> 按元数据名称获取类型 </summary>
    /// <param name="compilation"> </param>
    /// <param name="fullyQualifiedMetadataName"> </param>
    /// <returns> </returns>
    internal static INamedTypeSymbol? GetBestTypeByMetadataName(this Compilation compilation, string fullyQualifiedMetadataName)
    {
#if ROSLYN4_4_OR_GREATER
    INamedTypeSymbol? type = null;

    foreach (var currentType in compilation.GetTypesByMetadataName(fullyQualifiedMetadataName))
    {
        if (ReferenceEquals(currentType.ContainingAssembly, compilation.Assembly))
            return currentType;

        switch (currentType.GetResultantVisibility())
        {
            case SymbolVisibility.Public:
            case SymbolVisibility.Internal when currentType.ContainingAssembly.GivesAccessTo(compilation.Assembly):
                break;

            default:
                continue;
        }

        if (type != null)
            return null;

        type = currentType;
    }

    return type;
#else
        return compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
#endif
    }
}