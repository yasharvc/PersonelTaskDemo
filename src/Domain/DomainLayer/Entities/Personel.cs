using DomainLayer.Common;
using System;
using System.Collections.Generic;

namespace DomainLayer.Entities
{
	public class Personel : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public virtual ICollection<PersonelAddress> Addresses { get; set; } = new List<PersonelAddress>();

		public virtual IList<PersonelTask> PersonelTasks { get; set; } = new List<PersonelTask>();
	}
}