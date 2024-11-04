using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Prova.MarQ.Domain.Entities
{
    public class Employee : Base
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Documento { get; set; }

        [Required]
        [MaxLength(4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "O PIN deve conter exatamente 4 dígitos numéricos.")]
        public string PIN { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
