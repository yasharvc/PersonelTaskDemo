using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Common.Mappings;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Queries;
using ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress;
using ApplicationLayer.PersonelAddress.Commands.UpdatePersonelAddress;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace IntegrationTests.PersonelAddress
{
	public class UpdatePersonelAddressCommandTests : DependencyInjection
	{
		public UpdatePersonelAddressCommandTests() : base(true) { }
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			services.AddSingleton(configuration.CreateMapper());

			services.AddSingleton<IRequest<DomainLayer.Entities.Personel>, CreatePersonelCommand>();

			services.AddSingleton<IValidator<CreatePersonelAddressCommand>, CreatePersonelAddressCommandValidator>();
			services.AddSingleton<IValidator<UpdatePersonelAddressCommand>, UpdatePersonelAddressCommandValidator>();
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();

			services.AddSingleton<IRequestHandler<GetPersonelByIdQuery, PersonelVm>, GetPersonelByIdQueryHandler>();

			services.AddSingleton<IRequestHandler<CreatePersonelAddressCommand, PersonelVm>, CreatePersonelAddressCommandHandler>();
			services.AddSingleton<IRequestHandler<UpdatePersonelAddressCommand, PersonelVm>, UpdatePersonelAddressCommandHandler>();
		}

		[Fact]
		public async void Should_Create_PersonelAddress_Successfully()
		{
			var insertPersonelHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var insertedPersonel = await insertPersonelHandler.Handle(new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi"
			},new CancellationToken());

			var insertPersonelAddrHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelAddressCommand, PersonelVm>>();
			var result = await insertPersonelAddrHandler.Handle(new CreatePersonelAddressCommand
			{
				City = "Tabriz",
				Country = "Iran",
				No = "1",
				PersonelId = insertedPersonel.Id,
				Street = "Xyz"
			}, new CancellationToken());

			var personelInDb = ApplicationDbContext.Personels.FirstOrDefault();

			Assert.Equal(1, ApplicationDbContext.Personels.Count());
			Assert.Equal(1, ApplicationDbContext.PersonelAddresses.Count());
			Assert.Equal(1, personelInDb.Addresses.Count);
		}
		[Fact]
		public async void Should_Update_PersonelAddress_Successfully()
		{
			var insertPersonelHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var insertedPersonel = await insertPersonelHandler.Handle(new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi"
			},new CancellationToken());

			var insertPersonelAddrHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelAddressCommand, PersonelVm>>();
			var insertedPersonelAddr = await insertPersonelAddrHandler.Handle(new CreatePersonelAddressCommand
			{
				City = "Tabriz",
				Country = "Iran",
				No = "1",
				PersonelId = insertedPersonel.Id,
				Street = "Xyz"
			}, new CancellationToken());

			var updatePersonelAddrHandler = ServiceProvider.GetService<IRequestHandler<UpdatePersonelAddressCommand, PersonelVm>>();

			var res = await updatePersonelAddrHandler.Handle(new UpdatePersonelAddressCommand
			{
				City = "Istanbul",
				Country = "Turkey",
				No = "2",
				Street = "Sisli",
				PersonelId = insertedPersonel.Id,
				Id = insertedPersonel.Addresses.First().Id
			}, new CancellationToken());

			var personelInDb = ApplicationDbContext.Personels.FirstOrDefault();

			Assert.Equal(1, ApplicationDbContext.Personels.Count());
			Assert.Equal(1, ApplicationDbContext.PersonelAddresses.Count());
			Assert.Equal(1, personelInDb.Addresses.Count);
			Assert.Equal("Istanbul", personelInDb.Addresses.First().City);
		}
	}
}
