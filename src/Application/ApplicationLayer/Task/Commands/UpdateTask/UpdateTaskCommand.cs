using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Task.Commands.UpdateTask
{
	public class UpdateTaskCommand : IRequest<DomainLayer.Entities.Task>
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? DueDate { get; set; }

		public DomainLayer.Entities.Task ToEntity() => this;

		public static implicit operator DomainLayer.Entities.Task(UpdateTaskCommand c)
		{
			return new DomainLayer.Entities.Task
			{
				Id = c.Id,
				Description = c.Description,
				Title = c.Title,
				DueDate = c.DueDate,
				Status = DomainLayer.Enums.TaskStatus.NotStarted
			};
		}
	}
	public class UpdateTaskCommandHandler : IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public UpdateTaskCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Task> Handle(IRequest<DomainLayer.Entities.Task> request, CancellationToken cancellationToken)
		{
			try
			{
				var newTask = request.ToEntity();

				AppDbContext.Tasks.Add(newTask);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				return newTask;
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}