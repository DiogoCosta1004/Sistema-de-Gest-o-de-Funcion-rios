using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Departamento : BaseEntity
    {
        public DepartamentoGeral? DepartamentoGeral { get; set; }
        public int DepartamentoGeralId { get; set; }
        [JsonIgnore]
        public List<Filial>? Filiais { get; set; }
    }
}
