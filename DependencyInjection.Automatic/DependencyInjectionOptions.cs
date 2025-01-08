namespace Microsoft.Extensions.DependencyInjection;

/// <summary> DependencyInjection Options </summary>
public class DependencyInjectionOptions
{
    /// <summary> 需要忽略的程序集 </summary>
    public string[] IgnoredAssemblies { get; set; }
}