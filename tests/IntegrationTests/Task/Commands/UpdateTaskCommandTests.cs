using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Task.Commands.CreateTask;
using ApplicationLayer.Task.Commands.UpdateTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Task.Commands
{
	public class UpdateTaskCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>, CreateTaskCommandHandler>();

			services.AddSingleton<IRequestHandler<UpdateTaskCommand, DomainLayer.Entities.Task>, UpdateTaskCommandHandler>();
		}

		[Fact]
		public async void Should_Update_Entity_Successfully()
		{
			var cmd = new CreateTaskCommand
			{
				Description="Test",
				DueDate=DateTime.Now.AddDays(1),
				Title="Before"
			};

			var createHandler = ServiceProvider.GetService<IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>>();
			var updateHandler = ServiceProvider.GetService<IRequestHandler<UpdateTaskCommand, DomainLayer.Entities.Task>>();

			var insertedTask = await createHandler.Handle(cmd, new CancellationToken());

			var result = await updateHandler.Handle(new UpdateTaskCommand
			{
				Id = insertedTask.Id,
				Description = insertedTask.Description,
				Title = "After",
				DueDate = insertedTask.DueDate
			}, new CancellationToken());


			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(1, await ApplicationDbContext.Tasks.CountAsync());
			Assert.Equal(result.Title, (await ApplicationDbContext.Tasks.FirstAsync()).Title);
		}
	}
}