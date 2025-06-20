﻿using System;
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

                    string query = @"select UserName,Password,RoleId from LoginCredentials where UserName = @username And Password = @password ";

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
    }
}