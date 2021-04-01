using ApplicationLayer.Common.Mappings;

namespace ApplicationLayer.Personel.Queries
{
	public class PersonelAddressDto : IMapFrom<DomainLayer.Entities.PersonelAddress>
	{
		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string No { get; set; }
	}
}