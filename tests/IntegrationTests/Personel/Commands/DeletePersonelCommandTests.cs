using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Commands.DeletePersonel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Personel.Commands
{
	public class DeletePersonelCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();

			services.AddSingleton<IRequestHandler<DeletePersonelCommand, DomainLayer.Entities.Personel>, DeletePersonelCommandHandler>();
		}

		[Fact]
		public async void Should_Delete_Entity_Successfully()
		{
			var cmd = new CreatePersonelCommand
			{
				FirstName = "Yashar",
				DateOfBirth = DateTime.Now.AddDays(1),
				LastName = "Aliabbasi"
			};

			var createHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var deleteHandler = ServiceProvider.GetService<IRequestHandler<DeletePersonelCommand, DomainLayer.Entities.Personel>>();

			var insertedPersonel = await createHandler.Handle(cmd, new CancellationToken());

			var result = await deleteHandler.Handle(new DeletePersonelCommand
			{
				Id = insertedPersonel.Id
			}, new CancellationToken());


			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(0, await ApplicationDbContext.Personels.CountAsync());
		}
	}
}