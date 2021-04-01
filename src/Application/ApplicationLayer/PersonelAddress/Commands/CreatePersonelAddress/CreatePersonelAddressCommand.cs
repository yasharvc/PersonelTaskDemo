using ApplicationLayer.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress
{
	public class CreatePersonelAddressCommand : IRequest<DomainLayer.Entities.PersonelAddress>
	{
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string No { get; set; }
		public string PersonelId { get; set; }

		public DomainLayer.Entities.PersonelAddress ToEntity() => this;

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
	public class CreatePersonelAddressCommandHandler : IRequestHandler<CreatePersonelAddressCommand, DomainLayer.Entities.PersonelAddress>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public CreatePersonelAddressCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.PersonelAddress> Handle(CreatePersonelAddressCommand request, CancellationToken cancellationToken)
		{
			var tranId = "";
			try
			{
				var newPersonelAddress = request.ToEntity();

				tranId = AppDbContext.BeginTransaction();

				AppDbContext.PersonelAddresses.Add(newPersonelAddress);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				await AppDbContext.CommitTransactionAsync(tranId);
				return newPersonelAddress;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				await AppDbContext.RollbackTransactionAsync(tranId);
				throw;
			}
		}
	}
}
