using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Commands.DeletePersonel
{
	public class DeletePersonelCommand : IRequest<DomainLayer.Entities.Personel>
	{
		public string Id { get; set; }

		public DomainLayer.Entities.Personel ToEntity() => new DomainLayer.Entities.Personel
		{
			Id = Id
		};
	}
	public class DeletePersonelCommandHandler : IRequestHandler<DeletePersonelCommand, DomainLayer.Entities.Personel>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public DeletePersonelCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Personel> Handle(DeletePersonelCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var Personel = await AppDbContext.Personels.FindAsync(request.ToEntity().Id);

				if (Personel == null)
					throw new NotFoundException();

				AppDbContext.Personels.Remove(Personel);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				return Personel;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}
