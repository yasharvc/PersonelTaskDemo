using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IApplicationDbContext
    {
        DbSet<DomainLayer.Entities.Task> Tasks { get; set; }

        public DbSet<DomainLayer.Entities.Personel> Personels { get; set; }
        public DbSet<PersonelTask> PersonelTasks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}