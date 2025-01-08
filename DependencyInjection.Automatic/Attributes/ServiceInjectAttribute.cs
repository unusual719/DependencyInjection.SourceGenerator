namespace Microsoft.Extensions.DependencyInjection;

/// <summary> 服务注入特性 <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ServiceInjectAttribute : Attribute
{
    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    /// <param name="serviceType1"> 注册的服务类型 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime" /> </param>
    /// <param name="serviceType1"> 注册的服务类型1 </param>
    /// <param name="serviceType2"> 注册的服务类型2 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1,
        Type serviceType2)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    /// <param name="serviceType1"> 注册的服务类型1 </param>
    /// <param name="serviceType2"> 注册的服务类型2 </param>
    /// <param name="serviceType3"> 注册的服务类型3 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1,
        Type serviceType2,
        Type serviceType3)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    /// <param name="serviceType1"> 注册的服务类型1 </param>
    /// <param name="serviceType2"> 注册的服务类型2 </param>
    /// <param name="serviceType3"> 注册的服务类型3 </param>
    /// <param name="serviceType4"> 注册的服务类型4 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1,
        Type serviceType2,
        Type serviceType3,
        Type serviceType4)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    /// <param name="serviceType1"> 注册的服务类型1 </param>
    /// <param name="serviceType2"> 注册的服务类型2 </param>
    /// <param name="serviceType3"> 注册的服务类型3 </param>
    /// <param name="serviceType4"> 注册的服务类型4 </param>
    /// <param name="serviceType5"> 注册的服务类型5 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1,
        Type serviceType2,
        Type serviceType3,
        Type serviceType4,
        Type serviceType5)
    {
    }

    /// <summary> 注册服务生命周期 </summary>
    /// <param name="lifetime"> 生命周期 </param>
    /// <param name="serviceType1"> 注册的服务类型1 </param>
    /// <param name="serviceType2"> 注册的服务类型2 </param>
    /// <param name="serviceType3"> 注册的服务类型3 </param>
    /// <param name="serviceType4"> 注册的服务类型4 </param>
    /// <param name="serviceType5"> 注册的服务类型5 </param>
    /// <param name="serviceType6"> 注册的服务类型6 </param>
    public ServiceInjectAttribute(ServiceLifetime lifetime,
        Type serviceType1,
        Type serviceType2,
        Type serviceType3,
        Type serviceType4,
        Type serviceType5,
        Type serviceType6)
    {
    }
}