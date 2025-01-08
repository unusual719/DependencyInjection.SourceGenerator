using ClassLibrary1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace DependencyInjection.Test
{
    public interface IOneService
    { }

    public interface ITwoService1
    { }

    public interface ITwoService2
    { }

    [ServiceInject(ServiceLifetime.Scoped, typeof(IOneService))]
    public class OneService : IOneService, ISingletonDependency
    { }

    public class TwoService1 : ITwoService1, ISingletonDependency
    { }

    [Option("Test2")]
    public record Test2Options
    {
        public int Age { get; set; }
    }

    public class ServiceUnitTest
    {
        [Fact]
        public void OneServiceTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new[]
                    {
                        new KeyValuePair<string, string?>("Test1Options:Age", "10"),
                        new KeyValuePair<string, string?>("Test2:Age", "20")
                    }
                )
                .Build();

            var services = new ServiceCollection();

            services.AddDependencyInjection(configuration);

            //services.AddDependencyInjectionTestOptions(configuration);
            //services.AddClassLibrary2(configuration);
            //services.AddClassLibrary1Options(configuration);
            //services.AddClassLibrary1();

            var root = services.BuildServiceProvider();

            //var service1 = root.GetService<IConfiguration>();
            var service2 = root.GetService<IOneService>();

            var service3 = root.GetService<ClassLibrary2.ITwoService1>();
            var service4 = root.GetService<ClassLibrary1.IOneService>();
            var cc11 = root.GetService<IOneService1<int>>();
            var ccccc = cc11.vv(12);

            var cc = root.GetRequiredService<IOptions<Test2Options>>().Value;

            //Assert.Null(service1);
            //Assert.NotNull(service2);
        }
    }
}