using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MvcLoginApp.DAL
{
    public class UserDAL
    {
        private readonly string _connectionString;

        public UserDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LoginDbConnectionString");
        }

        public bool IsValidUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM dbo.Users WHERE Username=@Username AND Password=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Consider hashing the password in production
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return count == 1;
            }
        }

        public bool IsUserExists(string username)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM dbo.Users WHERE Username=@Username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return count > 0;
            }
        }

        public void RegisterUser(string username, string email, string password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO dbo.Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password); // Consider hashing the password in production
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}