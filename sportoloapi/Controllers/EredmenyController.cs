using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sportoloapi.Models;

namespace sportoloapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;database=sportolo13b;";

        [HttpGet]
        public List<Eredmeny> GetEredmenyek()
        {
            List<Eredmeny> eredmenyek = new List<Eredmeny>();

            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            string sql = "SELECT * FROM eredmeny";

            var cmd = new MySqlCommand(sql, connection);
            var data = cmd.ExecuteReader();

            while (data.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description"),
                    ResultTime = data.GetDateTime("ResultTime"),
                    UpdateTime = data.GetDateTime("UpdateTime"),
                    SportoloId = data.GetInt32("SportoloId")
                };

                eredmenyek.Add(eredmeny);
            }

            connection.Close();

            return eredmenyek;
        }
        [HttpGet("{id}")]
        public object GetEredmeny(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT * FROM `eredmeny`
                   WHERE `Id` = @Id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", id);

            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                var eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("Competition"),
                    Description = data.GetString("Description"),
                    ResultTime = data.GetDateTime("ResultTime"),
                    UpdateTime = data.GetDateTime("UpdateTime"),
                    SportoloId = data.GetInt32("SportoloId")
                };

                connection.Close();

                return eredmeny;
            }

            connection.Close();

            return new
            {
                message = "Nincs ilyen eredmény."
            };
        }

        [HttpPost]
        public object AddEredmeny([FromBody] AddNewEredmenyDto addNewEredmenyDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"INSERT INTO `eredmeny`
            (`Competition`, `Description`, `ResultTime`, `UpdateTime`, `SportoloId`)
            VALUES
            (@Competition, @Description, @ResultTime, @UpdateTime, @SportoloId)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Competition", addNewEredmenyDto.Competition);
            cmd.Parameters.AddWithValue("@Description", addNewEredmenyDto.Description);
            cmd.Parameters.AddWithValue("@ResultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@SportoloId", addNewEredmenyDto.SportoloId);

            cmd.ExecuteNonQuery();

            connection.Close();

            return new
            {
                message = "Sikeres felvétel.",
                result = addNewEredmenyDto
            };
        }

        [HttpPut("{id}")]
        public object UpdateEredmeny(int id, [FromBody] AddNewEredmenyDto eredmenyDto)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"UPDATE `eredmeny`
                   SET
                       `Competition` = @Competition,
                       `Description` = @Description,
                       `ResultTime` = @ResultTime,
                       `UpdateTime` = @UpdateTime,
                       `SportoloId` = @SportoloId
                   WHERE `Id` = @Id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Competition", eredmenyDto.Competition);
            cmd.Parameters.AddWithValue("@Description", eredmenyDto.Description);
            cmd.Parameters.AddWithValue("@ResultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@UpdateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@SportoloId", eredmenyDto.SportoloId);

            int rowsAffected = cmd.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected == 0)
            {
                return new
                {
                    message = "Nincs ilyen eredmény."
                };
            }

            return new
            {
                message = "Sikeres módosítás.",
                result = eredmenyDto
            };
        }
        [HttpDelete("{id}")]
        public object DeleteEredmeny(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"DELETE FROM `eredmeny`
                   WHERE `Id` = @Id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            connection.Close();

            if (rowsAffected == 0)
            {
                return new
                {
                    message = "Nincs ilyen eredmény."
                };
            }

            return new
            {
                message = "Sikeres törlés."
            };
        }
        [HttpGet("sportolo/{id}")]
        public object GetSportolo(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            connection.Open();

            string sql = @"SELECT name, email
                   FROM sportolo
                   WHERE id = @Id";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", id);

            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                var sportolo = new
                {
                    Name = data.GetString("name"),
                    Email = data.GetString("email")
                };

                connection.Close();

                return sportolo;
            }

            connection.Close();

            return new
            {
                message = "Nincs ilyen sportoló."
            };
        }
    }
}


