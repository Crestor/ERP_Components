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

        public User FindLoginCredentials(User users)
        {
            try
            {
                User user = new();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = @"SELECT EmployeeID, UserName, Password, RoleId FROM LoginCredentials WHERE UserName = @username AND Password = @password ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@username", users.userName);
                        cmd.Parameters.AddWithValue("@password", users.password);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    employeeId = reader["EmployeeID"]!=DBNull.Value?(Guid)reader["EmployeeID"]:Guid.Empty,
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

        public User GetUserName(Guid employeeId)
        {
            try
            {
                User user = new();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string query = $"select UserName,Password, EmployeeID from LoginCredentials where EmployeeID = '{employeeId}' ";

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
                                    employeeId = reader["EmployeeID"] != DBNull.Value?(Guid)reader["EmployeeID"]:Guid.Empty,
                                    
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
                    
                WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserName", user.userName);
                        cmd.Parameters.AddWithValue("@EmployeeID", user.employeeId);

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
                    
                WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Password", user.password);
                        cmd.Parameters.AddWithValue("@EmployeeID", user.employeeId);

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

        public User FindCenterID(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT CenterID FROM AssignedCenter WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {                        
                        cmd.Parameters.AddWithValue("@EmployeeID", user.employeeId);

                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user.assignedCenterID = reader["CenterID"] != DBNull.Value ? (Guid)reader["CenterID"] : Guid.Empty;
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return user;
        }
    }
}