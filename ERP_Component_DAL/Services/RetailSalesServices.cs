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
                cmd.CommandText = $"select I.ItemName,I.ItemId,I.GSTRate/2 AS Gst, P.MRP, P.DiscountRate from Items I Join ProductPrice P on I.ItemId=P.ProductID Where I.ItemType = 1  ";

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
                        discountRate   =  reader["DiscountRate"] != DBNull.Value ? Convert.ToInt32(reader["DiscountRate"]) : 0,
                        CGST  =  reader["GSt"]!= DBNull.Value ? Convert.ToInt32(reader["Gst"]):0,

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


        public List<MonthlyRetailSales> GetCustomerRetailHistory(Guid customerId)
        {
            try
            {
                List<MonthlyRetailSales> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select it.ItemName ,rl.Quantity,rh.GrossTotal,rh.CreatedAt from RetailBillLine rl JOIN RetailBillHeader rh ON rl.RetailBillID = rh.RetailBillID Left Join items it ON rl.ProductID = it.ItemID Where rh.RetailCustomerID = '{customerId}' Order By rh.CreatedAt Desc";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new MonthlyRetailSales
                    {
                        ProductName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),
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

         
       
        public MonthlyRetailSales GetCustomerName(Guid customerId)
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CustomerName From RetailCustomers Where RetailCustomerID = '{customerId}'";

                cmd.Connection = connection;
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

        public List<MonthlyRetailSales> SearchCustomersByContact(string term)
        {
            try
            {
                List<MonthlyRetailSales> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    string query = @"
        SELECT TOP 10 RetailCustomerId, CustomerName, ContactNumber
        FROM RetailCustomers
        WHERE ContactNumber LIKE @term + '%'
        ORDER BY ContactNumber";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@term", term);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sales.Add(new MonthlyRetailSales
                                {
                                    CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                                    RetailId = reader["RetailCustomerId"] != DBNull.Value ? (Guid)reader["RetailCustomerId"] : Guid.Empty,
                                    ContactNumber = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty
                                });
                            }
                        }
                    }
                }
                return sales;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public RetailItemModel CustomerBillAddressData()
        {
            try
            {
                RetailItemModel name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select D.CenterName, D.CenterCode, A.Country, A.State, A.City, A.Area, A.Pincode, A.AddressLine1, A.AddressLine2, A.District, A.Street from DistributionCenter D join Address A on D.AddressId= A.AddressID where D.CenterId= '12BC8BE2-C59A-409B-9DBE-9869A2102C16'";
                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name.CenterName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;
                    name.CenterCode = reader["CenterCode"] != DBNull.Value ? (string)reader["CenterCode"] : string.Empty;
                    name.Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty;
                    name.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    name.City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                    name.Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty;
                    name.Pincode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;
                    name.AddressLine1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    name.AddressLine2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty;
                    name.District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty;
                    name.Street = reader["Street"] != DBNull.Value ? (string)reader["Street"] : string.Empty;
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
        public List<RetailItemModel> GetRetailCustomerBillData(Guid RetailCustomerId)
        {
            try
            {
                List<RetailItemModel> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"select I.ItemName ,L.Quantity ,L.MRP from RetailBillLine L join  Items I on I.ItemId=L.ProductID where L.RetailBillID='{RetailBillID}'";
                cmd.CommandText = $"select C.CustomerName, H.GrossTotal,H.NetTotal,H.GST,I.ItemName,R.Quantity ,R.MRP from RetailBillLine R join Items I on I.ItemId = R.ProductID Join RetailBillHeader H on R.RetailBillID = H.RetailBillID join RetailCustomers C on C.RetailCustomerID=H.RetailCustomerID where H.RetailCustomerID = '{RetailCustomerId}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new RetailItemModel
                    {
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        MRP = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        NetTotal = reader["NetTotal"] != DBNull.Value ? Convert.ToDecimal(reader["NetTotal"]) : 0m,
                        GST = reader["GST"] != DBNull.Value ? Convert.ToDecimal(reader["GST"]) : 0m,
                        //date = reader["CreatedAT"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedAT"]) : default,
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



    }
}
