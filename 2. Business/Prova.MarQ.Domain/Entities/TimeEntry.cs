using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova.MarQ.Domain.Entities
{
    public class TimeEntry
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime Timestamp { get; set; }

        public Employee Employee { get; set; }
    }
}
