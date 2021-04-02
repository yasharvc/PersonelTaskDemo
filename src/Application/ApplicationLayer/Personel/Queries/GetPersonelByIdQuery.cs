using ApplicationLayer.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Queries
{
	public class GetPersonelByIdQuery : IRequest<PersonelVm>
	{
		public string PersonelId { get; set; }
	}

	public class GetPersonelByIdQueryHandler : IRequestHandler<GetPersonelByIdQuery, PersonelVm>
	{
		IApplicationDbContext AppDbContext { get; }
		IMapper Mapper { get; }

		public GetPersonelByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
		{
			AppDbContext = context;
			Mapper = mapper;
		}
		public async Task<PersonelVm> Handle(GetPersonelByIdQuery request, CancellationToken cancellationToken)
		{
			return new PersonelVm
			{
				Personel = await AppDbContext.Personels
					.AsNoTracking()
					.Where(m => m.Id == request.PersonelId)
					.ProjectTo<PersonelDto>(Mapper.ConfigurationProvider)
					.OrderBy(t => t.FirstName)
					.FirstOrDefaultAsync(cancellationToken)
			};
		}
	}
}
