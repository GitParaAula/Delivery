namespace Delivery.Models
{
    public class PedidoItem
    {
        public int PedidoItemId { get; set; }
        public int PedidoId { get; set; }
        public int PratoId { get; set; }
        public int Quantidade { get; set; }
        public Decimal PrecoUnitario { get; set; }
    }
}
