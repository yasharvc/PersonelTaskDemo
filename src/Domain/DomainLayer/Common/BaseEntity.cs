using System;

namespace DomainLayer.Common
{
	public abstract class BaseEntity
	{
		public string Id { get; set; }
		public DateTime Created { get; set; }
	}
}