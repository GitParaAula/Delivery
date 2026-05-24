namespace Delivery.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public int Cpf { get; set; }
        public bool Ativo { get; set; }
    }
}
