using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class UserServices
    {
        private string connectionString;
        //private Microsoft.Extensions.Configuration.IConfiguration configuration1;

        public UserServices(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("DefaultConnectionString");
        }

        public List<Role> GetRoles()
        {
            try
            {
                List<Role> roles = new();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select * from Roles;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Role role = new Role
                                {
                                    roleId = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0,
                                    role = reader["Role"] != DBNull.Value ? (string)reader["Role"] : string.Empty,
                                    homePage = reader["HomePage"] != DBNull.Value ? (string)reader["HomePage"] : string.Empty,
                                    controllerName = reader["ControllerName"] != DBNull.Value ? (string)reader["controllerName"] : string.Empty
                                };
                                roles.Add(role);

                            }
                        }
                    }
                }
                return roles;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public User GetUserInfo(User users)
        {
            try
            {
                User user = new();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"select LoginID, UserName,Password,RoleId from LoginCredentials where UserName = @username And Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = users.userName;
                        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = users.password;
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    loginId = reader["LoginID"]!=DBNull.Value?(Guid)reader["LoginID"]:Guid.Empty,
                                    userName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty,
                                    password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty,
                                    role = reader["RoleId"] != DBNull.Value ? Convert.ToInt32(reader["RoleId"]) : 0
                                };

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

        public User GetUserName(Guid login)
        {
            try
            {
                User user = new();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = $"select UserName,Password, LoginID from LoginCredentials where LoginID = '{login}' ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                     
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    
                                    oldUserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty,
                                    oldPassword = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty,
                                    loginId = reader["LoginID"] != DBNull.Value?(Guid)reader["LoginID"]:Guid.Empty,
                                    
                                };

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


        public bool UpdateUsername(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                UPDATE LoginCredentials
                SET UserName = @UserName
                    
                WHERE LoginID = @LoginID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserName", user.userName);
                        cmd.Parameters.AddWithValue("@LoginID", user.loginId);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public bool UpdatePassword(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                UPDATE LoginCredentials
                SET Password = @Password
                    
                WHERE LoginID = @LoginID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Password", user.password);
                        cmd.Parameters.AddWithValue("@LoginID", user.loginId);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }


    }
}