using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

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
						.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
						.Options;
			ApplicationDbContext = new ApplicationDbContext(options, new NullLogger());

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