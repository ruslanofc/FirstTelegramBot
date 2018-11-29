using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace telegramBot
{
    public class Student
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Groop { get; set; }
        public string Id { get; set; }
        public int SenderId { get; set; }
    }

    public class StudentService : IDataService
    {
        public void Delete(int id)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=student";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM student WHERE sender_id = @sender_id";
                    cmd.Parameters.AddWithValue("sender_id", id);
                    cmd.ExecuteNonQuery();
                }
            }           
        }

        public void Save(Student entity)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=student";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO student (name, age, groop, id, sender_id) VALUES (@name, @age, @groop, @id, @sender_id)";
                    cmd.Parameters.AddWithValue("name", entity.Name);
                    cmd.Parameters.AddWithValue("age", Convert.ToInt32(entity.Age));
                    cmd.Parameters.AddWithValue("groop",Convert.ToInt32( entity.Groop));
                    cmd.Parameters.AddWithValue("id", Convert.ToInt32(entity.Id));
                    cmd.Parameters.AddWithValue("sender_id",Convert.ToInt32(entity.SenderId));
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void Update(int id, Student entity)
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=student";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE student SET name= '" + entity.Name + "', age='"
                        + entity.Age.ToString() + "', groop='" + entity.Groop.ToString() + "', id='" + entity.Id.ToString()
                        + "',  WHERE sender_id='" + entity.SenderId.ToString() + "'";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void GetAll()
        {
            var connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=student";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT id_person FROM student", conn))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id_person FROM student";
                    NpgsqlDataReader reader = cmd.ExecuteReader();

                }
            }
        }
    }
}
