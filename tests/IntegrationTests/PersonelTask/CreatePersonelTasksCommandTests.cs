using ApplicationLayer.Common.Interfaces;
using ApplicationLayer.Common.Mappings;
using ApplicationLayer.Personel.Commands.CreatePersonel;
using ApplicationLayer.Personel.Queries;
using ApplicationLayer.PersonelAddress.Commands.CreatePersonelAddress;
using ApplicationLayer.PersonelTasks.Commands;
using ApplicationLayer.PersonelTasks.Queries;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace IntegrationTests.PersonelTask
{
	public class CreatePersonelTasksCommandTests : DependencyInjection
	{
		public CreatePersonelTasksCommandTests() : base(true) { }
		protected override void AddAdditionalServices(IServiceCollection services)
		{
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>();
			});
			services.AddSingleton(configuration.CreateMapper());

			services.AddSingleton<IRequestHandler<GetPersonelTasksWithPersonelIdQuery, PersonelTaskVm>, GetPersonelTasksWithPersonelIdQueryHandler>();
			services.AddSingleton<IRequestHandler<CreatePersonelTasksCommand, PersonelTaskVm>, CreatePersonelTasksCommandHandler>();


			services.AddSingleton<IRequestHandler<GetPersonelByIdQuery, PersonelVm>, GetPersonelByIdQueryHandler>();


			services.AddSingleton<IRequest<DomainLayer.Entities.Personel>, CreatePersonelCommand>();

			services.AddSingleton<IValidator<CreatePersonelAddressCommand>, CreatePersonelAddressCommandValidator>();
			services.AddSingleton<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>, CreatePersonelCommandHandler>();

			services.AddSingleton<IRequestHandler<GetPersonelByIdQuery, PersonelVm>, GetPersonelByIdQueryHandler>();

			services.AddSingleton<IRequestHandler<CreatePersonelAddressCommand, PersonelVm>, CreatePersonelAddressCommandHandler>();
		}

		[Fact]
		public async void Should_Create_PersonelTasks_From_Scratch()
		{
			var insertPersonelHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelCommand, DomainLayer.Entities.Personel>>();
			var insertedPersonel = await insertPersonelHandler.Handle(new CreatePersonelCommand
			{
				FirstName = "Yashar",
				LastName = "Aliabbasi",
				DateOfBirth = DateTime.Now.AddYears(-30)
			}, new CancellationToken());

			var personelTaskHandler = ServiceProvider.GetService<IRequestHandler<CreatePersonelTasksCommand, PersonelTaskVm>>();
			var personelTasks = await personelTaskHandler.Handle(new CreatePersonelTasksCommand
			{
				DateOfBirth = insertedPersonel.DateOfBirth,
				FirstName = "Test",
				LastName = "Test",
				PersonelId = insertedPersonel.Id,
				PersonelTasks = new List<DomainLayer.Entities.Task>
				{
					new DomainLayer.Entities.Task
					{
						Description = "Task 1 desc",
						Title = "Title 1",
						Status = DomainLayer.Enums.TaskStatus.NotStarted
					}
				}
			}, new CancellationToken());

			Assert.Equal(1, ApplicationDbContext.Tasks.Count());
			Assert.Equal(1, ApplicationDbContext.PersonelTasks.Count());
		}
	}
}
