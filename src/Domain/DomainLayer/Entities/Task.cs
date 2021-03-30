using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
	public class Task : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime? DueDate { get; set; }
		public TaskStatus Status { get; set; }

		public virtual IList<PersonelTask> PersonelTasks { get; set; }
	}
}