using Milestone.Models;
using System.Data.SqlClient;

namespace Milestone.Services
{
    public class UsersDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Milestone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool AddUser(UserModel user)
        {
            bool success = false;
            string sqlStatment = "INSERT INTO dbo.users (FirstName, LastName, Sex, Age, State, Email, UserName, Password) VALUES (@FirstName, @LastName, @Sex, @Age, @State, @Email, @UserName, @Password) ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);

                command.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar, 50).Value = user.FirstName;
                command.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar, 50).Value = user.LastName;
                command.Parameters.Add("@Sex", System.Data.SqlDbType.VarChar, 10).Value = user.Sex;
                command.Parameters.Add("@Age", System.Data.SqlDbType.Int).Value = user.Age;
                command.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 50).Value = user.State;
                command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 100).Value = user.Email;
                command.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 50).Value = user.UserName;
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = user.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    success = true;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return success;
            }
        }
        public bool FindUserByNameAndPassword(UserModel user)
        {

            bool success = false;

            string sqlStatement = "SELECT * FROM dbo.Users WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = user.UserName;
                command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40).Value = user.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            //return true/false to determine if the match was successful/unsuccessful
            return success;
        }

    }
}