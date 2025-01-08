using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary1
{
    public interface IOneService
    { }

    public interface IOneService1<T>
    {
        int vv(int v);
    }

    [ServiceInject(ServiceLifetime.Scoped, typeof(IOneService))]
    public class OneService : IOneService
    {
    }

    [ServiceInject(ServiceLifetime.Scoped, typeof(IOneService1<int>))]
    public class OneService1 : IOneService1<int>, IScopedDependency
    {
        public int vv(int v)
        {
            return v;
        }
    }
}