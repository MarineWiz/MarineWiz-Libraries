using MySql.Data.MySqlClient;
using System.Text;

namespace SampleData
{
    /// <summary>
    /// MySql 관리용 내부 클래스
    /// </summary>
    internal class DatabaseHelper
    {
        private string connectInfo = string.Empty;

        public string Id { get; private set; }

        public string Pw { get; private set; }

        public string Db { get; private set; }

        public string Host { get; private set; }

        private DatabaseHelper()
        {
        }

        public static DatabaseHelper Instanace { get; } = new DatabaseHelper();

        public MySqlConnection CreateConnection()
        {
            var conn = new MySqlConnection(connectInfo);
            conn.Open();

            return conn;
        }

        public bool ConfigureDB(string host, string id, string pw)
             => ConfigureDB(host, id, pw, "");

        public bool ConfigureDB(string host, string id, string pw, string db)
        {
            StringBuilder sb = new StringBuilder(connectInfo);
            sb.Append("SERVER=").Append(host).Append("; ");
            sb.Append("UID=").Append(id).Append("; ");
            sb.Append("PASSWORD=").Append(pw).Append("; ");
            if (!string.IsNullOrEmpty(db))
                sb.Append("DATABASE=").Append(db).Append(";");

            connectInfo = sb.ToString();

            using (var connection = new MySqlConnection(connectInfo))
            {
                try
                {
                    connection.Open();

                    Id = id;
                    Pw = pw;
                    Host = host;
                    Db = db;

                    return true;
                }
                catch { return false; }
            }
        }
    }
}