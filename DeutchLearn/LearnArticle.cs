using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeutchLearn
{
    public class LearnArticle
    {
        public static IEnumerable<ArticleBegin> GetWordByArticle(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"select id, germany, russian, article from articlebegin where id = @id";
                return conn.Query<ArticleBegin>(sql, new { id });
            }
        }

        public static string FindTranslateWord(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"SELECT russian FROM from articlebegin where id = @id";
                return conn.QueryFirstOrDefault(sql, new { id });
            }
        }

        public static int GetRandomId()
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = "SELECT id FROM articlebegin ORDER BY RANDOM() LIMIT 1";
                return conn.ExecuteScalar<int>(sql);
            }
        }
    }
}
