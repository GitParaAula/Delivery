using AgenciaViagem.Database;
using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Delivery.Models;
using Delivery.Autenticacao;

namespace Delivery.Controllers
{
    public class ListarController : Controller
    {
        public IActionResult Restaurante()
        {
            List<Restaurante> restaurantes = new List<Restaurante>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Restaurante";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Restaurante restaurante = new Restaurante();

                    restaurante.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    restaurante.Nome = reader["Nome"].ToString();
                    restaurante.CNPJ = reader["CNPJ"].ToString();
                    restaurante.Endereco = reader["Endereco"].ToString();
                    restaurante.Telefone = reader["Telefone"].ToString();
                    restaurante.Senha = reader["Senha"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    restaurantes.Add(restaurante);
                }
            }

            return View(restaurantes);
        }
        public IActionResult Cardapio(int? restauranteId)
        {
            if (restauranteId == null)
            {
                ViewBag.Erro = "Selecione o restaurante antes de visualizar o cardápio!";

                return View(new List<Prato>());
            }

            List<Prato> pratos = new List<Prato>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"SELECT * FROM Prato
                         WHERE RestauranteId = @RestauranteId
                         AND Disponivel = TRUE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Prato prato = new Prato();

                    prato.PratoId = Convert.ToInt32(reader["PratoId"]);
                    prato.Nome = reader["Nome"].ToString();
                    prato.Descricao = reader["Descricao"].ToString();
                    prato.Preco = Convert.ToDecimal(reader["Preco"]);
                    prato.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    prato.Disponivel = Convert.ToBoolean(reader["Disponivel"]);

                    pratos.Add(prato);
                }
            }

            return View(pratos);
        }
        public static class CarrinhoStorage
        {
            public static List<Carrinho> Itens = new List<Carrinho>();
            public static int? RestauranteAtual = null;
        }

        public IActionResult AdicionarCarrinho(int pratoId, int quantidade)
        {
            Conexao conexao = new Conexao();

            Prato prato = null;

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Prato WHERE PratoId = @Id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", pratoId);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    prato = new Prato
                    {
                        PratoId = Convert.ToInt32(reader["PratoId"]),
                        Nome = reader["Nome"].ToString(),
                        Preco = Convert.ToDecimal(reader["Preco"]),
                        RestauranteId = Convert.ToInt32(reader["RestauranteId"])
                    };
                }
            }

            if (prato == null)
                return RedirectToAction("Cardapio", new { restauranteId = prato.RestauranteId });

            // regra restaurante único
            if (CarrinhoStorage.RestauranteAtual != null &&
                CarrinhoStorage.RestauranteAtual != prato.RestauranteId)
            {
                TempData["Erro"] = "Só é possível pedir em uma loja de cada vez.";
                return RedirectToAction("Cardapio", new { restauranteId = prato.RestauranteId });
            }

            CarrinhoStorage.RestauranteAtual = prato.RestauranteId;

            var item = CarrinhoStorage.Itens
                .FirstOrDefault(x => x.PratoId == pratoId);

            if (item != null)
            {
                item.Quantidade += quantidade;
            }
            else
            {
                CarrinhoStorage.Itens.Add(new Carrinho
                {
                    PratoId = prato.PratoId,
                    Nome = prato.Nome,
                    PrecoUnitario = prato.Preco,
                    Quantidade = quantidade,
                    RestauranteId = prato.RestauranteId
                });
            }

            TempData["Sucesso"] = "Item adicionado ao carrinho";
            return RedirectToAction("Cardapio", new { restauranteId = prato.RestauranteId });
        }

        public IActionResult Carrinho()
        {
            return View(CarrinhoStorage.Itens);
        }

        public IActionResult RemoverItem(int pratoId)
        {
            var item = CarrinhoStorage.Itens.FirstOrDefault(x => x.PratoId == pratoId);

            if (item != null)
                CarrinhoStorage.Itens.Remove(item);

            if (CarrinhoStorage.Itens.Count == 0)
                CarrinhoStorage.RestauranteAtual = null;

            return RedirectToAction("Carrinho");
        }

        public IActionResult AtualizarQuantidade(int pratoId, int quantidade)
        {
            var item = CarrinhoStorage.Itens.FirstOrDefault(x => x.PratoId == pratoId);

            if (item != null)
            {
                item.Quantidade = quantidade < 1 ? 1 : quantidade;
            }

            return RedirectToAction("Carrinho");
        }

        public IActionResult LimparCarrinho()
        {
            CarrinhoStorage.Itens.Clear();
            CarrinhoStorage.RestauranteAtual = null;

            return RedirectToAction("Carrinho");
        }

        [HttpPost]
        public IActionResult FinalizarPedido()
        {
            var tipoUsuario = HttpContext.Session.GetString(SessionKeys.UserRole);
            if (!string.Equals(tipoUsuario, "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensagemErro"] = "Apenas clientes podem fazer pedidos.";
                return RedirectToAction("Carrinho");
            }
            int? clienteId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            if (clienteId == null)
            {
                TempData["MensagemErro"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Carrinho");
            }

            Conexao conexao = new Conexao();
            int entregadorId = 0;
            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT EntregadorId FROM Entregador WHERE Ativo = TRUE";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                var entregadores = new List<int>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entregadores.Add(Convert.ToInt32(reader["EntregadorId"]));
                    }
                }
                if (entregadores.Count == 0)
                {
                    TempData["MensagemErro"] = "Nenhum entregador disponível no momento.";
                    return RedirectToAction("Carrinho");
                }
                var random = new Random();
                entregadorId = entregadores[random.Next(entregadores.Count)];
            }

            if (CarrinhoStorage.Itens.Count == 0)
            {
                TempData["MensagemErro"] = "Carrinho vazio.";
                return RedirectToAction("Carrinho");
            }
            int restauranteId = CarrinhoStorage.Itens.First().RestauranteId;
            decimal valorTotal = CarrinhoStorage.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

            int pedidoId = 0;
            using (MySqlConnection conn = conexao.GetConnection())
            {
                string insertPedido = @"INSERT INTO Pedido (ClienteId, RestauranteId, EntregadorId, ValorTotal, Status, DataPedido)
                                VALUES (@ClienteId, @RestauranteId, @EntregadorId, @ValorTotal, 'Pendente', NOW());
                                SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(insertPedido, conn);
                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);
                cmd.Parameters.AddWithValue("@EntregadorId", entregadorId);
                cmd.Parameters.AddWithValue("@ValorTotal", valorTotal);

                pedidoId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var item in CarrinhoStorage.Itens)
                {
                    string insertItem = @"INSERT INTO PedidoItem (PedidoId, PratoId, Quantidade, PrecoUnitario)
                                  VALUES (@PedidoId, @PratoId, @Quantidade, @PrecoUnitario)";
                    MySqlCommand cmdItem = new MySqlCommand(insertItem, conn);
                    cmdItem.Parameters.AddWithValue("@PedidoId", pedidoId);
                    cmdItem.Parameters.AddWithValue("@PratoId", item.PratoId);
                    cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                    cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                    cmdItem.ExecuteNonQuery();
                }
            }

            CarrinhoStorage.Itens.Clear();
            CarrinhoStorage.RestauranteAtual = null;

            TempData["MensagemSucesso"] = "Pedido realizado com sucesso!";
            return RedirectToAction("Carrinho");
        }

    }
}