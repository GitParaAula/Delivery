using AgenciaViagem.Database;
using Delivery.Autenticacao;
using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Delivery.Controllers
{
    [SessionAuthorize(RoleAnyOf = "Admin")]
    public class DesativarController : Controller
    {
        public IActionResult PerguntaCliente()
        {
            return View();
        }

        public IActionResult ClienteAtivo()
        {
            List<Cliente> clientes = new List<Cliente>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Cliente WHERE Ativo = TRUE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente();

                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nome = reader["Nome"].ToString();
                    cliente.Cpf = Convert.ToInt32(reader["Cpf"]);
                    cliente.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    clientes.Add(cliente);
                }
            }

            return View(clientes);
        }

        public IActionResult ClienteDesativo()
        {
            List<Cliente> clientes = new List<Cliente>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Cliente WHERE Ativo = FALSE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente();

                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nome = reader["Nome"].ToString();
                    cliente.Cpf = Convert.ToInt32(reader["Cpf"]);
                    cliente.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    clientes.Add(cliente);
                }
            }

            return View(clientes);
        }

        public IActionResult DesativarCliente(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Cliente
                                 SET Ativo = FALSE
                                 WHERE ClienteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Cliente desativado com sucesso!";

            return RedirectToAction("PerguntaCliente");
        }

        public IActionResult AtivarCliente(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Cliente
                                 SET Ativo = TRUE
                                 WHERE ClienteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Cliente ativado com sucesso!";

            return RedirectToAction("PerguntaCliente");
        }
        public IActionResult PerguntaRestaurante()
        {
            return View();
        }

        public IActionResult RestauranteAtivo()
        {
            List<Restaurante> restaurantes = new List<Restaurante>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Restaurante WHERE Ativo = TRUE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Restaurante restaurante = new Restaurante();

                    restaurante.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    restaurante.Nome = reader["Nome"].ToString();
                    restaurante.CNPJ = reader["CNPJ"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    restaurantes.Add(restaurante);
                }
            }

            return View(restaurantes);
        }

        public IActionResult RestauranteDesativo()
        {
            List<Restaurante> restaurantes = new List<Restaurante>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Restaurante WHERE Ativo = FALSE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Restaurante restaurante = new Restaurante();

                    restaurante.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    restaurante.Nome = reader["Nome"].ToString();
                    restaurante.CNPJ = reader["CNPJ"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    restaurantes.Add(restaurante);
                }
            }

            return View(restaurantes);
        }

        public IActionResult DesativarRestaurante(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Restaurante
                         SET Ativo = FALSE
                         WHERE RestauranteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Restaurante desativado com sucesso!";

            return RedirectToAction("PerguntaRestaurante");
        }

        public IActionResult AtivarRestaurante(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Restaurante
                         SET Ativo = TRUE
                         WHERE RestauranteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Restaurante ativado com sucesso!";

            return RedirectToAction("PerguntaRestaurante");
        }
        public IActionResult Restaurantes()
        {
            List<Restaurante> restaurantes = new List<Restaurante>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Restaurante WHERE Ativo = TRUE";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    restaurantes.Add(new Restaurante
                    {
                        RestauranteId = Convert.ToInt32(reader["RestauranteId"]),
                        Nome = reader["Nome"].ToString()
                    });
                }
            }

            return View(restaurantes);
        }
        public IActionResult SelecionarRestaurante(int id)
        {
            HttpContext.Session.SetInt32("RestauranteId", id);
            return RedirectToAction("PerguntaPrato");
        }
        public IActionResult PerguntaPrato()
        {
            int restauranteId = HttpContext.Session.GetInt32("RestauranteId") ?? 0;

            Conexao conexao = new Conexao();

            bool existePrato = false;

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"SELECT COUNT(*) FROM Prato 
                         WHERE RestauranteId = @RestauranteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                existePrato = count > 0;
            }

            ViewBag.ExistePrato = existePrato;

            return View();
        }
        public IActionResult PratoAtivo()
        {
            int restauranteId = HttpContext.Session.GetInt32("RestauranteId") ?? 0;

            List<Prato> pratos = new List<Prato>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"SELECT * FROM Prato 
                         WHERE Ativo = TRUE 
                         AND RestauranteId = @RestauranteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pratos.Add(new Prato
                    {
                        PratoId = Convert.ToInt32(reader["PratoId"]),
                        Nome = reader["Nome"].ToString(),
                        ImagemArquivo = reader["ImagemArquivo"]?.ToString()
                    });
                }
            }

            return View(pratos);
        }
        public IActionResult PratoDesativo()
        {
            int restauranteId = HttpContext.Session.GetInt32("RestauranteId") ?? 0;

            List<Prato> pratos = new List<Prato>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"SELECT * FROM Prato 
                         WHERE Ativo = FALSE 
                         AND RestauranteId = @RestauranteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RestauranteId", restauranteId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pratos.Add(new Prato
                    {
                        PratoId = Convert.ToInt32(reader["PratoId"]),
                        Nome = reader["Nome"].ToString(),
                        ImagemArquivo = reader["ImagemArquivo"]?.ToString()
                    });
                }
            }

            return View(pratos);
        }
        public IActionResult DesativarPrato(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "UPDATE Prato SET Ativo = FALSE WHERE PratoId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Prato desativado com sucesso!";
            return RedirectToAction("PratoAtivo");
        }
        public IActionResult AtivarPrato(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "UPDATE Prato SET Ativo = TRUE WHERE PratoId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Prato ativado com sucesso!";
            return RedirectToAction("PratoDesativo");
        }
        public IActionResult PerguntaEntregador()
        {
            return View();
        }

        public IActionResult EntregadorAtivo()
        {
            List<Entregador> entregadores = new List<Entregador>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Entregador WHERE Ativo = TRUE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Entregador entregador = new Entregador();

                    entregador.EntregadorId = Convert.ToInt32(reader["EntregadorId"]);
                    entregador.Nome = reader["Nome"].ToString();
                    entregador.Cpf = Convert.ToInt32(reader["Cpf"]);
                    entregador.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    entregadores.Add(entregador);
                }
            }

            return View(entregadores);
        }

        public IActionResult EntregadorDesativo()
        {
            List<Entregador> entregadores = new List<Entregador>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Entregador WHERE Ativo = FALSE";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Entregador entregador = new Entregador();

                    entregador.EntregadorId = Convert.ToInt32(reader["EntregadorId"]);
                    entregador.Nome = reader["Nome"].ToString();
                    entregador.Cpf = Convert.ToInt32(reader["Cpf"]);
                    entregador.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    entregadores.Add(entregador);
                }
            }

            return View(entregadores);
        }

        public IActionResult DesativarEntregador(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Entregador
                         SET Ativo = FALSE
                         WHERE EntregadorId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Entregador desativado com sucesso!";
            return RedirectToAction("PerguntaEntregador");
        }

        public IActionResult AtivarEntregador(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Entregador
                         SET Ativo = TRUE
                         WHERE EntregadorId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Entregador ativado com sucesso!";
            return RedirectToAction("PerguntaEntregador");
        }
        public IActionResult PerguntaAdmin()
        {
            return View();
        }

        public IActionResult AdminAtivo()
        {
            List<Admin> admins = new List<Admin>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Admin WHERE Ativo = TRUE";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Admin admin = new Admin();

                    admin.AdminId = Convert.ToInt32(reader["AdminId"]);
                    admin.Nome = reader["Nome"].ToString();
                    admin.Cpf = Convert.ToInt32(reader["Cpf"]);
                    admin.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    admins.Add(admin);
                }
            }

            return View(admins);
        }

        public IActionResult AdminDesativo()
        {
            List<Admin> admins = new List<Admin>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Admin WHERE Ativo = FALSE";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Admin admin = new Admin();

                    admin.AdminId = Convert.ToInt32(reader["AdminId"]);
                    admin.Nome = reader["Nome"].ToString();
                    admin.Cpf = Convert.ToInt32(reader["Cpf"]);
                    admin.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    admins.Add(admin);
                }
            }

            return View(admins);
        }

        public IActionResult DesativarAdmin(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Admin
                         SET Ativo = FALSE
                         WHERE AdminId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Admin desativado com sucesso!";
            return RedirectToAction("PerguntaAdmin");
        }

        public IActionResult AtivarAdmin(int id)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"UPDATE Admin
                         SET Ativo = TRUE
                         WHERE AdminId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            TempData["Mensagem"] = "Admin ativado com sucesso!";
            return RedirectToAction("PerguntaAdmin");
        }
    }
}