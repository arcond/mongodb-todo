using Domain;
using Domain.Aggregates;
using Domain.Data;
using Domain.Factory;
using Domain.Repository;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Web;
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

			ServiceLocator.SetLocatorProvider(() => {
				return new UnityServiceLocator((IUnityContainer)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUnityContainer)));
			});
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// e.g. container.RegisterType<ITestService, TestService>();
			container
				.RegisterType<IUserRepository, UserRepository>()
				.RegisterType<ITaskRepository, TaskRepository>()
				.RegisterType<IUserFactory, UserFactory>()
				.RegisterType<ITaskFactory, TaskFactory>()
				.RegisterType<IContext, Context>()
				.RegisterType<IUserDependency, UserDependency>()
			;

			//var test = container.Resolve<UserDependency>();
			//var test2 = test.TaskFactory;
			//container.RegisterInstance<UserDependency>(container.Resolve<UserDependency>());

			return container;
		}
	}

	public class PerHttpRequestLifetime :LifetimeManager
	{
		private readonly Guid _key = Guid.NewGuid();

		public override object GetValue()
		{
			return HttpContext.Current.Items[_key];
		}

		public override void SetValue(object newValue)
		{
			HttpContext.Current.Items[_key] = newValue;
		}

		public override void RemoveValue()
		{
			var obj = GetValue();
			HttpContext.Current.Items.Remove(obj);
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