
using Domain;
using Domain.Aggregates;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using MongoData;
using MongoData.Data;
using MongoData.Factory;
using MongoData.Repository;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Unity.Mvc3;

namespace mongo_todo
{
	public static class Bootstrapper
	{
		public static void Initialise()
		{
			var container = BuildUnityContainer();

			GlobalConfiguration.Configuration.DependencyResolver =
				new WebApiUnityDependencyResolver(new UnityDependencyResolver(container));

			ServiceLocator.SetLocatorProvider(
				() => new UnityServiceLocator(
					(IUnityContainer)
						GlobalConfiguration.Configuration.DependencyResolver.GetService(
							typeof(IUnityContainer))));
		}

		private static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			container
				.RegisterType<IUserRepository, MongoDbUserRepository>()
				.RegisterType<ITodoRepository, MongoDbTodoRepository>()
				.RegisterType<IUserFactory, MongoDbUserFactory>()
				.RegisterType<ITodoFactory, MongoDbTodoFactory>()
				.RegisterType<IContext, Context>()
				.RegisterType<IUserDependency, UserDependency>()
				;

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

	internal sealed class WebApiUnityDependencyResolver :IDependencyResolver
	{
		public WebApiUnityDependencyResolver(UnityDependencyResolver resolver)
		{
			Resolver = resolver;
		}

		protected UnityDependencyResolver Resolver { get; private set; }

		public IDependencyScope BeginScope()
		{
			return this;
		}

		public object GetService(Type serviceType)
		{
			return Resolver.GetService(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Resolver.GetServices(serviceType);
		}

		public void Dispose()
		{
		}
	}
}