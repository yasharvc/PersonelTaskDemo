using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Personel.Commands.CreatePersonel
{
	public class CreatePersonelCommand : IRequest<DomainLayer.Entities.Personel>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		public DomainLayer.Entities.Personel ToEntity() => this;

		public static implicit operator DomainLayer.Entities.Personel(CreatePersonelCommand c)
		{
			return new DomainLayer.Entities.Personel
			{
				FirstName = c.FirstName,
				LastName = c.LastName,
				DateOfBirth = c.DateOfBirth
			};
		}
	}
	public class CreatePersonelCommandHandler : IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public CreatePersonelCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Personel> Handle(CreatePersonelCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var newPersonel = request.ToEntity();

				AppDbContext.Personels.Add(newPersonel);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				return newPersonel;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}