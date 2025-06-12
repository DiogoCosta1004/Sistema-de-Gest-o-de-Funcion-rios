using System.Text.Json.Serialization;

namespace BaseLibrary.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        //Aqui teremos uma relação de 1:N
        [JsonIgnore]
        public List<Funcionario>? Funcionarios { get; set; }

    }
}
