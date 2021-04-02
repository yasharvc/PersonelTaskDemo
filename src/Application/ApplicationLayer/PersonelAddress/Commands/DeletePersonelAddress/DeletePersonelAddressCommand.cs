using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.DeletePersonelAddress
{
	public class DeletePersonelAddressCommand : IRequest<string>
	{
		public string AddressId { get; set; }
		public string PersonelId { get; set; }

		public static implicit operator string(DeletePersonelAddressCommand c)
		{
			return c.AddressId;
		}
	}
	public class DeletePersonelAddressCommandHandler : IRequestHandler<DeletePersonelAddressCommand, string>
	{
		IApplicationDbContext AppDbContext { get; }
		ILogger Logger { get; }
		IValidator<DeletePersonelAddressCommand> Validator { get; }
		IRequestHandler<GetPersonelByIdQuery, PersonelVm> PersonelFinder { get; }

		public DeletePersonelAddressCommandHandler(IApplicationDbContext context,
			ILogger logger,
			IValidator<DeletePersonelAddressCommand> validator,
			IRequestHandler<GetPersonelByIdQuery, PersonelVm> personelFinder)
		{
			AppDbContext = context;
			Logger = logger;
			Validator = validator;
			PersonelFinder = personelFinder;
		}
		public async Task<string> Handle(DeletePersonelAddressCommand request, CancellationToken cancellationToken)
		{
			var tranId = "";
			try
			{
				await ValidateInput(request);

				tranId = AppDbContext.BeginTransaction();

				await DoDbActions(request, cancellationToken);

				await AppDbContext.CommitTransactionAsync(tranId);
				return request.PersonelId;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				await AppDbContext.RollbackTransactionAsync(tranId);
				throw;
			}
		}

		private async System.Threading.Tasks.Task DoDbActions(
			DeletePersonelAddressCommand request,
			CancellationToken cancellationToken)
		{
			var personel = await AppDbContext.Personels.FindAsync(request.PersonelId);
			await DeleteAddressInPersonel(personel, request, cancellationToken);
		}

		private async System.Threading.Tasks.Task DeleteAddressInPersonel(
			DomainLayer.Entities.Personel personel,
			DeletePersonelAddressCommand personelAddress,
			CancellationToken cancellationToken)
		{
			var addressInPrsonel = personel.Addresses.FirstOrDefault(m => m.Id == personelAddress.AddressId);
			var addr = AppDbContext.PersonelAddresses.Find(personelAddress.AddressId);
			if (addressInPrsonel != null)
				personel.Addresses.Remove(addressInPrsonel);

			AppDbContext.PersonelAddresses.Remove(addr);
			
			await AppDbContext.SaveChangesAsync(cancellationToken);
		}

		private async System.Threading.Tasks.Task ValidateInput(DeletePersonelAddressCommand personelAddress)
		{
			var validationResult = await Validator.ValidateAsync(personelAddress);

			if (validationResult.Any())
				throw validationResult.First();
		}
	}
}
