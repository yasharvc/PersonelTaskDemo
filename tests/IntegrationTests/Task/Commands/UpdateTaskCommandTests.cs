using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Task.Commands.CreateTask;
using ApplicationLayer.Task.Commands.UpdateTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace IntegrationTests.Task.Commands
{
	public class UpdateTaskCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>, CreateTaskCommandHandler>();

			services.AddSingleton<IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>, UpdateTaskCommandHandler>();
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

			var handler = ServiceProvider.GetService<IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>>();

			var result = await handler.Handle(cmd, new CancellationToken());

			Assert.False(string.IsNullOrEmpty(result.Id));
			Assert.Equal(1, await ApplicationDbContext.Tasks.CountAsync());
			Assert.Equal(result.Id, (await ApplicationDbContext.Tasks.FirstAsync()).Id);
		}
	}
}