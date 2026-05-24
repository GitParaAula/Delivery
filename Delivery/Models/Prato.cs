namespace Delivery.Models
{
    public class Prato
    {
        public int PratoId { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public Decimal Preco {  get; set; }
        public int RestauranteId { get; set; }
        public Boolean Disponivel { get; set; }
        public string? ImagemArquivo { get; set; }
        public bool Ativo { get; set; }
    }
}
