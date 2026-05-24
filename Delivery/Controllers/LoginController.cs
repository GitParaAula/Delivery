using Delivery.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using AgenciaViagem.Database;
using Delivery.Autenticacao;

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

                    if (reader["Ativo"] != DBNull.Value && !Convert.ToBoolean(reader["Ativo"]))
                    {
                        ViewBag.Erro = "Este registro não está ativo!";
                        return View("Index");
                    }

                    HttpContext.Session.SetInt32(SessionKeys.UserId, Convert.ToInt32(reader["AdminId"]));
                    HttpContext.Session.SetString(SessionKeys.UserName, reader["Nome"].ToString());
                    HttpContext.Session.SetString(SessionKeys.UserRole, "Admin");

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

                    if (reader["Ativo"] != DBNull.Value && !Convert.ToBoolean(reader["Ativo"]))
                    {
                        ViewBag.Erro = "Este registro não está ativo!";
                        return View("Index");
                    }

                    HttpContext.Session.SetInt32(SessionKeys.UserId, Convert.ToInt32(reader["ClienteId"]));
                    HttpContext.Session.SetString(SessionKeys.UserName, reader["Nome"].ToString());
                    HttpContext.Session.SetString(SessionKeys.UserRole, "Cliente");

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

                    if (reader["Ativo"] != DBNull.Value && !Convert.ToBoolean(reader["Ativo"]))
                    {
                        ViewBag.Erro = "Este registro não está ativo!";
                        return View("Index");
                    }

                    HttpContext.Session.SetInt32(SessionKeys.UserId, Convert.ToInt32(reader["EntregadorId"]));
                    HttpContext.Session.SetString(SessionKeys.UserName, reader["Nome"].ToString());
                    HttpContext.Session.SetString(SessionKeys.UserRole, "Entregador");

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

                    if (reader["Ativo"] != DBNull.Value && !Convert.ToBoolean(reader["Ativo"]))
                    {
                        ViewBag.Erro = "Este registro não está ativo!";
                        return View("Index");
                    }

                    HttpContext.Session.SetInt32(SessionKeys.UserId, Convert.ToInt32(reader["RestauranteId"]));
                    HttpContext.Session.SetString(SessionKeys.UserName, reader["Nome"].ToString());
                    HttpContext.Session.SetString(SessionKeys.UserRole, "Restaurante");

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Erro = "Tipo de login inválido.";
                return View("Index");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
    }
}