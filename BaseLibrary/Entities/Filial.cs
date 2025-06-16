namespace BaseLibrary.Entities
{
    public class Filial : BaseEntity
    {
        public Departamento? Departamento { get; set; }
        public int DepartamentoId { get; set; }

        public List<Funcionario>? Funcionarios { get; set; }
    }
}
