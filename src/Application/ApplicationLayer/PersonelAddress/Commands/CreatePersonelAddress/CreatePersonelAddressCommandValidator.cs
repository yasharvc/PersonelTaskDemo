using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.Common.Exceptions.Application;
using ApplicationLayer.Common.Exceptions.Application.Personel;
using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress
{
	public class CreatePersonelAddressCommandValidator : IValidator<CreatePersonelAddressCommand>
	{
		public CreatePersonelAddressCommandValidator(
			IRequestHandler<GetPersonelByIdQuery, PersonelVm> personelFinder)
		{
			PersonelFinder = personelFinder;
		}

		IRequestHandler<GetPersonelByIdQuery, PersonelVm> PersonelFinder { get; }

		public IEnumerable<Exception> Validate(CreatePersonelAddressCommand value)
		{
			var res = new List<Exception>();

			if (string.IsNullOrEmpty(value.PersonelId))
				res.Add(new PersonelNotFoundException());
			if (string.IsNullOrEmpty(value.Id))
				res.Add(new InvalidIdException());
			CheckForPersonel(value, res).RunSynchronously();

			return res;
		}

		public async Task<IEnumerable<Exception>> ValidateAsync(CreatePersonelAddressCommand value)
		{
			var res = new List<Exception>();

			if (string.IsNullOrEmpty(value.PersonelId))
				res.Add(new PersonelNotFoundException());
			await CheckForPersonel(value, res);

			return res;
		}

		private async System.Threading.Tasks.Task CheckForPersonel(DomainLayer.Entities.PersonelAddress value, List<Exception> res)
		{
			var personel = await PersonelFinder.Handle(new GetPersonelByIdQuery
			{
				PersonelId = value.Personel.Id
			}, new System.Threading.CancellationToken());
			if (personel == null)
				res.Add(new NotFoundException());
			GC.SuppressFinalize(personel);
		}
	}
}
