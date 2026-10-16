
using MySql.Data.MySqlClient;

namespace RepairManagement
{
    public static class Database
    {
        private static readonly string connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=repair_management;" +
            "Uid=root;" +
            "Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
