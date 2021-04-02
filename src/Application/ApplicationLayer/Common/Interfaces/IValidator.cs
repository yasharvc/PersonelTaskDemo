using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IValidator<T> where T : BaseEntity
	{
		IEnumerable<Exception> Validate(T value);
		Task<IEnumerable<Exception>> ValidateAsync(T value);
	}
}