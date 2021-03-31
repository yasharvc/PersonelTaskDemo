using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
	public abstract class DependencyInjection
	{
		IServiceCollection Services { get; } = new ServiceCollection();

		protected ServiceProvider ServiceProvider { get; }

		public DependencyInjection()
		{
			Services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
			AddAdditionalServices(Services);
			ServiceProvider = Services.BuildServiceProvider();
		}

		protected abstract void AddAdditionalServices(IServiceCollection services);
	}
}