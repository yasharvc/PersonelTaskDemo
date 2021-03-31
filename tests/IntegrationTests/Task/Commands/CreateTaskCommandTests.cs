using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Task.Commands.CreateTask;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Task.Commands
{
	public class CreateTaskCommandTests : DependencyInjection
	{
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			services.AddSingleton<IRequest<DomainLayer.Entities.Task>, CreateTaskCommand>();
			services.AddSingleton<IRequestHandler<IRequest<DomainLayer.Entities.Task>, DomainLayer.Entities.Task>, CreateTaskCommandHandler>();
		}
	}
}
