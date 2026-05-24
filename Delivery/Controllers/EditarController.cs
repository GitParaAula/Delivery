using AgenciaViagem.Database;
using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Delivery.Autenticacao;

namespace Delivery.Controllers
{
    public class EditarController : Controller
    {
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarCliente()
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
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefone = reader["Telefone"].ToString();
                    cliente.Endereco = reader["Endereco"].ToString();
                    cliente.Senha = reader["Senha"].ToString();
                    cliente.Cpf = Convert.ToInt32(reader["Cpf"]);
                    cliente.DataCadastro = Convert.ToDateTime(reader["DataCadastro"]);

                    clientes.Add(cliente);
                }
            }

            return View(clientes);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpGet]
        public IActionResult EditarCliente(int id)
        {
            Cliente cliente = new Cliente();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Cliente WHERE ClienteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nome = reader["Nome"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefone = reader["Telefone"].ToString();
                    cliente.Endereco = reader["Endereco"].ToString();
                    cliente.Senha = reader["Senha"].ToString();
                    cliente.Cpf = Convert.ToInt32(reader["Cpf"]);
                }
            }

            return View(cliente);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpPost]
        public IActionResult EditarCliente(Cliente cliente)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarEmail = @"SELECT COUNT(*) 
                                          FROM Cliente 
                                          WHERE Email = @Email 
                                          AND ClienteId != @ClienteId";

                MySqlCommand cmdEmail = new MySqlCommand(verificarEmail, conn);

                cmdEmail.Parameters.AddWithValue("@Email", cliente.Email);
                cmdEmail.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);

                int emailExiste = Convert.ToInt32(cmdEmail.ExecuteScalar());

                if (emailExiste > 0)
                {
                    ViewBag.Erro = "Este email já está cadastrado.";
                    return View(cliente);
                }

                string verificarCpf = @"SELECT COUNT(*) 
                                        FROM Cliente 
                                        WHERE Cpf = @Cpf 
                                        AND ClienteId != @ClienteId";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                cmdCpf.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";
                    return View(cliente);
                }

                string query = @"UPDATE Cliente
                                 SET
                                 Nome = @Nome,
                                 Email = @Email,
                                 Telefone = @Telefone,
                                 Endereco = @Endereco,
                                 Senha = @Senha,
                                 Cpf = @Cpf
                                 WHERE ClienteId = @ClienteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);
                cmd.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);

                cmd.ExecuteNonQuery();
            }

            TempData["Sucesso"] = "Dados salvos com sucesso";

            return RedirectToAction("EditarCliente", new { id = cliente.ClienteId });
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarRestaurante()
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
                    restaurante.Endereco = reader["Endereco"].ToString();
                    restaurante.Telefone = reader["Telefone"].ToString();
                    restaurante.Senha = reader["Senha"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    restaurantes.Add(restaurante);
                }
            }

            return View(restaurantes);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpGet]
        public IActionResult Restaurante(int id)
        {
            Restaurante restaurante = new Restaurante();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Restaurante WHERE RestauranteId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    restaurante.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    restaurante.Nome = reader["Nome"].ToString();
                    restaurante.CNPJ = reader["CNPJ"].ToString();
                    restaurante.Endereco = reader["Endereco"].ToString();
                    restaurante.Telefone = reader["Telefone"].ToString();
                    restaurante.Senha = reader["Senha"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);
                }
            }

            return View("EditarRestaurante", restaurante);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpPost]
        public IActionResult Restaurante(Restaurante restaurante)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarCnpj = @"SELECT COUNT(*) 
                                 FROM Restaurante 
                                 WHERE CNPJ = @CNPJ 
                                 AND RestauranteId != @RestauranteId";

                MySqlCommand cmdCnpj = new MySqlCommand(verificarCnpj, conn);

                cmdCnpj.Parameters.AddWithValue("@CNPJ", restaurante.CNPJ);
                cmdCnpj.Parameters.AddWithValue("@RestauranteId", restaurante.RestauranteId);

                int cnpjExiste = Convert.ToInt32(cmdCnpj.ExecuteScalar());

                if (cnpjExiste > 0)
                {
                    ViewBag.Erro = "Este CNPJ já está cadastrado.";
                    return View("EditarRestaurante", restaurante);
                }
                restaurante.Ativo = true;
                string query = @"UPDATE Restaurante
                         SET
                         Nome = @Nome,
                         CNPJ = @CNPJ,
                         Telefone = @Telefone,
                         Endereco = @Endereco,
                         Senha = @Senha,
                         Ativo = @Ativo
                         WHERE RestauranteId = @RestauranteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", restaurante.Nome);
                cmd.Parameters.AddWithValue("@CNPJ", restaurante.CNPJ);
                cmd.Parameters.AddWithValue("@Telefone", restaurante.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", restaurante.Endereco);
                cmd.Parameters.AddWithValue("@Senha", restaurante.Senha);
                cmd.Parameters.AddWithValue("@Ativo", restaurante.Ativo);
                cmd.Parameters.AddWithValue("@RestauranteId", restaurante.RestauranteId);

                cmd.ExecuteNonQuery();
            }

            TempData["Sucesso"] = "Dados salvos com sucesso";

            return RedirectToAction("Restaurante", new { id = restaurante.RestauranteId });
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarRestauranteP()
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
                    restaurante.Endereco = reader["Endereco"].ToString();
                    restaurante.Telefone = reader["Telefone"].ToString();
                    restaurante.Senha = reader["Senha"].ToString();
                    restaurante.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    restaurantes.Add(restaurante);
                }
            }

            return View(restaurantes);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarPratos(int restauranteId)
        {
            List<Prato> pratos = new List<Prato>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"SELECT * FROM Prato
                         WHERE RestauranteId = @RestauranteId";

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

                    if (reader["ImagemArquivo"] != DBNull.Value)
                    {
                        prato.ImagemArquivo = reader["ImagemArquivo"].ToString();
                    }

                    pratos.Add(prato);
                }
            }

            return View(pratos);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpGet]
        public IActionResult Prato(int id)
        {
            Prato prato = new Prato();

            List<SelectListItem> restaurantes = new List<SelectListItem>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string queryRestaurantes = "SELECT * FROM Restaurante WHERE Ativo = TRUE";

                MySqlCommand cmdRestaurantes = new MySqlCommand(queryRestaurantes, conn);

                MySqlDataReader readerRestaurantes = cmdRestaurantes.ExecuteReader();

                while (readerRestaurantes.Read())
                {
                    restaurantes.Add(new SelectListItem
                    {
                        Text = readerRestaurantes["Nome"].ToString(),
                        Value = readerRestaurantes["RestauranteId"].ToString()
                    });
                }

                readerRestaurantes.Close();

                string query = "SELECT * FROM Prato WHERE PratoId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    prato.PratoId = Convert.ToInt32(reader["PratoId"]);
                    prato.Nome = reader["Nome"].ToString();
                    prato.Descricao = reader["Descricao"].ToString();
                    prato.Preco = Convert.ToDecimal(reader["Preco"]);
                    prato.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);
                    prato.Disponivel = Convert.ToBoolean(reader["Disponivel"]);

                    if (reader["ImagemArquivo"] != DBNull.Value)
                    {
                        prato.ImagemArquivo = reader["ImagemArquivo"].ToString();
                    }
                }
            }

            ViewBag.Restaurantes = restaurantes;

            return View("EditarPrato", prato);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpPost]
        public IActionResult Prato(Prato prato, IFormFile imagem)
        {
            List<SelectListItem> restaurantes = new List<SelectListItem>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string queryRestaurantes = "SELECT * FROM Restaurante WHERE Ativo = TRUE";

                MySqlCommand cmdRestaurantes = new MySqlCommand(queryRestaurantes, conn);

                MySqlDataReader readerRestaurantes = cmdRestaurantes.ExecuteReader();

                while (readerRestaurantes.Read())
                {
                    restaurantes.Add(new SelectListItem
                    {
                        Text = readerRestaurantes["Nome"].ToString(),
                        Value = readerRestaurantes["RestauranteId"].ToString()
                    });
                }

                readerRestaurantes.Close();

                string imagemArquivo = prato.ImagemArquivo;

                if (imagem != null && imagem.Length > 0)
                {
                    string nomeArquivo = Guid.NewGuid().ToString()
                                          + Path.GetExtension(imagem.FileName);

                    string caminho = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/pratos",
                        nomeArquivo
                    );

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        imagem.CopyTo(stream);
                    }

                    imagemArquivo = "pratos/" + nomeArquivo;
                }
                else
                {
                    string buscarImagem = @"SELECT ImagemArquivo 
                                    FROM Prato 
                                    WHERE PratoId = @Id";

                    MySqlCommand cmdImagem = new MySqlCommand(buscarImagem, conn);

                    cmdImagem.Parameters.AddWithValue("@Id", prato.PratoId);

                    var resultado = cmdImagem.ExecuteScalar();

                    if (resultado != null)
                    {
                        imagemArquivo = resultado.ToString();
                    }
                }

                string query = @"UPDATE Prato
                         SET
                         Nome = @Nome,
                         Descricao = @Descricao,
                         Preco = @Preco,
                         RestauranteId = @RestauranteId,
                         Disponivel = @Disponivel,
                         ImagemArquivo = @ImagemArquivo
                         WHERE PratoId = @PratoId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", prato.Nome);
                cmd.Parameters.AddWithValue("@Descricao", prato.Descricao);
                cmd.Parameters.AddWithValue("@Preco", prato.Preco);
                cmd.Parameters.AddWithValue("@RestauranteId", prato.RestauranteId);
                cmd.Parameters.AddWithValue("@Disponivel", prato.Disponivel);
                cmd.Parameters.AddWithValue("@ImagemArquivo", imagemArquivo);
                cmd.Parameters.AddWithValue("@PratoId", prato.PratoId);

                cmd.ExecuteNonQuery();

                prato.ImagemArquivo = imagemArquivo;
            }

            ViewBag.Restaurantes = restaurantes;

            ViewBag.Sucesso = "Alterações salvas com sucesso";

            return View("EditarPrato", prato);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarEntregador()
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
                    entregador.Telefone = reader["Telefone"].ToString();
                    entregador.Veiculo = reader["Veiculo"].ToString();
                    entregador.Senha = reader["Senha"].ToString();
                    entregador.Cpf = Convert.ToInt32(reader["Cpf"]);
                    entregador.Ativo = Convert.ToBoolean(reader["Ativo"]);

                    entregadores.Add(entregador);
                }
            }

            return View(entregadores);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpGet]
        public IActionResult Entregador(int id)
        {
            Entregador entregador = new Entregador();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Entregador WHERE EntregadorId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    entregador.EntregadorId = Convert.ToInt32(reader["EntregadorId"]);
                    entregador.Nome = reader["Nome"].ToString();
                    entregador.Telefone = reader["Telefone"].ToString();
                    entregador.Veiculo = reader["Veiculo"].ToString();
                    entregador.Senha = reader["Senha"].ToString();
                    entregador.Cpf = Convert.ToInt32(reader["Cpf"]);
                    entregador.Ativo = Convert.ToBoolean(reader["Ativo"]);
                }
            }

            return View("EditarEntregador", entregador);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpPost]
        public IActionResult Entregador(Entregador entregador)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarCpf = @"SELECT COUNT(*)
                                FROM Entregador
                                WHERE Cpf = @Cpf
                                AND EntregadorId != @EntregadorId";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", entregador.Cpf);
                cmdCpf.Parameters.AddWithValue("@EntregadorId", entregador.EntregadorId);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";

                    return View("EditarEntregador", entregador);
                }
                entregador.Ativo = true;
                string query = @"UPDATE Entregador
                         SET
                         Nome = @Nome,
                         Telefone = @Telefone,
                         Veiculo = @Veiculo,
                         Senha = @Senha,
                         Cpf = @Cpf,
                         Ativo = @Ativo
                         WHERE EntregadorId = @EntregadorId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", entregador.Nome);
                cmd.Parameters.AddWithValue("@Telefone", entregador.Telefone);
                cmd.Parameters.AddWithValue("@Veiculo", entregador.Veiculo);
                cmd.Parameters.AddWithValue("@Senha", entregador.Senha);
                cmd.Parameters.AddWithValue("@Cpf", entregador.Cpf);
                cmd.Parameters.AddWithValue("@Ativo", entregador.Ativo);
                cmd.Parameters.AddWithValue("@EntregadorId", entregador.EntregadorId);

                cmd.ExecuteNonQuery();
            }

            ViewBag.Sucesso = "Dados salvos com sucesso";

            return View("EditarEntregador", entregador);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        public IActionResult ListarAdmin()
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
                    admin.Senha = reader["Senha"].ToString();
                    admin.Cpf = Convert.ToInt32(reader["Cpf"]);

                    admins.Add(admin);
                }
            }

            return View(admins);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpGet]
        public IActionResult Admin(int id)
        {
            Admin admin = new Admin();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT * FROM Admin WHERE AdminId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    admin.AdminId = Convert.ToInt32(reader["AdminId"]);
                    admin.Nome = reader["Nome"].ToString();
                    admin.Senha = reader["Senha"].ToString();
                    admin.Cpf = Convert.ToInt32(reader["Cpf"]);
                }
            }

            return View("EditarAdmin", admin);
        }
        [SessionAuthorize(RoleAnyOf = "Admin")]
        [HttpPost]
        public IActionResult Admin(Admin admin)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarCpf = @"SELECT COUNT(*)
                        FROM Admin
                        WHERE Cpf = @Cpf
                        AND AdminId != @AdminId";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", admin.Cpf);
                cmdCpf.Parameters.AddWithValue("@AdminId", admin.AdminId);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";

                    return View("EditarAdmin", admin);
                }

                string query = @"UPDATE Admin
                 SET
                 Nome = @Nome,
                 Senha = @Senha,
                 Cpf = @Cpf
                 WHERE AdminId = @AdminId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", admin.Nome);
                cmd.Parameters.AddWithValue("@Senha", admin.Senha);
                cmd.Parameters.AddWithValue("@Cpf", admin.Cpf);
                cmd.Parameters.AddWithValue("@AdminId", admin.AdminId);

                cmd.ExecuteNonQuery();
            }

            ViewBag.Sucesso = "Dados salvos com sucesso";

            return View("EditarAdmin", admin);
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        public IActionResult ListarPedido()
        {
            List<dynamic> pedidos = new List<dynamic>();

            int? clienteId = HttpContext.Session.GetInt32(SessionKeys.UserId);

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"
SELECT
    Pedido.PedidoId,
    Pedido.ClienteId,
    Pedido.RestauranteId,
    Pedido.EntregadorId,
    Pedido.DataPedido,
    Pedido.Status,
    Pedido.ValorTotal,
    Restaurante.Nome AS RestauranteNome,
    Entregador.Nome AS EntregadorNome
FROM Pedido
INNER JOIN Restaurante
    ON Pedido.RestauranteId = Restaurante.RestauranteId
LEFT JOIN Entregador
    ON Pedido.EntregadorId = Entregador.EntregadorId
WHERE Pedido.ClienteId = @ClienteId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ClienteId", clienteId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new
                    {
                        PedidoId = Convert.ToInt32(reader["PedidoId"]),
                        ClienteId = Convert.ToInt32(reader["ClienteId"]),
                        RestauranteId = Convert.ToInt32(reader["RestauranteId"]),
                        EntregadorId = reader["EntregadorId"] != DBNull.Value
                            ? Convert.ToInt32(reader["EntregadorId"])
                            : 0,

                        DataPedido = Convert.ToDateTime(reader["DataPedido"]),
                        Status = reader["Status"].ToString(),
                        ValorTotal = Convert.ToDecimal(reader["ValorTotal"]),
                        RestauranteNome = reader["RestauranteNome"].ToString(),
                        EntregadorNome = reader["EntregadorNome"].ToString()
                    });
                }
            }

            ViewBag.Pedidos = pedidos;

            return View();
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        [HttpGet]
        public IActionResult EditarPedido(int id)
        {
            Pedido pedido = new Pedido();

            List<SelectListItem> entregadores = new List<SelectListItem>();

            ViewBag.RestauranteNome = "";

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string queryEntregadores = @"
SELECT * FROM Entregador
WHERE Ativo = TRUE";

                MySqlCommand cmdEntregadores =
                    new MySqlCommand(queryEntregadores, conn);

                MySqlDataReader readerEntregadores =
                    cmdEntregadores.ExecuteReader();

                while (readerEntregadores.Read())
                {
                    entregadores.Add(new SelectListItem
                    {
                        Text = readerEntregadores["Nome"].ToString(),
                        Value = readerEntregadores["EntregadorId"].ToString()
                    });
                }

                readerEntregadores.Close();

                string query = @"
SELECT
    Pedido.*,
    Restaurante.Nome AS RestauranteNome
FROM Pedido
INNER JOIN Restaurante
    ON Pedido.RestauranteId = Restaurante.RestauranteId
WHERE Pedido.PedidoId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    pedido.PedidoId = Convert.ToInt32(reader["PedidoId"]);
                    pedido.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    pedido.RestauranteId = Convert.ToInt32(reader["RestauranteId"]);

                    pedido.EntregadorId =
                        reader["EntregadorId"] != DBNull.Value
                        ? Convert.ToInt32(reader["EntregadorId"])
                        : 0;

                    pedido.DataPedido =
                        Convert.ToDateTime(reader["DataPedido"]);

                    pedido.Status = reader["Status"].ToString();

                    pedido.ValorTotal =
                        Convert.ToDecimal(reader["ValorTotal"]);

                    ViewBag.RestauranteNome =
                        reader["RestauranteNome"].ToString();
                }
            }

            ViewBag.Entregadores = entregadores;

            return View(pedido);
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        [HttpPost]
        public IActionResult EditarPedido(Pedido pedido)
        {
            List<SelectListItem> entregadores =
                new List<SelectListItem>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string queryEntregadores = @"
SELECT * FROM Entregador
WHERE Ativo = TRUE";

                MySqlCommand cmdEntregadores =
                    new MySqlCommand(queryEntregadores, conn);

                MySqlDataReader readerEntregadores =
                    cmdEntregadores.ExecuteReader();

                while (readerEntregadores.Read())
                {
                    entregadores.Add(new SelectListItem
                    {
                        Text = readerEntregadores["Nome"].ToString(),
                        Value = readerEntregadores["EntregadorId"].ToString()
                    });
                }

                readerEntregadores.Close();

                string query = @"
UPDATE Pedido
SET
    EntregadorId = @EntregadorId
WHERE PedidoId = @PedidoId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@EntregadorId",
                    pedido.EntregadorId
                );

                cmd.Parameters.AddWithValue(
                    "@PedidoId",
                    pedido.PedidoId
                );

                cmd.ExecuteNonQuery();
            }

            ViewBag.Entregadores = entregadores;

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"
SELECT
    Restaurante.Nome AS RestauranteNome
