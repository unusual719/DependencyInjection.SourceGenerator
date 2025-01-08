### SourceGenerator 服务自动注入

```c#
public interface IOneService
{ 
}

// IScopedDependency 接口标识和 ServiceInject 特性标识，以接口标识为准。
[ServiceInject(ServiceLifetime.Scoped, typeof(IOneService))]
public class OneService : IOneService, IScopedDependency
{
}

[Option(sectionKey: "Test2")]
public record Test2Options
{
    public int Age { get; set; }
}


// 调用
var services = new ServiceCollection(); 
services.AddDependencyInjection(configuration);
```



