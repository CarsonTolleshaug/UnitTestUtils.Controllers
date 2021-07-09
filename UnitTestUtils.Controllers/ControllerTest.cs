using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace UnitTestUtils.Controllers
{
    public class ControllerTest<T> where T : ControllerBase
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly HttpContextMock _fakeHttpContext;

        public ControllerTest(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public ControllerTest(Func<T> controllerFactory) : this(HandleControllerFactoryInstantiation(controllerFactory)) { }
        
        public ControllerTest(T controller) : this(() => controller) { }

        public ControllerTest() : this(HandleParameterlessInstantiation()) { }
        
        private static IServiceCollection HandleParameterlessInstantiation()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<T>();
            return serviceCollection;
        }

        private static IServiceCollection HandleControllerFactoryInstantiation(Func<T> controllerFactory)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(svc => controllerFactory());
            return serviceCollection;
        }

        public static ControllerTest<TController> CreateFromStartup<TController, TStartup>(IConfiguration configuration = null) where TController : ControllerBase
        {
            TStartup startup = (TStartup)Activator.CreateInstance(typeof(TStartup), configuration);
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<TController>();

            System.Reflection.MethodInfo configureServicesMethod = startup.GetType().GetMethod("ConfigureServices");
            configureServicesMethod.Invoke(startup, new object[] { services });

            return new ControllerTest<TController>(services);
        }

        public T CreateController()
        {
            T controller = _serviceCollection.BuildServiceProvider().GetService<T>();

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = _fakeHttpContext;

            return controller;
        }
    }
}
