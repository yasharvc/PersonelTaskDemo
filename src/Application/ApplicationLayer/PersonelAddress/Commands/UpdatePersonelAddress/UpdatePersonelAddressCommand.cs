using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.UpdatePersonelAddress
{
	public class UpdatePersonelAddressCommand : IRequest<PersonelVm>
	{
		public string Id { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string No { get; set; }
		public string PersonelId { get; set; }

		public static implicit operator DomainLayer.Entities.PersonelAddress(UpdatePersonelAddressCommand c)
		{
			return new DomainLayer.Entities.PersonelAddress
			{
				Id = c.Id,
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
	public class UpdatePersonelAddressCommandHandler : IRequestHandler<UpdatePersonelAddressCommand, PersonelVm>
	{
		IApplicationDbContext AppDbContext { get; }
		ILogger Logger { get; }
		IValidator<UpdatePersonelAddressCommand> Validator { get; }
		IRequestHandler<GetPersonelByIdQuery, PersonelVm> PersonelFinder { get; }

		public UpdatePersonelAddressCommandHandler(IApplicationDbContext context,
			ILogger logger,
			IValidator<UpdatePersonelAddressCommand> validator,
			IRequestHandler<GetPersonelByIdQuery, PersonelVm> personelFinder)
		{
			AppDbContext = context;
			Logger = logger;
			Validator = validator;
			PersonelFinder = personelFinder;
		}
		public async Task<PersonelVm> Handle(UpdatePersonelAddressCommand request, CancellationToken cancellationToken)
		{
			var tranId = "";
			try
			{
				DomainLayer.Entities.PersonelAddress newPersonelAddress = request;
				await ValidateInput(request);

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

		private async System.Threading.Tasks.Task DoDbActions(
			DomainLayer.Entities.PersonelAddress personelAddress, 
			UpdatePersonelAddressCommand request, 
			CancellationToken cancellationToken)
		{
			var personel = await AppDbContext.Personels.FindAsync(request.PersonelId);
			await UpdateAddressInPersonel(personelAddress, personel, cancellationToken);
		}

		private async System.Threading.Tasks.Task UpdateAddressInPersonel(
			DomainLayer.Entities.PersonelAddress personelAddress, 
			DomainLayer.Entities.Personel personel,
			CancellationToken cancellationToken)
		{
			var address = personel.Addresses.FirstOrDefault(m => m.Id == personelAddress.Id);
			
			address.City = personelAddress.City;
			address.Country = personelAddress.Country;
			address.No = personelAddress.No;
			address.Street = personelAddress.Street;

			await AppDbContext.SaveChangesAsync(cancellationToken);
		}

		private async System.Threading.Tasks.Task ValidateInput(UpdatePersonelAddressCommand personelAddress)
		{
			var validationResult = await Validator.ValidateAsync(personelAddress);

			if (validationResult.Any())
				throw validationResult.First();
		}
	}
}
