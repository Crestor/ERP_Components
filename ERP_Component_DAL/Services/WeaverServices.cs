using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class WeaverServices
    {
        private readonly string connectionString;
        public WeaverServices(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnectionString");
        }

        public bool addWeaver(Weaver weaver)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddWorker", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@WorkerName", weaver.WeaverName);
                        cmd.Parameters.AddWithValue("@FirmName", weaver.Firm);
                        cmd.Parameters.AddWithValue("@ContactNumber", weaver.ContactNumber);
                        cmd.Parameters.AddWithValue("@EmailID", weaver.Email);
                        cmd.Parameters.AddWithValue("@Country", weaver.Country);
                        cmd.Parameters.AddWithValue("@State", weaver.State);
                        cmd.Parameters.AddWithValue("@City", weaver.City);
                        cmd.Parameters.AddWithValue("@Area", weaver.City);
                        cmd.Parameters.AddWithValue("@Pincode", weaver.Pincode);
                        cmd.Parameters.AddWithValue("@AddressLine1", weaver.Address);
                        cmd.Parameters.AddWithValue("@District", weaver.District);
                        cmd.Parameters.AddWithValue("@Street", weaver.City); 
                        cmd.Parameters.AddWithValue("@PANCardNumber", weaver.PANNumber);
                        cmd.Parameters.AddWithValue("@AadharNumber", weaver.AadharNumber);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Weaver> ViewWeaver()
        {
            List<Weaver> weavers = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT WorkerID, WorkerName, FirmName, ContactNumber FROM Workers", connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                weavers.Add(new Weaver
                                {
                                    WeaverId = reader["WorkerID"] != DBNull.Value ? (Guid)reader["WorkerID"] : Guid.Empty,
                                    WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty,
                                    Firm = reader["FirmName"] != DBNull.Value ? reader["FirmName"].ToString() : string.Empty,
                                    ContactNumber = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                }

                return weavers;
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw;
            }
        }

    }
}

        