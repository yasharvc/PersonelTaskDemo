using DomainLayer.Common;

namespace DomainLayer.Entities
{
	public class PersonelTask : BaseEntity
	{
		public string PersonelId { get; set; }
		public Personel Personel { get; set; }

		public string TaskId { get; set; }
		public Task Task { get; set; }
	}
}