using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Pais : BaseEntity
    {
        [JsonIgnore]
        public List<Cidade>? Cidades { get; set; }
    }
}
