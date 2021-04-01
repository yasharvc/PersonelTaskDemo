using ApplicationLayer.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Queries
{
	public class GetAllPersonelsQuery : IRequest<PersonalsVm>
	{
	}

	public class GetAllPersonelsQueryHandler : IRequestHandler<GetAllPersonelsQuery, PersonalsVm>
	{
		IApplicationDbContext AppDbContext { get; }
		IMapper Mapper { get; }

		public GetAllPersonelsQueryHandler(IApplicationDbContext context, IMapper mapper)
		{
			AppDbContext = context;
			Mapper = mapper;
		}
		public async Task<PersonalsVm> Handle(GetAllPersonelsQuery request, CancellationToken cancellationToken)
		{
			return new PersonalsVm
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