FROM Pedido
INNER JOIN Restaurante
    ON Pedido.RestauranteId = Restaurante.RestauranteId
WHERE Pedido.PedidoId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", pedido.PedidoId);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ViewBag.RestauranteNome =
                        reader["RestauranteNome"].ToString();
                }
            }

            ViewBag.Sucesso = "Dados salvos com sucesso";

            return View(pedido);
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        public IActionResult ListarPedidoItem(int pedidoId)
        {
            List<dynamic> itens = new List<dynamic>();

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"
SELECT
    PedidoItem.PedidoItemId,
    PedidoItem.PedidoId,
    PedidoItem.PratoId,
    PedidoItem.Quantidade,
    PedidoItem.PrecoUnitario,
    Prato.Nome AS PratoNome,
    Prato.ImagemArquivo
FROM PedidoItem
INNER JOIN Prato
    ON PedidoItem.PratoId = Prato.PratoId
WHERE PedidoItem.PedidoId = @PedidoId";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    itens.Add(new
                    {
                        PedidoItemId = Convert.ToInt32(reader["PedidoItemId"]),
                        PedidoId = Convert.ToInt32(reader["PedidoId"]),
                        PratoId = Convert.ToInt32(reader["PratoId"]),
                        Quantidade = Convert.ToInt32(reader["Quantidade"]),
                        PrecoUnitario = Convert.ToDecimal(reader["PrecoUnitario"]),
                        PratoNome = reader["PratoNome"].ToString(),
                        ImagemArquivo = reader["ImagemArquivo"].ToString()
                    });
                }
            }

            ViewBag.Itens = itens;
            ViewBag.PedidoId = pedidoId;

            return View();
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        [HttpGet]
        public IActionResult EditarItem(int id)
        {
            PedidoItem item = new PedidoItem();

            ViewBag.PratoNome = "";

            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = @"
SELECT
    PedidoItem.*,
    Prato.Nome AS PratoNome
FROM PedidoItem
INNER JOIN Prato
    ON PedidoItem.PratoId = Prato.PratoId
WHERE PedidoItem.PedidoItemId = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    item.PedidoItemId = Convert.ToInt32(reader["PedidoItemId"]);
                    item.PedidoId = Convert.ToInt32(reader["PedidoId"]);
                    item.PratoId = Convert.ToInt32(reader["PratoId"]);
                    item.Quantidade = Convert.ToInt32(reader["Quantidade"]);
                    item.PrecoUnitario = Convert.ToDecimal(reader["PrecoUnitario"]);

                    ViewBag.PratoNome = reader["PratoNome"].ToString();
                }
            }

            return View(item);
        }
        [SessionAuthorize(RoleAnyOf = "Admin,Cliente")]
        [HttpPost]
        public IActionResult EditarItem(PedidoItem item)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string atualizarItem = @"
        UPDATE PedidoItem
        SET Quantidade = @Quantidade
        WHERE PedidoItemId = @PedidoItemId";

                MySqlCommand cmd = new MySqlCommand(atualizarItem, conn);

                cmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                cmd.Parameters.AddWithValue("@PedidoItemId", item.PedidoItemId);

                cmd.ExecuteNonQuery();

                string buscarTotal = @"
        SELECT SUM(Quantidade * PrecoUnitario)
        FROM PedidoItem
        WHERE PedidoId = @PedidoId";

                MySqlCommand cmdTotal = new MySqlCommand(buscarTotal, conn);

                cmdTotal.Parameters.AddWithValue("@PedidoId", item.PedidoId);

                decimal novoTotal = Convert.ToDecimal(cmdTotal.ExecuteScalar());

                string atualizarPedido = @"
        UPDATE Pedido
        SET ValorTotal = @ValorTotal
        WHERE PedidoId = @PedidoId";

                MySqlCommand cmdPedido = new MySqlCommand(atualizarPedido, conn);

                cmdPedido.Parameters.AddWithValue("@ValorTotal", novoTotal);
                cmdPedido.Parameters.AddWithValue("@PedidoId", item.PedidoId);

                cmdPedido.ExecuteNonQuery();
            }

            ViewBag.Sucesso = "Dados salvos com sucesso";

            return View("EditarItem", item);
        }
    }

}