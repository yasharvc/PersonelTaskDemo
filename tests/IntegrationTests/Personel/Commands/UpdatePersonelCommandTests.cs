using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Commands.UpdatePersonel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Personel.Commands
{
	public class UpdatePersonelCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();

			services.AddSingleton<IRequestHandler<UpdatePersonelCommand, DomainLayer.Entities.Personel>, UpdatePersonelCommandHandler>();
		}

		[Fact]
		public async void Should_Update_Entity_Successfully()
		{
			var cmd = new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			};

			var createHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var updateHandler = ServiceProvider.GetService<IRequestHandler<UpdatePersonelCommand, DomainLayer.Entities.Personel>>();

			var insertedPersonel = await createHandler.Handle(cmd, new CancellationToken());

			var result = await updateHandler.Handle(new UpdatePersonelCommand
			{
				Id = insertedPersonel.Id,
				FirstName = "Another",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			}, new CancellationToken());


			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(1, await ApplicationDbContext.Personels.CountAsync());
			Assert.Equal(result.FirstName, (await ApplicationDbContext.Personels.FirstAsync()).FirstName);
		}
	}
}