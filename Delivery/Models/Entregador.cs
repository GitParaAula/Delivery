namespace Delivery.Models
{
    public class Entregador
    {
        public int EntregadorId { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Veiculo { get; set; }
        public string? Senha { get; set; }
        public int Cpf {  get; set; }
        public Boolean Ativo { get; set; }
    }
}
