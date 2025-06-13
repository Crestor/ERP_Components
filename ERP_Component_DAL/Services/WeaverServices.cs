using System;
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
    public class WeaverServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;


        public WeaverServices(IConfiguration config)
        {
            this.configuration = config;
        }
        public bool addWeaver(Weaver weaver)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;



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
    
    }
}

        