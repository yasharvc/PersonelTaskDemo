using ApplicationLayer.Common.Exceptions.Application;
using ApplicationLayer.Common.Exceptions.Application.Personel;
using ApplicationLayer.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress
{
	public class CreatePersonelAddressCommandValidator : IValidator<DomainLayer.Entities.PersonelAddress>
	{
		public IEnumerable<Exception> Validate(DomainLayer.Entities.PersonelAddress value)
		{
			var res = new List<Exception>();
			if (value.Personel == null)
				res.Add(new PersonelNotFoundException());
			if (string.IsNullOrEmpty(value.Id))
				res.Add(new InvalidIdException());
			return res;
		}
	}
}
