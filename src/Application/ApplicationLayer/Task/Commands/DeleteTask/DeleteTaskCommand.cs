using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Task.Commands.DeleteTask
{
	public class DeleteTaskCommand : IRequest<DomainLayer.Entities.Task>
	{
		public string Id { get; set; }

		public DomainLayer.Entities.Task ToEntity() => new DomainLayer.Entities.Task {
			Id = Id
		};
	}
	public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DomainLayer.Entities.Task>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public DeleteTaskCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Task> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var task = await AppDbContext.Tasks.FindAsync(request.ToEntity().Id);

				if (task == null)
					throw new NotFoundException();

				AppDbContext.Tasks.Remove(task);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				return task;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}
