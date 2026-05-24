using MySql.Data.MySqlClient;

namespace AgenciaViagem.Database
{
    public class Conexao
    {
        private readonly string connectionString = "server=localhost;port=3306;database=FoodDeliveryDB;user=root;password=12345678;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }

}