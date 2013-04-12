using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Domain
{
	public static class DependencyExtension
	{
		public static T ResolveDependency<T>(this T user)
		{
			var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
			return container.BuildUp(user);
		}
	}
}