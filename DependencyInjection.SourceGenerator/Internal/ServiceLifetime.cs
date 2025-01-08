namespace DependencyInjection.SourceGenerator;

/// <summary> 生命周期 </summary>
internal enum ServiceLifetime
{
    /// <summary> 单例生命周期 </summary>
    [Description("单例")]
    Singleton,

    /// <summary> 作用域生命周期 </summary>
    [Description("作用域")]
    Scoped,

    /// <summary> 瞬时生命周期 </summary>
    [Description("瞬时")]
    Transient
}