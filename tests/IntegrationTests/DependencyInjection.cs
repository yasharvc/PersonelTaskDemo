using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
	public abstract class DependencyInjection
	{
		IServiceCollection Services { get; } = new ServiceCollection();

		protected ServiceProvider ServiceProvider { get; }

		public DependencyInjection()
		{
			Services.AddSingleton<IApplicationDbContext>(sp=> {
				var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: "Test")
						.Options;
				var context = new ApplicationDbContext(options);
				return context;
			});
			AddAdditionalServices(Services);
			ServiceProvider = Services.BuildServiceProvider();
		}

		protected abstract void AddAdditionalServices(IServiceCollection services);
	}
}