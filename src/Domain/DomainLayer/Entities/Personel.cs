using DomainLayer.Common;
using System;
using System.Collections.Generic;

namespace DomainLayer.Entities
{
	public class Personel : BaseEntity
	{
		public Personel()
		{
			TaskNavigations = new HashSet<Task>();
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public virtual ICollection<PersonelAddress> Addresses { get; set; }

		public virtual ICollection<Task> TaskNavigations { get; set; }
	}
}