using DomainLayer.Entities;
using InfrastructureLayer.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unitests.Infrastructure
{
	public class PersistanceTests
	{
		[Fact]
		public async void With_InMemorySetting_DbContext_Should_Connect_To_InMemory()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "Test")
			.Options;

			using var context = new ApplicationDbContext(options);
			var p = new Personel
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			};

			context.Personels.Add(p);
			context.SaveChanges();
			Assert.Equal(1, await context.Personels.CountAsync());

		}
	}
}
