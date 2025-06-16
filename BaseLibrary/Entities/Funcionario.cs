using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Entities
{
    public class Funcionario  : BaseEntity
    {
        [Required]
        public string? CivilId { get; set; } = string.Empty;
        [Required]
        public string? NumeroArquivo { get; set; } = string.Empty;
        [Required]
        public string? NomeCompleto { get; set; } = string.Empty;
        [Required]
        public string? NomeDoTrabalho { get; set; } = string.Empty;
        [Required]
        public string? Endereco { get; set; } = string.Empty;
        [Required, DataType(DataType.PhoneNumber)]
        public string? NumeroTelefone { get; set; } = string.Empty;
        [Required]
        public string? Foto { get; set; } = string.Empty;
        public string? Outros { get; set; }  

       // Relationship : N para 1

        public Filial? Filial { get; set; }
        public int FilialId { get; set; }
        public Cidade? Cidade { get; set; }
        public int CidadeId { get; set; }
    }
}
