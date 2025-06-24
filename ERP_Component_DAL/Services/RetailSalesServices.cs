using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace ERP_Component_DAL.Services
{
    public class RetailSalesServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;


        public RetailSalesServices(IConfiguration config)
        {
            this.configuration = config;
        }


        public List<QuotationModel> AddBillItemName()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.ItemName,I.ItemId,I.HSN,I.UnitOFMeasure, L.SellingPrice from Items I Join LotBatch L on I.ItemId=L.ItemId Where I.ItemType = 1 ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new QuotationModel()
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        ItemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                    });
                }

                return sun;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }


        }


        public void AddCustomerBill(QuotationModel quotation, List<QuotationModel> ItemLists)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = $"INSERT INTO RetailBillHeader (RetailCustomerID, GrossTotal) OUTPUT Inserted.RetailBillID VALUES (@RetailCustomerID, @GrossTotal)";

                cmd.Parameters.AddWithValue("@RetailCustomerID", quotation.RetailCustomerId);
            
                cmd.Parameters.AddWithValue("@GrossTotal", quotation.grossTotal);

                cmd.Connection = connection;

                Guid RetailBillID = (Guid)cmd.ExecuteScalar(); 
                                                                

                foreach (var item in ItemLists)
                {
                    string insertLineQuery = "INSERT INTO RetailBillLine (RetailBillID, ProductID, Quantity, MRP, DiscountRate, GST) " +
                                             "VALUES (@RetailBillID, @ProductID, @Quantity, @MRP, @DiscountRate, @GST)";

                    using (SqlCommand cmd1 = new SqlCommand(insertLineQuery, connection))
                    {
                        cmd1.Parameters.AddWithValue("@RetailBillID", RetailBillID);
                        cmd1.Parameters.AddWithValue("@ProductID", item.ProductID);
                        cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd1.Parameters.AddWithValue("@MRP", item.UnitPrice);
                        cmd1.Parameters.AddWithValue("@DiscountRate", item.discountRate);
                        cmd1.Parameters.AddWithValue("@GST", item.IGST);

                       
                        cmd1.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }



        //public void AddCustomerBill(QuotationModel quotation, List<QuotationModel> ItemLists)
        //{
        //    SqlConnection connection = null;

        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionString);


        //        string insertHeaderQuery = "";

        //        SqlCommand cmd = new SqlCommand(insertHeaderQuery, connection);


        //        // Insert Line Items
        //        foreach (var item in ItemLists)
        //        {
        //            string insertLineQuery = "INSERT INTO RetailBillLine (RetailBillID, ProductID, Quantity, MRP, DiscountRate, GST) " +
        //                                     "VALUES (@RetailBillID, @ProductID, @Quantity, @MRP, @DiscountRate, @GST)";

        //            using (SqlCommand cmd1 = new SqlCommand(insertLineQuery, connection))
        //            {
        //                cmd1.Parameters.AddWithValue("@RetailBillID", RetailBillID);
        //                cmd1.Parameters.AddWithValue("@ProductID", item.ProductID);
        //                cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
        //                cmd1.Parameters.AddWithValue("@MRP", item.UnitPrice);
        //                cmd1.Parameters.AddWithValue("@DiscountRate", item.discountRate);
        //                cmd1.Parameters.AddWithValue("@GST", item.IGST);

        //                connection.Open();
        //                cmd1.ExecuteNonQuery();
        //                connection.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (connection != null && connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}








        public Guid AddRetailCustomer(QuotationModel quotation)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    string spName = "AddRetailCustomer";

                    using (SqlCommand cmd = new SqlCommand(spName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerName", quotation.CustomerName);
                        cmd.Parameters.AddWithValue("@ContactNumber", quotation.ContactNumber);
                      
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetGuid(0);
                            }
                        }

                        throw new Exception("No RetailCustomerID returned.");
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
