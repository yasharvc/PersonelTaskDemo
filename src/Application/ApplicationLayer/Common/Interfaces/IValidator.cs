using DomainLayer.Common;
using System;
using System.Collections.Generic;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IValidator<T> where T : BaseEntity
	{
		IEnumerable<Exception> Validate(T value);
	}
}