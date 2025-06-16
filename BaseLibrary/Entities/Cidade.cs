namespace BaseLibrary.Entities
{
    public class Cidade : BaseEntity
    {
        public Pais? Pais { get; set; }
        public int PaisId { get; set; }

        public List<Town> Towns { get; set; }
    }
}
