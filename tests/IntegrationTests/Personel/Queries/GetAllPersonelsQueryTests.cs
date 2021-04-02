using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Common.Mappings;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Queries;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace IntegrationTests.Personel.Queries
{
	public class GetAllPersonelsQueryTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			services.AddSingleton<IRequestHandler<GetAllPersonelsQuery, PersonelsVm>, GetAllPersonelsQueryHandler>();
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();

			services.AddSingleton(configuration.CreateMapper());
		}

		[Fact]
		public async void Should_Read_Inserted_Personel()
		{
			var cmd = new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			};

			var insertHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var readHandler = ServiceProvider.GetService<IRequestHandler<GetAllPersonelsQuery, PersonelsVm>>();

			var insertedPersonel = await insertHandler.Handle(cmd, new CancellationToken());

			var res = await readHandler.Handle(new GetAllPersonelsQuery(), new CancellationToken());

			Assert.NotEmpty(res.Personels);
			Assert.Equal(1, res.Personels.Count);
			Assert.Equal(insertedPersonel.Id, res.Personels.First().Id);
		}
	}
}
