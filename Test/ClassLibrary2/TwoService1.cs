using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary2
{
    public interface ITwoService1 { }

    [ServiceInject(ServiceLifetime.Scoped, typeof(ITwoService1))]
    public class TwoService1 : ITwoService1, ISingletonDependency { }
}
