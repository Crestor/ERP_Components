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
                cmd.CommandText = $"select I.ItemName,I.ItemId,I.HSN,I.UnitOFMeasure, P.MRP from Items I Join ProductPrice P on I.ItemId=P.ProductID Where I.ItemType = 1  ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new QuotationModel()
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        SellingPrice = reader["MRP"] != DBNull.Value ? (decimal)reader["MRP"] : 0m,
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
            
                cmd.Parameters.AddWithValue("@GrossTotal", quotation.GrossTotal);

                cmd.Connection = connection;

                Guid RetailBillID = (Guid)cmd.ExecuteScalar(); 
                                                                

                foreach (var item in ItemLists)
                {
                    string insertLineQuery = "INSERT INTO RetailBillLine (RetailBillID, ProductID, Quantity, MRP, DiscountRate, GST) " +
                                             "VALUES (@RetailBillID, @ProductID, @Quantity, @MRP, @DiscountRate, @GST)";

                    using (SqlCommand cmd1 = new SqlCommand(insertLineQuery, connection))
                    {
                        cmd1.Parameters.AddWithValue("@RetailBillID", RetailBillID);
                        cmd1.Parameters.AddWithValue("@ProductID", item.ItemId);
                        cmd1.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd1.Parameters.AddWithValue("@MRP", item.SellingPrice);
                        cmd1.Parameters.AddWithValue("@DiscountRate", item.discountRate);
                        cmd1.Parameters.AddWithValue("@GST", item.IGST);
                        //cmd1.Parameters.AddWithValue("@GrossTotal", item.TotalAmount);
                        //cmd1.Parameters.AddWithValue("@NetTotal", item.TaxableAmount);

                       
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




        public List<QuotationModel> ViewCustomerBill()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select H.RetailBillID, C.CustomerName,C.ContactNumber,H.GrossTotal,H.NetTotal from  RetailBillHeader H join RetailCustomers C on H.RetailCustomerID=C.RetailCustomerID";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new QuotationModel()
                    {

                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,
                        TaxableAmount = reader["NetTotal"] != DBNull.Value ? (decimal)reader["NetTotal"] : 0m,
                        ContactNO = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty,

                        RetailBillID = reader["RetailBillID"] != DBNull.Value ? (Guid)reader["RetailBillID"] : Guid.Empty,
                        //UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
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


        public List<RetailItemModel> GetCustomerRetailData(Guid RetailBillID)
        {
            try
            {
                List<RetailItemModel> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.ItemName ,L.Quantity ,L.MRP, L.CreatedAT from RetailBillLine L join  Items I on I.ItemId=L.ProductID where L.RetailBillID='{RetailBillID}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new RetailItemModel
                    {
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        MRP = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                        date = reader["CreatedAT"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedAT"]) : default,
                    });
                }

                return sales;

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

         public QuotationModel GetCustomerNameForCompair (string ContactNO)
        {
            try
            {
                QuotationModel name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT CustomerName FROM RetailCustomers WHERE ContactNumber = '{ContactNO}'";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(); 
                while (reader.Read())
                {

                    name.CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;

                }


                return name;

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
        public QuotationModel GetCustomerName(Guid RetailBillID)
        {
            try
            {
                QuotationModel name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select rc.CustomerName From RetailCustomers rc Join RetailBillHeader rh On rc.RetailCustomerID = rh.RetailCustomerID Where rh.RetailBillID = '{RetailBillID}'";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;

                }


                return name;

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





    }
}
