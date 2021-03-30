using System;

namespace DomainLayer.Common
{
	public abstract class BaseEntity
	{
		public string Id { get; set; }
		public DateTime Created { get; set; }
		public BaseEntity()
		{
			Created = DateTime.Now;
			Id = Guid.NewGuid().ToString();
		}
	}
}