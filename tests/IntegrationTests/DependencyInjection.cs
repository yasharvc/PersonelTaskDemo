using ApplicationLayer.Common.Interfaces;
using InfrastructureLayer.Persistance;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IntegrationTests
{
	public abstract class DependencyInjection
	{
		IServiceCollection Services { get; } = new ServiceCollection();
		protected ApplicationDbContext ApplicationDbContext { get; }

		protected ServiceProvider ServiceProvider { get; }

		public DependencyInjection(bool useSqlite = false)
		{
			if (!useSqlite)
			{
				var options = new DbContextOptionsBuilder<ApplicationDbContext>()
							.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
							.Options;
				ApplicationDbContext = new ApplicationDbContext(options, new NullLogger());
			}
			else
			{
				var connection = new SqliteConnection("DataSource=:memory:");
				connection.Open();
				var options = new DbContextOptionsBuilder<ApplicationDbContext>()
						.UseSqlite(connection)
						.Options;
				ApplicationDbContext = new ApplicationDbContext(options, new NullLogger());
				ApplicationDbContext.Database.EnsureCreated();
			}

			Services.AddSingleton<ILogger, NullLogger>();

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