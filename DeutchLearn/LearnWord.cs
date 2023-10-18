using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace DeutchLearn
{
    public class LearnWord
    {

        //public static FirstLevel GetRandomWord(int id)
        //{
        //    using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
        //    {
        //        string sql = $"select russian from a1word where id = @id";
        //        return conn.QueryFirstOrDefault<FirstLevel>(sql,new { id });
        //    }
        //}

        public static int GetRandomId()
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = "SELECT id FROM a1word ORDER BY RANDOM() LIMIT 1";
                return conn.ExecuteScalar<int>(sql);
            }
        }

        public static IEnumerable<FirstLevel> GetWordById(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"select germany, russian from a1word where id = @id";
                return conn.Query<FirstLevel>(sql, new { id });
            }
        }

        public static void Insert(RepeatWord repeatWord)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"insert into repeatword (id,chatid, wordde,wordru,worddate) values (@id,@chatid,@wordde,@wordru,@worddate)";
                conn.Execute(sql, new {id = repeatWord.id, chatid = repeatWord.chatid, wordde = repeatWord.wordde, wordru = repeatWord.wordru, worddate = repeatWord.worddate });
            }
        }
        public static bool WordExistsInRepeat(string word)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM repeatword WHERE wordde = @word";
                int count = conn.ExecuteScalar<int>(sql, new { word });
                return count > 0;
            }
        }

        public static int GetMaxId()
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"SELECT MAX(id) FROM repeatword";
                return conn.ExecuteScalar<int>(sql);
            }
        }

        public static (string wordde, string wordru) GetOldDateWord(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"SELECT wordde, wordru FROM repeatword where chatid =@id AND worddate = (SELECT MIN(worddate) FROM repeatword where chatid =@id)";
                return conn.QueryFirstOrDefault<(string, string)>(sql, new { id });
            }
        }

        public static bool UpdateDateWord(int id)
        {
            using (var conn = new NpgsqlConnection(ConfigDb.SqlConnectionString))
            {
                string sql = $"update repeatword set worddate = TO_CHAR(NOW(), 'YYYY-MM-DD HH24:MI:SS') where chatid =@id AND worddate = (SELECT MIN(worddate) FROM repeatword where chatid =@id)";
                conn.Execute(sql, new { id });
                return true;
            }
        }
    }
}   
    
