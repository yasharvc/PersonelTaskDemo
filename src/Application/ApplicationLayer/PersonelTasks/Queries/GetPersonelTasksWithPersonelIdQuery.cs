using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelTasks.Queries
{
	public class GetPersonelTasksWithPersonelIdQuery : IRequest<PersonelTaskVm>
	{
		public string PersonelId { get; set; }
	}
	public class GetPersonelTasksWithPersonelIdQueryHandler : IRequestHandler<GetPersonelTasksWithPersonelIdQuery, PersonelTaskVm>
	{
		IApplicationDbContext AppDbContext { get; }
		IMapper Mapper { get; }

		public GetPersonelTasksWithPersonelIdQueryHandler(IApplicationDbContext context, IMapper mapper)
		{
			AppDbContext = context;
			Mapper = mapper;
		}
		public async Task<PersonelTaskVm> Handle(GetPersonelTasksWithPersonelIdQuery request, CancellationToken cancellationToken)
		{
			return new PersonelTaskVm
			{
				PersonelTasks = await AppDbContext.PersonelTasks
					.AsNoTracking()
					.Where(m => m.PersonelId == request.PersonelId)
					.ProjectTo<PersonelTaskDto>(Mapper.ConfigurationProvider)
					.ToListAsync(cancellationToken)
				,
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
