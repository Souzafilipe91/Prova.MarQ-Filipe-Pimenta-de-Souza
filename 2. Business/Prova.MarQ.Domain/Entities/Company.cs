using System.ComponentModel.DataAnnotations;

namespace Prova.MarQ.Domain.Entities
{
    public class Company : Base
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O documento é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O documento deve estar no formato CPF (ex: 123.456.789-00).")]
        public string Document { get; set; }

        // Exemplo de outra propriedade com validação
        [EmailAddress(ErrorMessage = "O e-mail deve estar em um formato válido.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "O telefone deve estar em um formato válido.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Address { get; set; }
    }
}
