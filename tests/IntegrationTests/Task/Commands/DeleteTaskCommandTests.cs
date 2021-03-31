using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Task.Commands.CreateTask;
using ApplicationLayer.Task.Commands.DeleteTask;
using ApplicationLayer.Task.Commands.UpdateTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Task.Commands
{
	public class DeleteTaskCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>, CreateTaskCommandHandler>();

			services.AddSingleton<IRequestHandler<DeleteTaskCommand, DomainLayer.Entities.Task>, DeleteTaskCommandHandler>();
		}

		[Fact]
		public async void Should_Delete_Entity_Successfully()
		{
			var cmd = new CreateTaskCommand
			{
				Description="Test",
				DueDate=DateTime.Now.AddDays(1),
				Title="Before"
			};

			var createHandler = ServiceProvider.GetService<IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>>();
			var deleteHandler = ServiceProvider.GetService<IRequestHandler<DeleteTaskCommand, DomainLayer.Entities.Task>>();

			var insertedTask = await createHandler.Handle(cmd, new CancellationToken());

			var result = await deleteHandler.Handle(new DeleteTaskCommand
			{
				Id = insertedTask.Id
			}, new CancellationToken());


			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(0, await ApplicationDbContext.Tasks.CountAsync());
		}
	}
}