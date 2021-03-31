using ApplicationLayer.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Task.Commands.CreateTask
{
	public class CreateTaskCommand : IRequest<DomainLayer.Entities.Task>
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? DueDate { get; set; }

		public static implicit operator DomainLayer.Entities.Task(CreateTaskCommand c)
		{
			return new DomainLayer.Entities.Task
			{
				Description = c.Description,
				Title = c.Title,
				DueDate = c.DueDate,
				Status = DomainLayer.Enums.TaskStatus.NotStarted
			};
		}
	}
	public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>
	{
		private IApplicationDbContext AppDbContext { get; }
		private ILogger Logger { get; }

		public CreateTaskCommandHandler(IApplicationDbContext context,
			ILogger logger)
		{
			AppDbContext = context;
			Logger = logger;
		}
		public async Task<DomainLayer.Entities.Task> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
		{
			try
			{
				DomainLayer.Entities.Task newTask = request;

				AppDbContext.Tasks.Add(newTask);
				await AppDbContext.SaveChangesAsync(cancellationToken);

				return newTask;
			}catch(Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}
	}
}
