using Domain;
using Domain.Data;
using Domain.Factory;
using Domain.Repository;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.Mvc3;

namespace mongo_todo
{
	public static class Bootstrapper
	{
		public static void Initialise()
		{
			var container = BuildUnityContainer();

			GlobalConfiguration.Configuration.DependencyResolver = new WebApiUnityDependencyResolver(new UnityDependencyResolver(container));
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// e.g. container.RegisterType<ITestService, TestService>();
			//container.RegisterType<IUserRepository, DummyUserRepository>();
			//container.RegisterType<ITaskRepository, DummyTaskRepository>();
			container.RegisterType<IUserRepository, UserRepository>();
			container.RegisterType<ITaskRepository, TaskRepository>();
			container.RegisterType<IUserFactory, UserFactory>();
			container.RegisterType<ITaskFactory, TaskFactory>();
			container.RegisterType<IContext, Context>();

			return container;
		}
	}

	internal sealed class WebApiUnityDependencyResolver :System.Web.Http.Dependencies.IDependencyResolver
	{
		protected UnityDependencyResolver Resolver { get; private set; }

		public WebApiUnityDependencyResolver(UnityDependencyResolver resolver)
		{
			Resolver = resolver;
		}

		public System.Web.Http.Dependencies.IDependencyScope BeginScope()
		{
			return this;
		}

		public object GetService(System.Type serviceType)
		{
			return Resolver.GetService(serviceType);
		}

		public System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
		{
			return Resolver.GetServices(serviceType);
		}

		public void Dispose()
		{
		}
	}
}