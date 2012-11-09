using Domain;
using Domain.Factory;
using Domain.Repository;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc3;

namespace mongo_todo
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

			container.RegisterType<IUserRepository, DummyUserRepository>();
			container.RegisterType<ITaskRepository, DummyTaskRepository>();
			container.RegisterType<IUserFactory, UserFactory>();
			container.RegisterType<ITaskFactory, TaskFactory>();

            return container;
        }
    }
}