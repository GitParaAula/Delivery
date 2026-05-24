namespace Delivery.Models
{
    public class Carrinho
    {
        public int PratoId { get; set; }
        public string Nome { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public int RestauranteId { get; set; }
        public string? ImagemArquivo { get; set; }
        public decimal TotalItem => PrecoUnitario * Quantidade;
    }
}