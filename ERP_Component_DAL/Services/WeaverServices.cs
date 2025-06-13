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
                    using (SqlCommand cmd = new SqlCommand("AddWeaver", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@WorkerName", weaver.WeaverName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirmName", weaver.Firm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ContactNumber", weaver.ContactNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmailID", weaver.Email ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Country", weaver.Country ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", weaver.State ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", weaver.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pincode", weaver.Pincode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddressLine1", weaver.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@District", weaver.District ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@PANCard", GetBytesFromIFormFile(weaver.DocPANCard) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PANCardNumber", weaver.PANNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharImage", GetBytesFromIFormFile(weaver.DocAadhar) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharNumber", weaver.AadharNumber ?? (object)DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private byte[] GetBytesFromIFormFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
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
        public Weaver GetWeaverDetailsById(Guid workerId)
        {
            Weaver weaver = new Weaver();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT w.WorkerName, w.FirmName, w.ContactNumber, w.EmailID, a.Country, a.State, a.City, a.Pincode, a.AddressLine1, a.District, d.PANCardNumber, d.AadharNumber
                         FROM Workers w JOIN Address a ON w.AddressID = a.AddressID
                    JOIN Documents d ON w.DocumentID = d.DocumentID where WorkerID =@WorkerID";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkerID", workerId);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                weaver.WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty;
                                weaver.Firm = reader["FirmName"] != DBNull.Value ? reader["FirmName"].ToString() : string.Empty;
                                weaver.ContactNumber = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : string.Empty;
                                weaver.Email = reader["EmailID"] != DBNull.Value ? reader["EmailID"].ToString() : string.Empty;
                                weaver.Country = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : string.Empty;
                                weaver.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : string.Empty;
                                weaver.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : string.Empty;
                                weaver.Pincode = reader["Pincode"] != DBNull.Value ? reader["Pincode"].ToString() : string.Empty;
                                weaver.Address = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : string.Empty;
                                weaver.District = reader["District"] != DBNull.Value ? reader["District"].ToString() : string.Empty;
                                weaver.PANNumber = reader["PANCardNumber"] != DBNull.Value ? reader["PANCardNumber"].ToString() : string.Empty;
                                weaver.AadharNumber = reader["AadharNumber"] != DBNull.Value ? reader["AadharNumber"].ToString() : string.Empty;



                            }
                        }
                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateWeaver(Weaver weaver)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateWeaver", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WorkerID", weaver.WeaverId);
                        cmd.Parameters.AddWithValue("@WorkerName", weaver.WeaverName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirmName", weaver.Firm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ContactNumber", weaver.ContactNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmailID", weaver.Email ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Country", weaver.Country ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", weaver.State ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", weaver.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pincode", weaver.Pincode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddressLine1", weaver.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@District", weaver.District ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@PANCard", GetBytesFromIFormFile(weaver.DocPANCard) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PANCardNumber", weaver.PANNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharImage", GetBytesFromIFormFile(weaver.DocAadhar) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharNumber", weaver.AadharNumber ?? (object)DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool DeleteWeaver(Guid weaverId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Workers WHERE WorkerID = @WorkerID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkerID", weaverId);
                        connection.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}

        