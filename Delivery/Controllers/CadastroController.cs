using AgenciaViagem.Database;
using Delivery.Autenticacao;
using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.IO;

namespace Delivery.Controllers
{
    [SessionAuthorize(RoleAnyOf = "Admin")]
    public class CadastroController : Controller
    {
        [HttpGet]
        public IActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cliente(Cliente cliente)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarEmail = "SELECT COUNT(*) FROM Cliente WHERE Email = @Email";

                MySqlCommand cmdEmail = new MySqlCommand(verificarEmail, conn);

                cmdEmail.Parameters.AddWithValue("@Email", cliente.Email);

                int emailExiste = Convert.ToInt32(cmdEmail.ExecuteScalar());

                if (emailExiste > 0)
                {
                    ViewBag.Erro = "Este email já está cadastrado.";
                    return View(cliente);
                }

                string verificarCpf = "SELECT COUNT(*) FROM Cliente WHERE Cpf = @Cpf";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", cliente.Cpf);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";
                    return View(cliente);
                }

                string query = @"INSERT INTO Cliente
        (Nome, Email, Telefone, Endereco, Senha, Cpf)
        VALUES
        (@Nome, @Email, @Telefone, @Endereco, @Senha, @Cpf)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@Cpf", cliente.Cpf);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Sucesso");
        }

        public IActionResult Sucesso()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Entregador()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entregador(Entregador entregador)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string verificarCpf = "SELECT COUNT(*) FROM Entregador WHERE Cpf = @Cpf";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", entregador.Cpf);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";
                    return View(entregador);
                }

                string query = @"INSERT INTO Entregador
        (Nome, Telefone, Veiculo, Senha, Cpf)
        VALUES
        (@Nome, @Telefone, @Veiculo, @Senha, @Cpf)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", entregador.Nome);
                cmd.Parameters.AddWithValue("@Telefone", entregador.Telefone);
                cmd.Parameters.AddWithValue("@Veiculo", entregador.Veiculo);
                cmd.Parameters.AddWithValue("@Senha", entregador.Senha);
                cmd.Parameters.AddWithValue("@Cpf", entregador.Cpf);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("SucessoEntregador");
        }

        public IActionResult SucessoEntregador()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Prato()
        {
            Conexao conexao = new Conexao();
            List<SelectListItem> restaurantes = new List<SelectListItem>();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string query = "SELECT RestauranteId, Nome FROM Restaurante WHERE Ativo = TRUE";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    restaurantes.Add(new SelectListItem
                    {
                        Value = reader["RestauranteId"].ToString(),
                        Text = reader["Nome"].ToString()
                    });
                }
            }

            ViewBag.Restaurantes = restaurantes;

            return View();
        }

        [HttpPost]
        public IActionResult Prato(Prato prato, IFormFile? imagem)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                string? caminhoImagem = null;

                if (imagem != null && imagem.Length > 0)
                {
                    var extensao = Path.GetExtension(imagem.FileName);

                    var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

                    var pasta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "pratos"
                    );

                    Directory.CreateDirectory(pasta);

                    var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        imagem.CopyTo(stream);
                    }

                    caminhoImagem = Path.Combine("pratos", nomeArquivo)
                        .Replace("\\", "/");
                }
                if (string.IsNullOrEmpty(prato.Nome) || prato.Preco <= 0 || prato.RestauranteId <= 0)
                {
                    ViewBag.Erro = "Preencha todos os campos obrigatórios.";
                    return View(prato);
                }

                string verificar = @"SELECT COUNT(*) FROM Prato 
                                     WHERE Nome = @Nome AND RestauranteId = @RestauranteId";

                MySqlCommand cmdVerificar = new MySqlCommand(verificar, conn);
                cmdVerificar.Parameters.AddWithValue("@Nome", prato.Nome);
                cmdVerificar.Parameters.AddWithValue("@RestauranteId", prato.RestauranteId);

                int existe = Convert.ToInt32(cmdVerificar.ExecuteScalar());

                if (existe > 0)
                {
                    ViewBag.Erro = "Este prato já existe neste restaurante.";
                    return View(prato);
                }

                string query = @"INSERT INTO Prato
                (Nome, Descricao, Preco, RestauranteId, Disponivel, ImagemArquivo)
                VALUES
                (@Nome, @Descricao, @Preco, @RestauranteId, @Disponivel, @ImagemArquivo)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", prato.Nome);
                cmd.Parameters.AddWithValue("@Descricao", prato.Descricao);
                cmd.Parameters.AddWithValue("@Preco", prato.Preco);
                cmd.Parameters.AddWithValue("@RestauranteId", prato.RestauranteId);
                cmd.Parameters.AddWithValue("@Disponivel", prato.Disponivel);
                cmd.Parameters.AddWithValue("@ImagemArquivo", caminhoImagem);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("SucessoPrato");
        }
        public IActionResult SucessoPrato()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Restaurante()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Restaurante(Restaurante restaurante)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                if (string.IsNullOrEmpty(restaurante.Nome) ||
                    string.IsNullOrEmpty(restaurante.CNPJ) ||
                    string.IsNullOrEmpty(restaurante.Senha))
                {
                    ViewBag.Erro = "Preencha todos os campos obrigatórios.";
                    return View(restaurante);
                }

                string verificarCnpj = "SELECT COUNT(*) FROM Restaurante WHERE CNPJ = @CNPJ";

                MySqlCommand cmdCnpj = new MySqlCommand(verificarCnpj, conn);

                cmdCnpj.Parameters.AddWithValue("@CNPJ", restaurante.CNPJ);

                int cnpjExiste = Convert.ToInt32(cmdCnpj.ExecuteScalar());

                if (cnpjExiste > 0)
                {
                    ViewBag.Erro = "Este CNPJ já está cadastrado.";
                    return View(restaurante);
                }
                restaurante.Ativo = true;
                string query = @"INSERT INTO Restaurante
        (Nome, CNPJ, Endereco, Telefone, Senha, Ativo)
        VALUES
        (@Nome, @CNPJ, @Endereco, @Telefone, @Senha, @Ativo)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", restaurante.Nome);
                cmd.Parameters.AddWithValue("@CNPJ", restaurante.CNPJ);
                cmd.Parameters.AddWithValue("@Endereco", restaurante.Endereco);
                cmd.Parameters.AddWithValue("@Telefone", restaurante.Telefone);
                cmd.Parameters.AddWithValue("@Senha", restaurante.Senha);
                cmd.Parameters.AddWithValue("@Ativo", restaurante.Ativo);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("SucessoRestaurante");
        }

        public IActionResult SucessoRestaurante()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Admin(Admin admin)
        {
            Conexao conexao = new Conexao();

            using (MySqlConnection conn = conexao.GetConnection())
            {
                if (string.IsNullOrEmpty(admin.Nome) ||
                    string.IsNullOrEmpty(admin.Senha) ||
                    admin.Cpf <= 0)
                {
                    ViewBag.Erro = "Preencha todos os campos obrigatórios.";
                    return View(admin);
                }

                string verificarCpf = "SELECT COUNT(*) FROM Admin WHERE Cpf = @Cpf";

                MySqlCommand cmdCpf = new MySqlCommand(verificarCpf, conn);

                cmdCpf.Parameters.AddWithValue("@Cpf", admin.Cpf);

                int cpfExiste = Convert.ToInt32(cmdCpf.ExecuteScalar());

                if (cpfExiste > 0)
                {
                    ViewBag.Erro = "Este CPF já está cadastrado.";
                    return View(admin);
                }

                string query = @"INSERT INTO Admin
        (Nome, Senha, Cpf)
        VALUES
        (@Nome, @Senha, @Cpf)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", admin.Nome);
                cmd.Parameters.AddWithValue("@Senha", admin.Senha);
                cmd.Parameters.AddWithValue("@Cpf", admin.Cpf);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("SucessoAdmin");
        }

        public IActionResult SucessoAdmin()
        {
            return View();
        }
    }
}