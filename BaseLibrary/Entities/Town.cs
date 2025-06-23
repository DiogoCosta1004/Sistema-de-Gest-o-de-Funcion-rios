using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {
        [JsonIgnore]
        public List<Funcionario>? Funcionarios { get; set; }
        public Cidade? Cidade { get; set; }
        public int CidadeId { get; set; }
    }
}
