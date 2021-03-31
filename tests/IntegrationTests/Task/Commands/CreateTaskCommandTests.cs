using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Task.Commands.CreateTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace IntegrationTests.Task.Commands
{
	public class CreateTaskCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequest<DomainLayer.Entities.Task>, CreateTaskCommand>();
			services.AddSingleton<IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>, CreateTaskCommandHandler>();
		}

		[Fact]
		public async void Should_Create_Entity_Successfully()
		{
			var cmd = new CreateTaskCommand
			{
				Description="Test",
				DueDate=DateTime.Now.AddDays(1),
				Title="Title"
			};

			var handler = ServiceProvider.GetService<IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>>();

			var result = await handler.Handle(cmd, new CancellationToken());

			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(1, await ApplicationDbContext.Tasks.CountAsync());
			Assert.Equal(result.Id, (await ApplicationDbContext.Tasks.FirstAsync()).Id);
		}
	}
}