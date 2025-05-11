using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;

namespace ERP_Component_DAL.Services
{
    public class UserServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;
        //private Microsoft.Extensions.Configuration.IConfiguration configuration1;

        public UserServices(IConfiguration config)
        {
            this.configuration = config;
        }

        //public UserServices(Microsoft.Extensions.Configuration.IConfiguration configuration)
        //{
        //    this.configuration1 = configuration;
        //}

        public List<User> Authentication(User users)
        {
            try
            {
                List<User> user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,RoleId from LoginCredentials where UserName = @username And Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = users.password;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.Add(new User
                                {
                                    userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty,
                                    password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty,
                                    role = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                                });

                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public User HandleAdmin(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,UserRole,FirstName,LastName from Users where UserName = @username And Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = users.password;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["UserRole"] != DBNull.Value ? (int)reader["UserRole"] : 0;
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }

        public User HandleManager(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,Role from Users where UserName = @username ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["Role"] != DBNull.Value ? (int)reader["Role"] : 0;


                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }

        public User HandleUsers(User users)
        {
            try
            {
                User user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select UserName,Password,RoleId from LoginCredentials where UserName = @username ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                user.userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty;
                                user.password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty;
                                user.role = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0;


                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching route details", ex);
            }
        }


    }
}




