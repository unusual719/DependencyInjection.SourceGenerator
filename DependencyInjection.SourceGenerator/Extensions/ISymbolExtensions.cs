namespace DependencyInjection.SourceGenerator;

internal enum SymbolVisibility
{
    Public,
    Internal,
    Private,
}

internal static class ISymbolExtensions
{
    internal static SymbolVisibility GetResultantVisibility(this ISymbol symbol)
    {
        while (true)
        {
            // Start by assuming it's visible.
            var visibility = SymbolVisibility.Public;
            switch (symbol.Kind)
            {
                case SymbolKind.Alias:
                    // Aliases are uber private. They're only visible in the same file that they were declared in.
                    return SymbolVisibility.Private;

                case SymbolKind.Parameter:
                    // Parameters are only as visible as their containing symbol
                    symbol = symbol.ContainingSymbol;
                    continue;

                case SymbolKind.TypeParameter:
                    // Type Parameters are private.
                    return SymbolVisibility.Private;
            }

            while (symbol != null && symbol.Kind != SymbolKind.Namespace)
            {
                switch (symbol.DeclaredAccessibility)
                {
                    // If we see anything private, then the symbol is private.
                    case Accessibility.NotApplicable:
                    case Accessibility.Private:
                        return SymbolVisibility.Private;

                    // If we see anything internal, then knock it down from public to internal.
                    case Accessibility.Internal:
                    case Accessibility.ProtectedAndInternal:
                        visibility = SymbolVisibility.Internal;
                        break;
                        // For anything else (Public, Protected, ProtectedOrInternal), the symbol stays at the level we've gotten so far.
                }
                symbol = symbol.ContainingSymbol;
            }

            return visibility;
        }
    }

    internal static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol? attribute)
    {
        return attribute != null && symbol.GetAttributes().Any(attr => attr.AttributeClass?.Equals(attribute, SymbolEqualityComparer.Default) == true);
    }
}