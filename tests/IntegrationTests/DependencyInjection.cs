using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
	public abstract class DependencyInjection
	{
		IServiceCollection Services { get; } = new ServiceCollection();
		ApplicationDbContext ApplicationDbContext { get; }

		protected ServiceProvider ServiceProvider { get; }

		public DependencyInjection()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: "Test")
						.Options;
			ApplicationDbContext = new ApplicationDbContext(options);
			Services.AddSingleton<IApplicationDbContext>(sp =>
			{
				return ApplicationDbContext;
			});
			AddAdditionalServices(Services);
			ServiceProvider = Services.BuildServiceProvider();
		}

		protected abstract void AddAdditionalServices(IServiceCollection services);
	}
}