using ApplicationLayer.Common.Mappings;
using AutoMapper;
using System;

namespace ApplicationLayer.Personel.Queries
{
	public class PersonelTaskDto : IMapFrom<DomainLayer.Entities.PersonelTask>
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? DueDate { get; set; }
		public string Status { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<DomainLayer.Entities.PersonelTask, PersonelTaskDto>()
				.ForMember(d => d.Title, opt => opt.MapFrom(s => s.Task.Title))
				.ForMember(d => d.Description, opt => opt.MapFrom(s => s.Task.Description))
				.ForMember(d => d.DueDate, opt => opt.MapFrom(s => s.Task.DueDate))
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Task.Status.ToString()));
		}
	}
}