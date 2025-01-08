namespace DependencyInjection.SourceGenerator;

/// <summary> Option 选项配置描述 </summary>
internal sealed class OptionDescriptor
{
    internal OptionDescriptor(TypeSymbol declaredType, string section)
    {
        this.DeclaredType = declaredType;
        this.SectionKey = section;
    }

    /// <summary> SectionKey </summary>
    internal string SectionKey { get; }

    /// <summary> DeclaredType </summary>
    internal TypeSymbol DeclaredType { get; }

    internal string ToString(string service, string configuration)
    {
        configuration = @$"{configuration}.GetSection(""{this.SectionKey}"")";
        return $"{service}.AddOptions<{this.DeclaredType}>().Bind({configuration}).ValidateDataAnnotations();";
    }
}