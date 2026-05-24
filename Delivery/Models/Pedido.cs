namespace Delivery.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public int RestauranteId { get; set; }
        public int EntregadorId { get; set; }
        public DateTime DataPedido { get; set; }
        public string? Status { get; set; }
        public Decimal ValorTotal { get; set; }
    }
}
