namespace DependencyInjection.SourceGenerator;

internal class DependencyInjectionDescriptor
{
    internal ITypeSymbol DeclaredType { get; set; }
    internal ServiceDescriptor ServiceDescriptor { get; set; }
    internal OptionDescriptor OptionDescriptor { get; set; }
    internal Diagnostic? Diagnostic { get; set; }
}