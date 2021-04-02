using ApplicationLayer.Common.Exceptions.Application;
using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.DeletePersonelAddress
{
	public class DeletePersonelAddressCommandValidator : IValidator<DeletePersonelAddressCommand>
	{
		public DeletePersonelAddressCommandValidator(
			IRequestHandler<GetPersonelByIdQuery, PersonelVm> personelFinder)
		{
			PersonelFinder = personelFinder;
		}

		IRequestHandler<GetPersonelByIdQuery, PersonelVm> PersonelFinder { get; }

		public IEnumerable<Exception> Validate(DeletePersonelAddressCommand value)
		{
			var res = new List<Exception>();

			if (string.IsNullOrEmpty(value.PersonelId))
				res.Add(new InvalidIdException());
			if (string.IsNullOrEmpty(value.AddressId))
				res.Add(new InvalidIdException());

			return res;
		}

		public Task<IEnumerable<Exception>> ValidateAsync(DeletePersonelAddressCommand value) => System.Threading.Tasks.Task.FromResult(Validate(value));
	}
}
