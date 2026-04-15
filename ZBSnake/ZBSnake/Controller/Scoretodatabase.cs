using System;
using MySql.Data.MySqlClient;

namespace ZBSnake.Controller
{
    internal class Scoretodatabase
    {
        private const string Server = "localhost";
        private const string Port = "3306";
        private const string Database = "eredmenyek";
        private const string User = "root";
        private const string Password = "";

        private string ServerOnlyConnStr =>
            $"Server={Server};Port={Port};Uid={User};Pwd={Password};";

        private string ConnectionString =>
            $"Server={Server};Port={Port};Database={Database};Uid={User};Pwd={Password};";

        public Scoretodatabase()
        {
            CreateDatabaseIfNotExists();
            CreateTableIfNotExists();
        }

        private void CreateDatabaseIfNotExists()
        {
            using var conn = new MySqlConnection(ServerOnlyConnStr);
            conn.Open();

            string sql = $"CREATE DATABASE IF NOT EXISTS `{Database}` CHARACTER SET utf8mb4;";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private void CreateTableIfNotExists()
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS eredmenyek (
                    Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                    Versenyzonev VARCHAR(32) NOT NULL,
                    Score INT NOT NULL,
                    Datum DATETIME NOT NULL
                );";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public bool TrySaveScore(string nev, int score)
        {
            int best = GetHighScore();
            SaveScore(nev, score);
            return score >= best;
        }

        public int GetHighScore()
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string sql = "SELECT COALESCE(MAX(Score), 0) FROM eredmenyek;";
            using var cmd = new MySqlCommand(sql, conn);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void SaveScore(string nev, int score)
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string sql = @"INSERT INTO eredmenyek 
                           (Versenyzonev, Score, Datum) 
                           VALUES (@nev, @score, @datum);";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nev", nev);
            cmd.Parameters.AddWithValue("@score", score);
            cmd.Parameters.AddWithValue("@datum", DateTime.Now);

            cmd.ExecuteNonQuery();
        }
    }
}