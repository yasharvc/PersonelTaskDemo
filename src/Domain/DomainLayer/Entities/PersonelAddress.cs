using DomainLayer.Common;

namespace DomainLayer.Entities
{
	public class PersonelAddress : BaseEntity
	{
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string No { get; set; }
		public Personel Personel { get; set; }
	}
}