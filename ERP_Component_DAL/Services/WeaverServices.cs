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
        
        private byte[] GetBytesFromIFormFile(IFormFile? formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return null;
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

        