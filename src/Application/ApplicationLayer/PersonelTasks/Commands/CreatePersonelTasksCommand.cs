using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Personel.Queries;
using ApplicationLayer.PersonelTasks.Queries;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.PersonelTasks.Commands
{
	public class CreatePersonelTasksCommand : IRequest<PersonelTaskVm>
	{
		public string PersonelId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public IList<DomainLayer.Entities.Task> PersonelTasks { get; set; }
	}

	public class CreatePersonelTasksCommandHandler : IRequestHandler<CreatePersonelTasksCommand, PersonelTaskVm>
	{
		IApplicationDbContext AppDbContext { get; }
		ILogger Logger { get; }
		IValidator<CreatePersonelTasksCommand> Validator { get; }
		IRequestHandler<GetPersonelTasksWithPersonelIdQuery, PersonelTaskVm> PersonelTaskFinder { get; }

		public CreatePersonelTasksCommandHandler(
			IApplicationDbContext context,
			ILogger logger,
			IRequestHandler<GetPersonelTasksWithPersonelIdQuery, PersonelTaskVm> personelFinder)
		{
			AppDbContext = context;
			Logger = logger;
			PersonelTaskFinder = personelFinder;
		}
		public async Task<PersonelTaskVm> Handle(CreatePersonelTasksCommand request, CancellationToken cancellationToken)
		{
			var tranId = "";
			try
			{
				await ValidateInput(request);

				tranId = AppDbContext.BeginTransaction();
				await DoDbActions(request, cancellationToken);

				await AppDbContext.CommitTransactionAsync(tranId);
				return await PersonelTaskFinder.Handle(new GetPersonelTasksWithPersonelIdQuery { PersonelId = request.PersonelId }, cancellationToken);
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				await AppDbContext.RollbackTransactionAsync(tranId);
				throw;
			}
		}

		private async System.Threading.Tasks.Task DoDbActions(CreatePersonelTasksCommand request, CancellationToken cancellationToken)
		{
			var personel = await AppDbContext.Personels.FindAsync(request.PersonelId);

			foreach (var item in request.PersonelTasks)
			{
				var searchForTask = await FindTask(item.Id);
				if (searchForTask == null)
				{
					searchForTask = await CreateTask(item, request.PersonelId, cancellationToken);
					personel.PersonelTasks.Add(searchForTask);
				}
				else
				{
					var taskToEdit = await AppDbContext.Tasks.FindAsync(item.Id);
					taskToEdit.DueDate = item.DueDate;
					taskToEdit.Description = item.Description;
					taskToEdit.Status = item.Status;
					taskToEdit.Title = item.Title;
				}
				await AppDbContext.SaveChangesAsync(cancellationToken);
			}
		}

		private async Task<PersonelTask> CreateTask(DomainLayer.Entities.Task request, string personelId, CancellationToken cancellationToken)
		{
			var task = await AppDbContext.Tasks.FindAsync(request.Id);
			if (task == null)
			{
				task = new DomainLayer.Entities.Task
				{
					DueDate = request.DueDate,
					Title = request.Title,
					Status = request.Status,
					Description = request.Description
				};
				AppDbContext.Tasks.Add(task);
				await AppDbContext.SaveChangesAsync(cancellationToken);
			}
			var personel = await AppDbContext.Personels.FindAsync(personelId);
			var newPersonelTask = new PersonelTask
			{
				Personel = personel,
				PersonelId = personelId,
				Task = task,
				TaskId = request.Id
			};

			await AppDbContext.PersonelTasks.AddAsync(newPersonelTask);
			return newPersonelTask;
		}

		private Task<PersonelTask> FindTask(string taskId) => System.Threading.Tasks.Task.FromResult(
			AppDbContext.PersonelTasks
			.FirstOrDefault(m=>m.TaskId == taskId));

		private System.Threading.Tasks.Task ValidateInput(CreatePersonelTasksCommand request)
		{
			return System.Threading.Tasks.Task.CompletedTask;
		}
	}
}
