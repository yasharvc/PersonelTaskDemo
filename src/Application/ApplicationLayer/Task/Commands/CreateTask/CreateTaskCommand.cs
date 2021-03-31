﻿using ApplicationLayer.Common.Interfaces;
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
	}
	public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, DomainLayer.Entities.Task>
	{
		public Task<DomainLayer.Entities.Task> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
