using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class Registro : ContaBase
    {
        [Required]
        [MinLength(5)] 
        [MaxLength(100)]
        public string? NomeCompleto { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Senha), ErrorMessage = "As senhas não coincidem.")]
        public string? ConfirmarSenha { get; set; }
    }
}
