using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Domain.Entities.Scheduler
{
	public class Course
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public double Credits { get; set; }
		[DisplayName("Starting Year")]
		public int StartYear { get; set; } = DateTime.Now.Year;
		public int EndYear { get; set; } = 2100;
		public bool Disabled { get; set; }

		public int DepartmentId { get; set; }
		public Department Department { get; set; }
	}
}
				