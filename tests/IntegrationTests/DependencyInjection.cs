using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
	public abstract class DependencyInjection
	{
		IServiceCollection Services { get; } = new ServiceCollection();
		protected IApplicationDbContext ApplicationDbContext { get; }

		protected ServiceProvider ServiceProvider { get; }

		public DependencyInjection()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseInMemoryDatabase(databaseName: "Test")
						.Options;
			ApplicationDbContext = new ApplicationDbContext(options);

			Services.AddSingleton<ILogger, NullLogger>();

			Services.AddSingleton(sp =>
			{
				return ApplicationDbContext;
			});
			AddAdditionalServices(Services);
			ServiceProvider = Services.BuildServiceProvider();
		}

		protected abstract void AddAdditionalServices(IServiceCollection services);
	}
}