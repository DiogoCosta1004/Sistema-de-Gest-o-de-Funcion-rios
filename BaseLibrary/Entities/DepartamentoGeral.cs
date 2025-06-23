using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class DepartamentoGeral : BaseEntity
    {
        [JsonIgnore]
        public List<Departamento>? Departamentos { get; set; }
    }
}
