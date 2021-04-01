using ApplicationLayer.Common.Mappings;
using System;
using System.Collections.Generic;

namespace ApplicationLayer.Personel.Queries
{
	public class PersonelDto : IMapFrom<DomainLayer.Entities.Personel>
	{
		public PersonelDto()
		{
			Addresses = new List<PersonelAddressDto>();
			PersonelTasks = new List<PersonelTaskDto>();
		}
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public virtual ICollection<PersonelAddressDto> Addresses { get; set; }
		public virtual IList<PersonelTaskDto> PersonelTasks { get; set; }
	}
}