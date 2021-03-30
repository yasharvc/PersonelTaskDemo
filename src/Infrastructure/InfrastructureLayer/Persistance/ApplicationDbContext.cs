using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Persistance
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
		{

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
        }

        public DbSet<Personel> Personels { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<PersonelTask> PersonelTasks { get; set; }
    }
}
