using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Milestone.Models;

namespace Milestone.Services
{
    public class GameDAO : IGameService
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Milestone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public List<GameDTO> GetSavedGames()
        {
            List<GameDTO> savedGames = new List<GameDTO>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlStatement = "SELECT * FROM Games";

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GameDTO game = new GameDTO
                    {
                        GameId = (int)reader["gameId"],
                        UserId = (string)reader["UserId"],
                        time = (string)reader["time"],
                        date = (string)reader["date"],
                        gameData = (string)reader["gameData"]
                    };

                    savedGames.Add(game);
                }
        }

            return savedGames;
        }

        public GameDTO GetGameById(int gameId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlStatement = "SELECT * FROM Games WHERE gameId = @GameId";

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@GameId", gameId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    GameDTO game = new GameDTO
                    {
                        GameId = (int)reader["gameId"],
                        UserId = (string)reader["UserId"],
                        time = (string)reader["time"],
                        date = (string)reader["date"],
                        gameData = (string)reader["gameData"]
                    };

                    return game;
                }
            }

            return null;
        }

        public void SaveGame(GameDTO game)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlStatement = "INSERT INTO [Games] (UserId, time, date, gameData) " +
                                      "VALUES (@UserId, @time, @date, @gameData)";

                SqlCommand command = new SqlCommand(sqlStatement, connection);
        
                
                command.Parameters.AddWithValue("@UserId",  game.UserId);
                command.Parameters.AddWithValue("@time", game.time);
                command.Parameters.AddWithValue("@date", game.date);
                command.Parameters.AddWithValue("@gameData", game.gameData);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void DeleteGame(int gameId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlStatement = "DELETE FROM Games WHERE gameId = @GameId";

                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@GameId", gameId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}