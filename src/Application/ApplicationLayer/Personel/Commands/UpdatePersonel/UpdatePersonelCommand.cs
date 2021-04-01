using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Commands.UpdatePersonel
{
	public class UpdatePersonelCommand : IRequest<DomainLayer.Entities.Personel>
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public static implicit operator DomainLayer.Entities.Personel(UpdatePersonelCommand c)
		{
			return new DomainLayer.Entities.Personel
			{
				Id = c.Id,
				FirstName = c.FirstName,
				LastName = c.LastName,
				DateOfBirth = c.DateOfBirth
			};
		}
	}
	public class UpdatePersonelCommandHandler : IRequestHandler<UpdatePersonelCommand, DomainLayer.Entities.Personel>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public UpdatePersonelCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Personel> Handle(UpdatePersonelCommand request, CancellationToken cancellationToken)
		{
			try
			{
				DomainLayer.Entities.Personel personel = request;
				var entity = await AppDbContext.Personels.FindAsync(personel.Id);

				if (entity == null)
					throw new NotFoundException();

				entity.DateOfBirth = personel.DateOfBirth;
				entity.FirstName = personel.FirstName;
				entity.LastName = personel.LastName;

				await AppDbContext.SaveChangesAsync(cancellationToken);

				return entity;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}
