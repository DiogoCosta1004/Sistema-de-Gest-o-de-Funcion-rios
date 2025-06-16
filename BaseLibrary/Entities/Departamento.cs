namespace BaseLibrary.Entities
{
    public class Departamento : BaseEntity
    {
        public DepartamentoGeral? DepartamentoGeral { get; set; }
        public int DepartamentoGeralId { get; set; }

        public List<Filial>? Filiais { get; set; }
    }
}
