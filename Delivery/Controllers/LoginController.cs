using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AgenciaViagem.Database;

namespace Delivery.Controllers
{
    public class LoginController : Controller
    {
        private readonly Conexao _conexao = new Conexao();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(string tipoLogin, string documento, string senha)
        {
            using (MySqlConnection conn = _conexao.GetConnection())
            {
                string query = "";
                MySqlCommand cmd;

                if (tipoLogin == "Admin")
                {
                    query = "SELECT * FROM Admin WHERE Cpf = @documento";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@documento", documento);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        ViewBag.Erro = "CPF não encontrado.";
                        return View("Index");
                    }

                    if (reader["Senha"].ToString() != senha)
                    {
                        ViewBag.Erro = "Senha incorreta.";
                        return View("Index");
                    }

                    return RedirectToAction("Index", "Home");
                }

                else if (tipoLogin == "Cliente")
                {
                    query = "SELECT * FROM Cliente WHERE Cpf = @documento";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@documento", documento);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        ViewBag.Erro = "CPF não encontrado.";
                        return View("Index");
                    }

                    if (reader["Senha"].ToString() != senha)
                    {
                        ViewBag.Erro = "Senha incorreta.";
                        return View("Index");
                    }

                    return RedirectToAction("Index", "Home");
                }

                else if (tipoLogin == "Entregador")
                {
                    query = "SELECT * FROM Entregador WHERE Cpf = @documento";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@documento", documento);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        ViewBag.Erro = "CPF não encontrado.";
                        return View("Index");
                    }

                    if (reader["Senha"].ToString() != senha)
                    {
                        ViewBag.Erro = "Senha incorreta.";
                        return View("Index");
                    }

                    return RedirectToAction("Index", "Home");
                }

                else if (tipoLogin == "Restaurante")
                {
                    query = "SELECT * FROM Restaurante WHERE CNPJ = @documento";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@documento", documento);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        ViewBag.Erro = "CNPJ não encontrado.";
                        return View("Index");
                    }

                    if (reader["Senha"].ToString() != senha)
                    {
                        ViewBag.Erro = "Senha incorreta.";
                        return View("Index");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Erro = "Tipo de login inválido.";
                return View("Index");
            }
        }
    }
}