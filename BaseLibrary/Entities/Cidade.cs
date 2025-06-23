using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class Cidade : BaseEntity
    {
        public Pais? Pais { get; set; }
        public int PaisId { get; set; }
        [JsonIgnore]
        public List<Town> Towns { get; set; }
    }
}
