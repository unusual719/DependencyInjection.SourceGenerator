namespace DependencyInjection.SourceGenerator;

internal sealed class TypeSymbol : IEquatable<TypeSymbol>
{
    private readonly ITypeSymbol typeSymbol;

    public TypeSymbol(ITypeSymbol typeSymbol) => this.typeSymbol = typeSymbol;

    public bool Equals(TypeSymbol? other) => other != null && SymbolEqualityComparer.Default.Equals(typeSymbol, other.typeSymbol);

    public override bool Equals(object? obj) => Equals(obj as TypeSymbol);

    public override int GetHashCode() => SymbolEqualityComparer.Default.GetHashCode(typeSymbol);

    public override string ToString() => typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
}