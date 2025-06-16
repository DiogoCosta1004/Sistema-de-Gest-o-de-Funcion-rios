namespace BaseLibrary.Entities
{
    public class Pais : BaseEntity
    {
        public List<Cidade>? Cidades { get; set; }
    }
}
