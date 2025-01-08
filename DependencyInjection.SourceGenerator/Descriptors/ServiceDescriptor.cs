namespace DependencyInjection.SourceGenerator;

/// <summary> Service 服务描述 </summary>
internal sealed class ServiceDescriptor
{
    internal ServiceDescriptor(ServiceLifetime lifetime, TypeSymbol declaredType)
    {
        this.Lifetime = lifetime;
        this.DeclaredType = declaredType;
    }

    /// <summary> 生命周期 </summary>
    internal ServiceLifetime Lifetime { get; }

    /// <summary> TypeSymbol </summary>
    internal TypeSymbol DeclaredType { get; }

    /// <summary> 注入接口列表 </summary>
    internal HashSet<TypeSymbol> ServiceTypes { get; } = new HashSet<TypeSymbol>();
}