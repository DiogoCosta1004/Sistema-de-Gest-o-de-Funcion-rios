namespace BaseLibrary.Entities
{
    public class Funcionario 
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? CivilId { get; set; }
        public string? NumeroArquivo { get; set; }
        public string? NomeCompleto { get; set; }
        public string? NomeDoTrabalho { get; set; }
        public string? Endereco { get; set; }
        public string? NumeroTelefone { get; set; }
        public string? Foto { get; set; }
        public string? Outros { get; set; }  

       // Relationship : N para 1

        public DepartamentoGeral? DepartamentoGeral { get; set; }
        public int DepartamentoGeralId { get; set; }
        public Departamento? Departamento { get; set; }
        public int DepartamentoId { get; set; }
        public Filial? Filial { get; set; }
        public int FilialId { get; set; }
        public Cidade? Cidade { get; set; }
        public int CidadeId { get; set; }
    }
}
