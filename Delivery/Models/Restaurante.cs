namespace Delivery.Models
{
    public class Restaurante
    {
        public int? RestauranteId { get; set; }
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Senha { get; set; }
        public Boolean Ativo { get; set; }
    }
}
