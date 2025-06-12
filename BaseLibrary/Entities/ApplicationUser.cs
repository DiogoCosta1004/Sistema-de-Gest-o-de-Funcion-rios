namespace BaseLibrary.Entities
{
    public class ApplicationUser 
    {
        public int Id { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
