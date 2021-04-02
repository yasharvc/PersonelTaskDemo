using System.Collections.Generic;

namespace ApplicationLayer.Personel.Queries
{
	public class PersonelTaskVm
	{
		public PersonelDto Personel { get; set; }
		public IList<PersonelTaskDto> PersonelTasks { get; set; }
	}
}