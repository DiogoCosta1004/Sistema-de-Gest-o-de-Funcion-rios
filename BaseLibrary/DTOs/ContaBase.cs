using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class ContaBase
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]

        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string? Senha { get; set; }
    }
}
