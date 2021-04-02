using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress
{
	public class CreatePersonelAddressCommand : IRequest<PersonelVm>
	{
		public string Id { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string No { get; set; }
		public string PersonelId { get; set; }

		public static implicit operator DomainLayer.Entities.PersonelAddress(CreatePersonelAddressCommand c)
		{
			return new DomainLayer.Entities.PersonelAddress
			{
				City = c.City,
				Country = c.Country,
				No = c.No,
				Street = c.Street,
				Personel = new DomainLayer.Entities.Personel
				{
					Id = c.PersonelId
				}
			};
		}
	}
	public class CreatePersonelAddressCommandHandler : IRequestHandler<CreatePersonelAddressCommand, PersonelVm>
	{
		IApplicationDbContext AppDbContext { get; }
		ILogger Logger { get; }
		IValidator<DomainLayer.Entities.PersonelAddress> Validator { get; }
		IRequestHandler<GetPersonelByIdQuery, PersonelVm> PersonelFinder { get; }

		public CreatePersonelAddressCommandHandler(IApplicationDbContext context,
			ILogger logger,
			IValidator<DomainLayer.Entities.PersonelAddress> validator,
			IRequestHandler<GetPersonelByIdQuery,PersonelVm> personelFinder)
		{
			AppDbContext = context;
			Logger = logger;
			Validator = validator;
			PersonelFinder = personelFinder;
		}
		public async Task<PersonelVm> Handle(CreatePersonelAddressCommand request, CancellationToken cancellationToken)
		{
			var tranId = "";
			try
			{
				DomainLayer.Entities.PersonelAddress newPersonelAddress = request;
				await ValidateInput(newPersonelAddress);

				tranId = AppDbContext.BeginTransaction();
				await DoDbActions(newPersonelAddress, request, cancellationToken);

				await AppDbContext.CommitTransactionAsync(tranId);
				return await PersonelFinder.Handle(new GetPersonelByIdQuery { PersonelId = request.PersonelId }, cancellationToken);
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				await AppDbContext.RollbackTransactionAsync(tranId);
				throw;
			}
		}

		private async System.Threading.Tasks.Task DoDbActions(DomainLayer.Entities.PersonelAddress newPersonelAddress, CreatePersonelAddressCommand request, CancellationToken cancellationToken)
		{
			var personel = await AppDbContext.Personels.FindAsync(request.PersonelId);
			await AddAddressIntoPersonel(newPersonelAddress, personel, cancellationToken);
		}

		private async System.Threading.Tasks.Task AddAddressIntoPersonel(DomainLayer.Entities.PersonelAddress newPersonelAddress, DomainLayer.Entities.Personel personel, CancellationToken cancellationToken)
		{
			personel.Addresses.Add(newPersonelAddress);
			await AppDbContext.SaveChangesAsync(cancellationToken);
		}

		private async System.Threading.Tasks.Task ValidateInput(DomainLayer.Entities.PersonelAddress newPersonelAddress)
		{
			var validationResult = await Validator.ValidateAsync(newPersonelAddress);

			if (validationResult.Any())
				throw validationResult.First();
		}
	}
}
