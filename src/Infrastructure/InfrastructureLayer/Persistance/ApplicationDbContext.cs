using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.Common.Interfaces;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace InfrastructureLayer.Persistance
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
		public DbSet<Personel> Personels { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<PersonelTask> PersonelTasks { get; set; }
		public DbSet<PersonelAddress> PersonelAddresses { get; set; }
		Dictionary<string, IDbContextTransaction> Transactions { get; set; } = new Dictionary<string, IDbContextTransaction>();
		ILogger Logger { get; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,ILogger logger)
            : base(options)
		{
			Logger = logger;
		}

		public string BeginTransaction()
		{
			var guid = $"TRAN_{Guid.NewGuid().ToString().Replace("-", "")}";
			Transactions[guid] = Database.BeginTransaction();
			//Transactions[guid].CreateSavepoint(guid);
			return guid;
		}

		public async System.Threading.Tasks.Task CommitTransactionAsync(string transId)
		{
			try
			{
				if (Transactions.ContainsKey(transId))
				{
					var tran = Transactions[transId];
					await tran.CommitAsync();
					Transactions.Remove(transId);
					//GC.SuppressFinalize(tran);
				}
				else
				{
					throw new NotFoundException();
				}
			}
			catch(Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}

		public async System.Threading.Tasks.Task RollbackTransactionAsync(string transId)
		{
			if (string.IsNullOrEmpty(transId))
				return;
			try
			{
				if (Transactions.ContainsKey(transId))
				{
					var tran = Transactions[transId];
					await tran.RollbackAsync();
					//await tran.CreateSavepointAsync(transId);
					Transactions.Remove(transId);
					//GC.SuppressFinalize(tran);
				}
			}
			catch (Exception e)
			{
				await Logger.ErrorAsync(e);
				throw;
			}
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonelTask>().HasKey(pt => new { pt.PersonelId, pt.TaskId });

            modelBuilder.Entity<PersonelTask>()
                .HasOne(p => p.Personel)
                .WithMany(s => s.PersonelTasks)
                .HasForeignKey(sc => sc.PersonelId);


            modelBuilder.Entity<PersonelTask>()
                .HasOne(p => p.Task)
                .WithMany(s => s.PersonelTasks)
                .HasForeignKey(sc => sc.TaskId);

			modelBuilder.Entity<Personel>()
				.HasMany(c => c.Addresses)
				.WithOne(p => p.Personel);
        }
	}
}
