using ApplicationLayer.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress
{
	public class CreatePersonelAddressCommandValidator : IValidator<DomainLayer.Entities.PersonelAddress>
	{
		public IEnumerable<Exception> Validate(DomainLayer.Entities.PersonelAddress value)
		{
			if(value.Personel == null)

		}
	}
}
