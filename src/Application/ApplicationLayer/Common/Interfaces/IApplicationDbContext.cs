using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Common.Interfaces
{
	public interface IApplicationDbContext
    {
        DbSet<DomainLayer.Entities.Task> Tasks { get; set; }

        DbSet<DomainLayer.Entities.Personel> Personels { get; set; }
        DbSet<DomainLayer.Entities.PersonelTask> PersonelTasks { get; set; }
        DbSet<DomainLayer.Entities.PersonelAddress> PersonelAddresses { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        string BeginTransaction();
        System.Threading.Tasks.Task CommitTransactionAsync(string transId);
        System.Threading.Tasks.Task RollbackTransactionAsync(string transId);
    }
}