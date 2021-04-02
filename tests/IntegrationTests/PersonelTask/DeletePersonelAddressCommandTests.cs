using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Common.Mappings;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Queries;
using ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress;
using ApplicationLayer.PersonelAddress.Commands.DeletePersonelAddress;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace IntegrationTests.PersonelTask
{
	public class DeletePersonelAddressCommandTests : DependencyInjection
	{
		public DeletePersonelAddressCommandTests() : base(true) { }
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			services.AddSingleton(configuration.CreateMapper());

			services.AddSingleton<IRequest<DomainLayer.Entities.Personel>, CreatePersonelCommand>();

			services.AddSingleton<IValidator<CreatePersonelAddressCommand>, CreatePersonelAddressCommandValidator>();
			services.AddSingleton<IValidator<DeletePersonelAddressCommand>, DeletePersonelAddressCommandValidator>();
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();
			services.AddSingleton<IRequestHandler<CreatePersonelAddressCommand, PersonelVm>, CreatePersonelAddressCommandHandler>();

			services.AddSingleton<IRequestHandler<GetPersonelByIdQuery, PersonelVm>, GetPersonelByIdQueryHandler>();

			services.AddSingleton<IRequestHandler<DeletePersonelAddressCommand, string>, DeletePersonelAddressCommandHandler>();
		}

		[Fact]
		public async void Should_Delete_PersonelAddress_Successfully()
		{
			var insertPersonelHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var insertedPersonel = await insertPersonelHandler.Handle(new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi"
			}, new CancellationToken());

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

			var deleteHandler = ServiceProvider.GetService<IRequestHandler<DeletePersonelAddressCommand, string>>();

			await deleteHandler.Handle(new DeletePersonelAddressCommand
			{
				AddressId = insertedPersonel.Addresses.First().Id,
				PersonelId = insertedPersonel.Id
			}, new CancellationToken());

			Assert.Equal(0, ApplicationDbContext.PersonelAddresses.Count());
			Assert.Equal(1, ApplicationDbContext.Personels.Count());
			Assert.Equal(0, ApplicationDbContext.Personels.First().Addresses.Count);
		}
	}
}
