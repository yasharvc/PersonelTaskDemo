using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Personel.Commands
{
	public class CreatePersonelCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequest<DomainLayer.Entities.Personel>, CreatePersonelCommand>();
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();
		}

		[Fact]
		public async void Should_Create_Entity_Successfully()
		{
			var cmd = new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			};

			var handler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();

			var result = await handler.Handle(cmd, new CancellationToken());

			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(1, await ApplicationDbContext.Personels.CountAsync());
			Assert.Equal(result.Id, (await ApplicationDbContext.Personels.FirstAsync()).Id);
		}
	}
}
