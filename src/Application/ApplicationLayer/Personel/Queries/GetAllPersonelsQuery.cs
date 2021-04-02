using ApplicationLayer.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Queries
{
	public class GetAllPersonelsQuery : IRequest<PersonelsVm>
	{
	}

	public class GetAllPersonelsQueryHandler : IRequestHandler<GetAllPersonelsQuery, PersonelsVm>
	{
		IApplicationDbContext AppDbContext { get; }
		IMapper Mapper { get; }

		public GetAllPersonelsQueryHandler(IApplicationDbContext context, IMapper mapper)
		{
			AppDbContext = context;
			Mapper = mapper;
		}
		public async Task<PersonelsVm> Handle(GetAllPersonelsQuery request, CancellationToken cancellationToken)
		{
			return new PersonelsVm
			{
				Personels = await AppDbContext.Personels
					.AsNoTracking()
					.ProjectTo<PersonelDto>(Mapper.ConfigurationProvider)
					.OrderBy(t => t.FirstName)
					.ToListAsync(cancellationToken)
			};
		}
	}
}