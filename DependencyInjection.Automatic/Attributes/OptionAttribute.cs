namespace Microsoft.Extensions.DependencyInjection;

/// <summary> 选项绑定配置 </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class OptionAttribute : Attribute
{
    private OptionAttribute()
    {
    }

    /// <summary> 标记该选项类型绑定到 <see cref="Microsoft.Extensions.Configuration.IConfiguration" /> 的指定 SectionKey </summary>
    /// <param name="sectionKey"> 配置的 sectionKey 名 </param>
    public OptionAttribute(string sectionKey)
    {
        if (string.IsNullOrWhiteSpace(sectionKey))
            throw new ArgumentNullException(nameof(sectionKey), "Section key cannot be null or whitespace.");
    }
}