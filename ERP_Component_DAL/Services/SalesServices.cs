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
    public class SalesServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;
        private readonly string _connectionString;

        public SalesServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = configuration.GetConnectionString("DefaultConnectionString");
        }





        public List<QuotationModel> GetProductDetails()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT i.ItemName, i.ItemId, i.HSN, i.UnitOFMeasure, pp.SellingPrice 
                    FROM Items i JOIN ProductPrice pp ON pp.ProductID = i.ItemId 
                    WHERE i.ItemType = 1 ORDER BY i.ItemName";

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

        public List<QuotationViewModel.Customer> GetCustomersList()
        {
            connection = new SqlConnection(_connectionString);
            var Customers = new List<QuotationViewModel.Customer>();
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT CustomerID, CustomerName FROM Customers";
                cmd.CommandTimeout = 300;
                cmd.Connection = connection;
                connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customers.Add(
                        new QuotationViewModel.Customer
                        {                           
                            CustomerID = reader["CustomerID"] != DBNull.Value ? reader.GetGuid("CustomerID") : Guid.Empty,
                            CustomerName = reader["CustomerName"] != DBNull.Value ? reader.GetString("CustomerName") : "No Name",
                         }
                    );
                }
                return Customers;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                connection.Close();
            }

            return Customers;
        }

        public bool SaveQuotationProduct(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = @"INSERT INTO QuotationProduct 
                                    ([QuotationID],[Quantity],[SellingPrice], [TaxableAmount],[discountRate],
                                    [DiscountAmount],[CGST],[SGST],[IGST],[TotalAmount], [ProductID]) 
                                    VALUES (@QuotationID,@Quantity,@SellingPrice,@TaxableAmount,@discountRate,
                                    @DiscountAmount,@CGST,@SGST,@IGST,@TotalAmount, @ProductID)";

                cmd.Parameters.AddWithValue("@QuotationID", Aq.QuotationID);
                cmd.Parameters.AddWithValue("@Quantity", Aq.Quantity);
                cmd.Parameters.AddWithValue("@SellingPrice", Aq.SellingPrice);
                cmd.Parameters.AddWithValue("@TaxableAmount", Aq.TaxableAmount);
                cmd.Parameters.AddWithValue("@discountRate", Aq.discountRate);
                cmd.Parameters.AddWithValue("@DiscountAmount", Aq.DiscountAmount);
                cmd.Parameters.AddWithValue("@CGST", Aq.CGST);
                cmd.Parameters.AddWithValue("@SGST", Aq.SGST);
                cmd.Parameters.AddWithValue("@IGST", Aq.IGST);
                cmd.Parameters.AddWithValue("@TotalAmount", Aq.TotalAmount);
                cmd.Parameters.AddWithValue("@ProductID", Aq.ItemId);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public Guid AddQuotaDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"INSERT INTO CustomerQuotation ([QuotationSeries], [status]) " + "OUTPUT INSERTED.QuotationID " +
                                  "VALUES (@QuotationSeries, 'open' )";

                cmd1.Parameters.AddWithValue("@QuotationSeries", Aq.QuotationSeries ?? (object)DBNull.Value);

                cmd1.Connection = connection;
                connection.Open();
                Guid quotationID = (Guid)cmd1.ExecuteScalar();

                return quotationID;
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


        public bool finalEditQuota(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = @" UPDATE TermCondition SET DeliveryTerms = @DeliveryTerms, PaymentTerm = @PaymentTerm, Other = @Other WHERE TermConditionID = @TermConditionID";


                cmd.Parameters.AddWithValue("@DeliveryTerms", Aq.DeliveryTerms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentTerm", Aq.PaymentTerm ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Other", Aq.Other ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TermConditionID", Aq.TermConditionID);


                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public bool editAmount(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"
                    UPDATE CustomerQuotation SET 
                        GrossTotal=@GrossTotal,
                        CustomerName = @CustomerName
                    WHERE QuotationId = @QuotationID";
                cmd.Parameters.AddWithValue("@CustomerName", Aq.CustomerName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@GrossTotal", Aq.GrossTotal);
                cmd.Parameters.AddWithValue("@QuotationID", Aq.QuotationID);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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


        public bool updateQuotaDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);

                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"INSERT INTO TermCondition ([PaymentTerm],[DeliveryTerms],[Other]) OUTPUT INSERTED.TermConditionId VALUES (@PaymentTerm,@DeliveryTerms,@Other)";


                cmd.Parameters.AddWithValue("@PaymentTerm", Aq.PaymentTerm ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeliveryTerms", Aq.DeliveryTerms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Other", Aq.Other ?? (object)DBNull.Value);


                cmd.Connection = connection;
                connection.Open();
                Guid TermConditionID = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand(@"
    UPDATE CustomerQuotation 
    SET 
        QuotationSeries = @QuotationSeries, 
        CustomerID = @CustomerID, 
        GrossTotal = @GrossTotal,
        TermConditionID = @TermConditionID
    WHERE QuotationID = @QuotationID", connection);

                cmd2.Parameters.AddWithValue("@QuotationSeries", Aq.QuotationSeries ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@CustomerID", Aq.CustomerID);
                cmd2.Parameters.AddWithValue("@GrossTotal", Aq.GrossTotal);
                cmd2.Parameters.AddWithValue("@QuotationID", Aq.QuotationID);
                cmd2.Parameters.AddWithValue("@TermConditionID", TermConditionID); // Use inserted ID

                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();

                return true;

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


        public bool AddCustomerdetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "INSERT INTO Quotations ([OrderSeries], [CustomerName]) VALUES (@OrderSeries, @CustomerName)";
                cmd.Parameters.AddWithValue("@OrderSeries", Aq.OrderSeries);
                cmd.Parameters.AddWithValue("@CustomerName", Aq.CustomerName);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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


        public List<QuotationModel> OrderList(Guid QuotationID)
        {
            try
            {
                List<QuotationModel> OL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select i.ItemName,i.HSN,qp.Quantity,i.UnitOFMeasure,qp.SellingPrice,qp.TaxableAmount,qp.discountRate,qp.DiscountAmount,qp.CGST,qp.SGST,qp.IGST,qp.TotalAmount from QuotationProduct qp JOIN Items i ON i.ItemId = qp.ProductID where qp.QuotationID=@QuotationID";
                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OL.Add(new QuotationModel()
                    {
                        //QuotationID = reader["QuotationID"] != DBNull.Value ? (int)reader["QuotationID"] : 0,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,
                        IGST = reader["IGST"] != DBNull.Value ? (decimal)reader["IGST"] : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,


                    });
                }

                return OL;

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

        public List<QuotationModel> ListQuotation()
        {
            try
            {
                List<QuotationModel> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "SELECT q.QuotationSeries, q.QuotationID, SUM(qp.TotalAmount) AS finalAmount,q.QuotationDate, q.status, c.CustomerName, c.Phone FROM CustomerQuotation q JOIN QuotationProduct qp ON q.QuotationID=qp.QuotationID JOIN Customers c ON q.CustomerID = c.CustomerID WHERE q.status='open' GROUP BY q.QuotationSeries, q.QuotationID, q.QuotationDate, q.status, c.CustomerName, c.Phone";
                // Q.date,
                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new QuotationModel()
                    {
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,
                        QuotationDate = reader["QuotationDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["QuotationDate"]) : DateOnly.MinValue,
                        finalAmount = reader["finalAmount"] != DBNull.Value ? (decimal)reader["finalAmount"] : 0m,
                        Status = reader["status"] != DBNull.Value ? (string)reader["status"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ContactNumber = reader["Phone"] != DBNull.Value ? Convert.ToInt64( reader["Phone"]) : 0,
                        QuotationSeries = reader.GetString("QuotationSeries")

                    });
                }

                return CL;

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

        // for delete Quotation 
        public bool DeleteQuotation(QuotationModel id)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update CustomerQuotation Set status='close' where QuotationId=@QuotationId";
                cmd.Parameters.AddWithValue("@QuotationID", id.QuotationID);


                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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


        public List<QuotationModel> EditQuota(Guid QuotationID)
        {
            try
            {
                List<QuotationModel> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT cq.QuotationID, cq.QuotationDate ,cq.GrossTotal,c.CustomerName,cq.QuotationSeries,cq.TermConditionID,qp.ProductID,i.ItemName,i.HSN,qp.Quantity,i.UnitOFMeasure,qp.SellingPrice,qp.TaxableAmount,qp.discountRate,qp.DiscountAmount,qp.CGST,qp.SGST,qp.IGST,qp.TotalAmount,T.PaymentTerm,T.DeliveryTerms,T.Other FROM CustomerQuotation cq JOIN QuotationProduct qp ON cq.QuotationID = qp.QuotationID JOIN TermCondition T ON cq.TermConditionID = T.TermConditionID  JOIN Items i ON i.ItemId = qp.ProductID JOIN Customers c ON c.CustomerID=cq.CustomerID where cq.QuotationID = @QuotationID";

                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new QuotationModel()
                    {
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,
                        PaymentTerm = reader["PaymentTerm"] != DBNull.Value ? (string)reader["PaymentTerm"] : string.Empty,
                        DeliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,
                        IGST = reader["IGST"] != DBNull.Value ? (decimal)reader["IGST"] : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,
                        QuotationSeries = reader["QuotationSeries"] != DBNull.Value ? (string)reader["QuotationSeries"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ProductID = reader["ProductID"] != DBNull.Value ? (Guid)reader["ProductID"] : Guid.Empty,
                        //ItemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        TermConditionID = reader["TermConditionID"] != DBNull.Value ? (Guid)reader["TermConditionID"] : Guid.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,
                        QuotationDate = reader["QuotationDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["QuotationDate"]) : DateOnly.MinValue,



                    });
                }

                return CL;

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

        //update edit value 
        public bool updateEditPro(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update QuotationProduct Set  Quantity='{Aq.Quantity}' , TaxableAmount='{Aq.TaxableAmount}' , discountRate='{Aq.discountRate}', DiscountAmount = '{Aq.DiscountAmount}' ,  CGST= '{Aq.CGST}' , SGST = '{Aq.SGST}', IGST = '{Aq.IGST}',  TotalAmount= '{Aq.TotalAmount}' where ProductID = @ProductID ";
                cmd.Parameters.AddWithValue("@ProductID", Aq.ProductID);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public List<QuotationModel> ViewQuotationDoc(Guid QuotationID)
        {
            try
            {
                List<QuotationModel> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT cq.GrossTotal,c.CustomerName,cq.QuotationDate,cq.QuotationSeries,i.ItemName,i.HSN,qp.Quantity,i.UnitOFMeasure,qp.SellingPrice,qp.TaxableAmount,qp.discountRate,qp.DiscountAmount,qp.CGST,qp.SGST,qp.TotalAmount,T.PaymentTerm,T.DeliveryTerms,T.Other FROM CustomerQuotation cq JOIN QuotationProduct qp ON cq.QuotationID = qp.QuotationID JOIN TermCondition T ON cq.TermConditionID = T.TermConditionID  JOIN Customers c ON cq.CustomerID = c.CustomerID JOIN Items i ON i.ItemId = qp.ProductID  WHERE cq.QuotationID = @QuotationID";

                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);
                cmd.Connection = connection;
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new QuotationModel()
                    {
                        PaymentTerm = reader["PaymentTerm"] != DBNull.Value ? (string)reader["PaymentTerm"] : string.Empty,
                        QuotationSeries = reader["QuotationSeries"] != DBNull.Value ? (string)reader["QuotationSeries"] : string.Empty,
                        DeliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,
                        //IGST = reader["IGST"] != DBNull.Value ? (decimal)reader["IGST"] : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        Other = reader["Other"] != DBNull.Value ? (string)reader["Other"] : string.Empty,
                        QuotationDate = reader["QuotationDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["QuotationDate"]) : DateOnly.MinValue,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,

                    });

                }


                return CL;

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

        public List<QuotationModel> ShowProductLi(int QuotationID)
        {
            try
            {
                List<QuotationModel> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select ItemName,HSN,Quantity,UnitOFMeasure,SellingPrice,TaxableAmount,discountRate,DiscountAmount,CGST,SGST,TotalAmount from QuotationProduct where QuotationID=@QuotationID ";
                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new QuotationModel()
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,

                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,


                    });
                }

                return CL;

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

        //for invoice 
        public List<Invoice> InvoiceQvalue(Guid QuotationID)
        {
            try
            {
                List<Invoice> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = @"SELECT cq.QuotationID, cq.GrossTotal,c.CustomerName, cq.QuotationSeries, cq.TermConditionID, i.ItemName, 
                                    qp.Quantity, i.UnitOFMeasure, qp.SellingPrice, qp.TaxableAmount, qp.discountRate, qp.DiscountAmount, qp.CGST, 
                                    qp.SGST,qp.IGST, qp.TotalAmount FROM CustomerQuotation cq 
                                    JOIN QuotationProduct qp ON cq.QuotationID=qp.QuotationID JOIN Customers c ON c.CustomerID = cq.CustomerID
                                    JOIN Items i ON i.ItemId = qp.ProductID WHERE cq.QuotationID=@QuotationID";  //Q.date
                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);
                cmd.Connection = connection;
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new Invoice()
                    {
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,
                        QuotationSeries = reader["QuotationSeries"] != DBNull.Value ? (string)reader["QuotationSeries"] : string.Empty,
                        TermConditionID = reader["TermConditionID"] != DBNull.Value ? (Guid)reader["TermConditionID"] : Guid.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,
                        IGST = reader["IGST"] != DBNull.Value ? (decimal)reader["IGST"] : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,

                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,

                    });

                }


                return CL;

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




        public Guid AddInvoiceDetails(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = "UPDATE TermCondition SET PaymentTerm = @PaymentTerm, DeliveryTerms = @DeliveryTerms, Other = @Other WHERE TermConditionID = @TermConditionID";
    
                cmd1.Parameters.AddWithValue("@PaymentTerm", Aq.PaymentTerm ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@DeliveryTerms", Aq.DeliveryTerms ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@Other", Aq.Other ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@TermConditionID", Aq.TermConditionID);

                cmd1.Connection = connection;
                connection.Open();
                cmd1.ExecuteNonQuery(); // Since this is an UPDATE
                connection.Close();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"INSERT INTO Invoice ([QuotationID],[Status],[PaymentMode],[PaymentType],[ReferenceNumber],[InvoiceNumber],[TDSPercentage],[TDSAmount],[TermConditionID],[TCSPercentage],[TCSAmount]) " + "OUTPUT INSERTED.InvoiceID " +
                                                "VALUES (@QuotationID,'Unpaid',@PaymentMode,@PaymentType,@ReferenceNumber,@InvoiceNumber,@TDSPercentage,@TDSAmount,@TermConditionID,@TCSPercentage,@TCSAmount)";
                cmd.Parameters.AddWithValue("@QuotationID", Aq.QuotationID);
                cmd.Parameters.AddWithValue("@InvoiceDate", Aq.InvoiceDate);
                cmd.Parameters.AddWithValue("@PaymentMode", Aq.PaymentMode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PaymentType", Aq.PaymentType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ReferenceNumber", Aq.ReferenceNumber);
                cmd.Parameters.AddWithValue("@InvoiceNumber", Aq.InvoiceNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TDSPercentage", Aq.TDSPercentage);
                cmd.Parameters.AddWithValue("@TDSAmount", Aq.TDSAmount);
                cmd.Parameters.AddWithValue("@TermConditionID", Aq.TermConditionID);
                cmd.Parameters.AddWithValue("@TCSPercentage", Aq.TCSPercentage);
                cmd.Parameters.AddWithValue("@TCSAmount", Aq.TCSAmount);

                cmd.Connection = connection;
                connection.Open();
                Guid InvoiceID = (Guid)cmd.ExecuteScalar();
                connection.Close();

                return InvoiceID;
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

        public bool SaveShipingDetails(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd1 = new SqlCommand();

                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"INSERT INTO Address ([Country],[State],[City],[Area],[Pincode],[AddressLine1],[District]) OUTPUT INSERTED.AddressID VALUES ('India',@State,@City,@Area,@Pincode,'',@District)";


                cmd1.Parameters.AddWithValue("@State", Aq.State ?? "-");
                cmd1.Parameters.AddWithValue("@City", Aq.City ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@Area", Aq.Area ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@Pincode", Aq.Pincode ?? (object)DBNull.Value);

                cmd1.Parameters.AddWithValue("@District", Aq.District ?? (object)DBNull.Value);


                cmd1.Connection = connection;
                connection.Open();
                int AddressID = Convert.ToInt32(cmd1.ExecuteScalar());
                connection.Close();

                Aq.AddressID = AddressID;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = "UPDATE Invoice SET ShippingAddressID = @ShippingAddressID WHERE InvoiceID =  @InvoiceID";
                cmd.Parameters.AddWithValue("@InvoiceID", Aq.InvoiceID);
                cmd.Parameters.AddWithValue("@ShippingAddressID", Aq.AddressID);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public bool TransportDetail(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into TransportDetails ([InvoiceID],[TransporterID],[ModeOfShipment],[GRNumber],[NumberOfCartons],[TransporterName],[VehicleType],[NetWeight],[TransactionType],[Distance],[VehicleNumber],[TimeOfSupply],[GrossWeight],[FreightCharges],[TransporterDocumentNumber],[TransporterDocumentDate])values('{Aq.InvoiceID}','{Aq.TransporterID}','{Aq.ModeOfShipment}','{Aq.GRNumber}','{Aq.NumberOfCartons}','{Aq.TransporterName}','{Aq.VehicleType}','{Aq.NetWeight}','{Aq.TransactionType}','{Aq.Distance}','{Aq.VehicleNumber}','{Aq.TimeOfSupply}','{Aq.GrossWeight}','{Aq.FreightCharges}','{Aq.TransporterDocumentNumber}','{Aq.TransporterDocumentDate}')";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public bool DispatchDetail(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd1 = new SqlCommand();

                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"INSERT INTO Address ([Country],[AddressLine1],[District],[Area],[City],[State],[Pincode]) OUTPUT INSERTED.AddressID VALUES ('India',@AddressLine1,@District,@Area,@City,@State,@Pincode)";


                cmd1.Parameters.AddWithValue("@AddressLine1", Aq.AddressLine1 ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@District", Aq.District ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@Area", Aq.Area ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@City", Aq.City ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@State", Aq.State ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@Pincode", Aq.Pincode ?? (object)DBNull.Value);

                cmd1.Connection = connection;
                connection.Open();
                int AddressID = Convert.ToInt32(cmd1.ExecuteScalar());

                connection.Close();

                Aq.AddressID = AddressID;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into DispatchDetails ([InvoiceID],[AddressID])values(@InvoiceID,@AddressID)";
                cmd.Parameters.AddWithValue("@InvoiceID", Aq.InvoiceID);
                cmd.Parameters.AddWithValue("@AddressID", Aq.AddressID);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public bool CourierDetail(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into CourierDetails ([InvoiceID],[CourierDetail],[CourierCompany],[TrackingNumber])values('{Aq.InvoiceID}','{Aq.CourierDetail}','{Aq.CourierCompany}','{Aq.TrackingNumber}')";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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


        public bool Updatestatus(Invoice id)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update CustomerQuotation Set status='close' where QuotationID=@QuotationID; update CustomerQuotation Set GrossTotal='{id.GrossTotal}' where QuotationID=@QuotationID;";
                cmd.Parameters.AddWithValue("@QuotationID", id.QuotationID);


                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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

        public bool updateInvoiceDetails(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update Invoice Set InvoiceDate='{Aq.InvoiceDate}' ,InvoiceNumber='{Aq.InvoiceNumber}', QuotationID='{Aq.QuotationID}' where InvoiceID = '{Aq.InvoiceID}' ";



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public List<Invoice> ListInvoice()
        {
            try
            {
                List<Invoice> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "SELECT q.QuotationID, i.InvoiceID, i.InvoiceDate, q.GrossTotal, i.Status, i.InvoiceNumber, i.EWayBill, c.CustomerName, c.Phone FROM CustomerQuotation q JOIN Invoice i ON q.QuotationID = i.QuotationID JOIN Customers c ON c.CustomerID = q.CustomerID WHERE i.Status = 'Unpaid';";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new Invoice()
                    {
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,

                        InvoiceID = reader["InvoiceID"] != DBNull.Value ? (Guid)reader["InvoiceID"] : Guid.Empty,
                        InvoiceDate = reader["InvoiceDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["InvoiceDate"]) : DateOnly.MinValue,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,
                        Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        EWayBill = reader["EWayBill"] != DBNull.Value ? (string)reader["EWayBill"] : string.Empty,
                        ContactNumber = reader["Phone"] != DBNull.Value ? Convert.ToInt64(reader["Phone"]) : 0,

                    });
                }

                return CL;

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

        public bool CancelInvoice(Invoice id, Guid InvoiceID)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update Invoice Set status='Cancel' where InvoiceID=@InvoiceID";
                cmd.Parameters.AddWithValue("@InvoiceID", id.InvoiceID);


                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public Invoice EditInvoice(Guid InvoiceID)
        {

            Invoice cl = new Invoice();
            try
            {

                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.InvoiceID, I.InvoiceDate,I.InvoiceNumber,I.PaymentMode,I.PaymentType,I.ReferenceNumber,c.CustomerName,Q.GrossTotal from CustomerQuotation Q Join Invoice I ON Q.QuotationID=I.QuotationID JOIN Customers c ON c.CustomerID = Q.CustomerID where InvoiceID='{InvoiceID}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cl = new Invoice
                    {

                        InvoiceID = reader["InvoiceID"] != DBNull.Value ? (Guid)reader["InvoiceID"] : Guid.Empty,
                        InvoiceDate = reader["InvoiceDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["InvoiceDate"]) : DateOnly.MinValue,
                        InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        PaymentMode = reader["PaymentMode"] != DBNull.Value ? (string)reader["PaymentMode"] : string.Empty,
                        PaymentType = reader["PaymentType"] != DBNull.Value ? (string)reader["PaymentType"] : string.Empty,
                        ReferenceNumber = reader["ReferenceNumber"] != DBNull.Value ? (string)reader["ReferenceNumber"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,
                    };



                }

                return cl;

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
        public Invoice ViewShippedTo(Guid InvoiceID)
        {
            Invoice cl = new Invoice();
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = " SELECT S.CompanyName, A.State, A.Pincode, S.ConsignmentNumber, S.Branch, A.District, S.ContactNumber, S.GSTIN, A.Area, A.City, S.ShippingRemote, S.OtherReferences,A.AddressLine1 FROM ShippedTo S JOIN Address A ON S.AddressID = A.AddressID WHERE InvoiceID =@InvoiceID ";
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cl = new Invoice
                    {
                        CompanyName = reader["CompanyName"] != DBNull.Value ? (string)reader["CompanyName"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        Pincode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty,
                        ConsignmentNumber = reader["ConsignmentNumber"] != DBNull.Value ? (string)reader["ConsignmentNumber"] : string.Empty,
                        Branch = reader["Branch"] != DBNull.Value ? (string)reader["Branch"] : string.Empty,
                        District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty,
                        ContactNumber = reader["ContactNumber"] != DBNull.Value ? (long)reader["ContactNumber"] : 0,
                        GSTIN = reader["GSTIN"] != DBNull.Value ? (string)reader["GSTIN"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        ShippingRemote = reader["ShippingRemote"] != DBNull.Value ? (string)reader["ShippingRemote"] : string.Empty,
                        OtherReferences = reader["OtherReferences"] != DBNull.Value ? (string)reader["OtherReferences"] : string.Empty
                    };
                }
                return cl;
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

        public Invoice ViewTransportDetails(Guid InvoiceID)
        {
            Invoice cl = new Invoice();
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT TransporterID, ModeOfShipment, GRNumber, NumberOfCartons, TransporterName, VehicleType, NetWeight, TransactionType, Distance, VehicleNumber, TimeOfSupply, GrossWeight, FreightCharges,TransporterDocumentNumber, TransporterDocumentDate FROM TransportDetails WHERE InvoiceID = @InvoiceID";
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cl = new Invoice
                    {
                        TransporterID = reader["TransporterID"] != DBNull.Value ? (string)reader["TransporterID"] : string.Empty,
                        ModeOfShipment = reader["ModeOfShipment"] != DBNull.Value ? (string)reader["ModeOfShipment"] : string.Empty,
                        GRNumber = reader["GRNumber"] != DBNull.Value ? (string)reader["GRNumber"] : string.Empty,
                        NumberOfCartons = reader["NumberOfCartons"] != DBNull.Value ? (int)reader["NumberOfCartons"] : 0,
                        TransporterName = reader["TransporterName"] != DBNull.Value ? (string)reader["TransporterName"] : string.Empty,
                        VehicleType = reader["VehicleType"] != DBNull.Value ? (string)reader["VehicleType"] : string.Empty,
                        NetWeight = reader["NetWeight"] != DBNull.Value ? (decimal)reader["NetWeight"] : 0m,
                        TransactionType = reader["TransactionType"] != DBNull.Value ? (string)reader["TransactionType"] : string.Empty,
                        Distance = reader["Distance"] != DBNull.Value ? (int)reader["Distance"] : 0,
                        VehicleNumber = reader["VehicleNumber"] != DBNull.Value ? (string)reader["VehicleNumber"] : string.Empty,
                        TimeOfSupply = reader["TimeOfSupply"] != DBNull.Value ? TimeOnly.FromTimeSpan((TimeSpan)reader["TimeOfSupply"]) : TimeOnly.MinValue,

                        GrossWeight = reader["GrossWeight"] != DBNull.Value ? (decimal)reader["GrossWeight"] : 0m,
                        FreightCharges = reader["FreightCharges"] != DBNull.Value ? (string)reader["FreightCharges"] : string.Empty,
                        TransporterDocumentNumber = reader["TransporterDocumentNumber"] != DBNull.Value ? (string)reader["TransporterDocumentNumber"] : string.Empty,
                        TransporterDocumentDate = reader["TransporterDocumentDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["TransporterDocumentDate"]) : DateOnly.MinValue
                    };
                }
                return cl;
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
        public void SaveDispatchDetails(Dispatch dispatch)
        {

            connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"INSERT INTO DispatchDetails ( InvoiceID, TransporterID,TransporterName,TransporterDocumentNumber,TransporterDocumentDate,
                                    CourierDetail,CourierCompany,TrackingNumber,GRNumber,NumberOfCartons,ModeOfShipment, VehicleType, NetWeight,
                                    GrossWeight,TransactionType,DistanceKM,VehicleNumber,TimeOfSupply,FreightCharges)
                                    VALUES 
                                    ( @InvoiceID, @TransporterID, @TransporterName, @TransporterDocumentNumber, @TransporterDocumentDate, 
                                      @CourierDetail, @CourierCompany, @TrackingNumber, @GRNumber, @NumberOfCartons, @ModeOfShipment, 
                                      @VehicleType, @NetWeight, @GrossWeight, @TransactionType, @DistanceKM, @VehicleNumber, @TimeOfSupply,
                                      @FreightCharges );
                                    UPDATE Invoice SET InvoiceStatus = @InvoiceStatus WHERE InvoiceID = @InvoiceID;";

                cmd.Parameters.AddWithValue("@InvoiceID", dispatch.InvoiceID);
                cmd.Parameters.AddWithValue("@TransporterID", dispatch.TransporterID);
                cmd.Parameters.AddWithValue("@TransporterName", dispatch.TransporterName);
                cmd.Parameters.AddWithValue("@TransporterDocumentNumber", dispatch.TransporterDocumentNumber);
                cmd.Parameters.AddWithValue("@TransporterDocumentDate", dispatch.TransporterDocumentDate);
                cmd.Parameters.AddWithValue("@CourierDetail", dispatch.CourierDetail);
                cmd.Parameters.AddWithValue("@CourierCompany", dispatch.CourierCompany);
                cmd.Parameters.AddWithValue("@TrackingNumber", dispatch.TrackingNumber);
                cmd.Parameters.AddWithValue("@GRNumber", dispatch.GRNumber);
                cmd.Parameters.AddWithValue("@NumberOfCartons", dispatch.NumberOfCartons);
                cmd.Parameters.AddWithValue("@ModeOfShipment", dispatch.ModeOfShipment);
                cmd.Parameters.AddWithValue("@VehicleType", dispatch.VehicleType);
                cmd.Parameters.AddWithValue("@NetWeight", dispatch.NetWeight);
                cmd.Parameters.AddWithValue("@GrossWeight", dispatch.GrossWeight);
                cmd.Parameters.AddWithValue("@TransactionType", dispatch.TransactionType);
                cmd.Parameters.AddWithValue("@DistanceKM", dispatch.DistanceKM);
                cmd.Parameters.AddWithValue("@VehicleNumber", dispatch.VehicleNumber);
                cmd.Parameters.AddWithValue("@TimeOfSupply", dispatch.TimeOfSupply);
                cmd.Parameters.AddWithValue("@FreightCharges", dispatch.FreightCharges);
                cmd.Parameters.AddWithValue("@InvoiceStatus", 1);
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                cmd.Dispose();
            }
        }
        public Invoice ViewDispatchDetails(Guid InvoiceID)
        {
            Invoice cl = new Invoice();
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT a.AddressLine1, a.District, a.Area, a.City, a.State, a.Pincode FROM Address a Join  Invoice i On a.AddressID=i.ShippingAddressID WHERE i.InvoiceID = @InvoiceID";
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cl = new Invoice
                    {
                        AddressLine1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,
                        District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        Pincode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty
                    };
                }
                return cl;
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
        public Invoice ViewCourierDetails(Guid InvoiceID)
        {
            Invoice cl = new Invoice();
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT CourierDetail, CourierCompany, TrackingNumber FROM CourierDetails WHERE InvoiceID = @InvoiceID";
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cl = new Invoice
                    {
                        CourierDetail = reader["CourierDetail"] != DBNull.Value ? (string)reader["CourierDetail"] : string.Empty,
                        CourierCompany = reader["CourierCompany"] != DBNull.Value ? (string)reader["CourierCompany"] : string.Empty,
                        TrackingNumber = reader["TrackingNumber"] != DBNull.Value ? (string)reader["TrackingNumber"] : string.Empty
                    };
                }
                return cl;
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
        public bool updateInvoice(Invoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update Invoice Set  PaymentMode='{Aq.PaymentMode}' , PaymentType='{Aq.PaymentType}' , ReferenceNumber='{Aq.ReferenceNumber}' , InvoiceNumber = '{Aq.InvoiceNumber}' where InvoiceID = @InvoiceID ";
                cmd.Parameters.AddWithValue("@InvoiceID", Aq.InvoiceID);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public List<Invoice> ViewInvoiceProd(Guid QuotationID)
        {
            try
            {
                List<Invoice> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"select i.ItemName, qp.Quantity, i.UnitOFMeasure, qp.SellingPrice, qp.TaxableAmount, qp.discountRate, qp.DiscountAmount, qp.CGST, qp.SGST,qp.IGST, qp.TotalAmount  from QuotationProduct qp JOIN Items i ON qp.ProductID=i.ItemId where qp.QuotationID=@QuotationID";
                cmd.Parameters.AddWithValue("@QuotationID", QuotationID);
                cmd.Connection = connection;
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new Invoice()
                    {
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UnitOFMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        SellingPrice = reader["SellingPrice"] != DBNull.Value ? (decimal)reader["SellingPrice"] : 0m,
                        TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? (decimal)reader["TaxableAmount"] : 0m,
                        discountRate = reader["discountRate"] != DBNull.Value ? (decimal)reader["discountRate"] : 0m,
                        DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? (decimal)reader["DiscountAmount"] : 0m,
                        CGST = reader["CGST"] != DBNull.Value ? (decimal)reader["CGST"] : 0m,
                        SGST = reader["SGST"] != DBNull.Value ? (decimal)reader["SGST"] : 0m,
                        IGST = reader["IGST"] != DBNull.Value ? (decimal)reader["IGST"] : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0m,

                    });

                }


                return CL;

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
        public bool Proformainvoice(ProformaInvoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"INSERT INTO TermCondition ([PaymentTerm],[DeliveryTerms]) OUTPUT INSERTED.TermConditionID VALUES (@PaymentTerm,@DeliveryTerms)";


                cmd.Parameters.AddWithValue("@PaymentTerm", Aq.PaymentTerm ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DeliveryTerms", Aq.DeliveryTerms ?? (object)DBNull.Value);


                cmd.Connection = connection;
                connection.Open();
                Guid TermConditionID = (Guid)cmd.ExecuteScalar();
                connection.Close();

                Aq.TermConditionID = TermConditionID;

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = @"INSERT INTO ProformaInvoice 
                ([OrderSeriesID],[CustomerName],[POReferenceNumber],[VenderCode],[JobCode],[PODate],[TermConditionID],[Status])
                VALUES (@OrderSeriesID, @CustomerName, @POReferenceNumber, @VenderCode, @JobCode, @PODate, @TermConditionID, 'Pending')";
                cmd1.Parameters.AddWithValue("@OrderSeriesID", Aq.OrderSeriesID ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@CustomerName", Aq.CustomerName ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@POReferenceNumber", Aq.POReferenceNumber ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@VenderCode", Aq.VenderCode ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@JobCode", Aq.JobCode ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@PODate", Aq.PODate);
                cmd1.Parameters.AddWithValue("@TermConditionID", TermConditionID);

                cmd1.Connection = connection;
                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();





                return true;

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



        public List<ProformaInvoice> ListProformaInvoice()
        {
            try
            {
                List<ProformaInvoice> PI = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "select ProformaInvoiceID,CustomerName,PODate,Status from ProformaInvoice Where Status='Pending'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI.Add(new ProformaInvoice()
                    {
                        ProformaInvoiceID = reader["ProformaInvoiceID"] != DBNull.Value ? (Guid)reader["ProformaInvoiceID"] : Guid.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty,
                        PODate = reader["PODate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["PODate"]) : DateOnly.MinValue,
                    });
                }

                return PI;

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

        public bool DeleteProforma(ProformaInvoice id, Guid ProformaInvoiceID)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM ProformaInvoice WHERE ProformaInvoiceID = @ProformaInvoiceID";
                cmd.Parameters.AddWithValue("@ProformaInvoiceID", ProformaInvoiceID);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

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



        public ProformaInvoice EditProformaInvoice(ProformaInvoice PI, Guid ProformaInvoiceID)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"SELECT P.ProformaInvoiceID,P.OrderSeriesID, P.CustomerName,P.TermConditionID ,T.PaymentTerm,T.DeliveryTerms,P.POReferenceNumber,P.VenderCode,P.JobCode,P.PODate FROM ProformaInvoice P Join TermCondition T ON P.TermConditionID=T.TermConditionID WHERE ProformaInvoiceID ='{ProformaInvoiceID}'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI = new ProformaInvoice
                    {
                        ProformaInvoiceID = reader["ProformaInvoiceID"] != DBNull.Value ? (Guid)reader["ProformaInvoiceID"] : Guid.Empty,
                        OrderSeriesID = reader["OrderSeriesID"] != DBNull.Value ? (string)reader["OrderSeriesID"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        TermConditionID = reader["TermConditionID"] != DBNull.Value ? (Guid)reader["TermConditionID"] : Guid.Empty,
                        JobCode = reader["JobCode"] != DBNull.Value ? (string)reader["JobCode"] : string.Empty,
                        PaymentTerm = reader["PaymentTerm"] != DBNull.Value ? (string)reader["PaymentTerm"] : string.Empty,
                        DeliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,
                        POReferenceNumber = reader["POReferenceNumber"] != DBNull.Value ? (string)reader["POReferenceNumber"] : string.Empty,
                        VenderCode = reader["VenderCode"] != DBNull.Value ? (string)reader["VenderCode"] : string.Empty,

                        PODate = reader["PODate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["PODate"]) : DateOnly.MinValue,
                    };
                }

                return PI;

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

        public bool UpdateProfoma(ProformaInvoice Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;


                cmd1.CommandText = @" UPDATE TermCondition SET DeliveryTerms = @DeliveryTerms, PaymentTerm = @PaymentTerm WHERE TermConditionID = @TermConditionID";


                cmd1.Parameters.AddWithValue("@DeliveryTerms", Aq.DeliveryTerms ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@PaymentTerm", Aq.PaymentTerm ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@TermConditionID", Aq.TermConditionID);


                cmd1.Connection = connection;
                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update ProformaInvoice Set OrderSeriesID='{Aq.OrderSeriesID}', CustomerName='{Aq.CustomerName}' , POReferenceNumber='{Aq.POReferenceNumber}', VenderCode = '{Aq.VenderCode}',JobCode='{Aq.JobCode}' where ProformaInvoiceID = @ProformaInvoiceID ";
                cmd.Parameters.AddWithValue("@ProformaInvoiceID", Aq.ProformaInvoiceID);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public bool CreditNote(CreditNote Ac)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into CreditNote ([OrderSeriesID],[CreditNoteFor],[CreditNoteType],[CustomerName],[InvoiceNumber],[NumberOfCartons],[LorryReceiptNumber],[LorryReceiptDate],[ModeOfShipment],[Remarks],[CostCenter],[Location],[VenderNoteId],[Status])values('{Ac.OrderSeriesID}','{Ac.CreditNoteFor}','{Ac.CreditNoteType}','{Ac.CustomerName}','{Ac.InvoiceNumber}','{Ac.NumberOfCartons}','{Ac.LorryReceiptNumber}','{Ac.LorryReceiptDate}','{Ac.ModeOfShipment}','{Ac.Remarks}','{Ac.CostCenter}','{Ac.Location}','{Ac.VenderNoteId}','Pending')";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                //connection.Close();



                return true;

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

        public bool DeleteCreditNote(CreditNote id, Guid CreditNoteId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM CreditNote WHERE CreditNoteId = @CreditNoteId";
                cmd.Parameters.AddWithValue("@CreditNoteId", CreditNoteId);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

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

        public CreditNote EditCreditNote(CreditNote PI, Guid CreditNoteId)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"select CreditNoteId,OrderSeriesID,CreditNoteFor,CreditNoteType,CreatedOn,CustomerName,InvoiceNumber,NumberOfCartons,LorryReceiptNumber,LorryReceiptDate,ModeOfShipment,Remarks,CostCenter,Location,VenderNoteId FROM CreditNote WHERE CreditNoteId ='{CreditNoteId}'";


                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI = new CreditNote
                    {
                        CreditNoteId = reader["CreditNoteId"] != DBNull.Value ? (Guid)reader["CreditNoteId"] : Guid.Empty,
                        OrderSeriesID = reader["OrderSeriesID"] != DBNull.Value ? (string)reader["OrderSeriesID"] : string.Empty,
                        CreditNoteFor = reader["CreditNoteFor"] != DBNull.Value ? (string)reader["CreditNoteFor"] : string.Empty,
                        CreditNoteType = reader["CreditNoteType"] != DBNull.Value ? (string)reader["CreditNoteType"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedOn"]) : DateOnly.MinValue,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        NumberOfCartons = reader["NumberOfCartons"] != DBNull.Value ? (int)reader["NumberOfCartons"] : 0,
                        LorryReceiptNumber = reader["LorryReceiptNumber"] != DBNull.Value ? (string)reader["LorryReceiptNumber"] : string.Empty,
                        LorryReceiptDate = reader["LorryReceiptDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["LorryReceiptDate"]) : DateOnly.MinValue,
                        ModeOfShipment = reader["ModeOfShipment"] != DBNull.Value ? (string)reader["ModeOfShipment"] : string.Empty,
                        Remarks = reader["Remarks"] != DBNull.Value ? (string)reader["Remarks"] : string.Empty,
                        CostCenter = reader["CostCenter"] != DBNull.Value ? (string)reader["CostCenter"] : string.Empty,
                        Location = reader["Location"] != DBNull.Value ? (string)reader["Location"] : string.Empty,
                        VenderNoteId = reader["VenderNoteId"] != DBNull.Value ? (string)reader["VenderNoteId"] : string.Empty,


                    };
                }

                return PI;

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

        public bool UpdateCreditNote(CreditNote Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update CreditNote Set OrderSeriesID='{Aq.OrderSeriesID}', CreditNoteFor='{Aq.CreditNoteFor}' , CreditNoteType='{Aq.CreditNoteType}'  , CustomerName='{Aq.CustomerName}', InvoiceNumber = '{Aq.InvoiceNumber}',NumberOfCartons='{Aq.NumberOfCartons}', LorryReceiptNumber='{Aq.LorryReceiptNumber}', ModeOfShipment='{Aq.ModeOfShipment}', Remarks='{Aq.Remarks}', CostCenter='{Aq.CostCenter}', Location='{Aq.Location}', VenderNoteId='{Aq.VenderNoteId}' where CreditNoteId = @CreditNoteId ";
                cmd.Parameters.AddWithValue("@CreditNoteId", Aq.CreditNoteId);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public List<CreditNote> ListCreditNote()
        {
            try
            {
                List<CreditNote> PI = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "select CreditNoteId,InvoiceNumber,CustomerName,CreatedOn,NumberOfCartons,CreditNoteFor from CreditNote Where Status='Pending'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI.Add(new CreditNote()
                    {
                        CreditNoteId = reader["CreditNoteId"] != DBNull.Value ? (Guid)reader["CreditNoteId"] : Guid.Empty,
                        InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedOn"]) : DateOnly.MinValue,
                        NumberOfCartons = reader["NumberOfCartons"] != DBNull.Value ? (int)reader["NumberOfCartons"] : 0,
                        CreditNoteFor = reader["CreditNoteFor"] != DBNull.Value ? (string)reader["CreditNoteFor"] : string.Empty,

                    });
                }

                return PI;

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

        public bool DeliveryChallan(DeliveryChallan Ac)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into DeliveryChallan ([DeliveryChallanType],[Branch],[OrderSeriesID],[SecuritySealNumber],[CustomerName],[ChallanDate],[Status])values('{Ac.DeliveryChallanType}','{Ac.Branch}','{Ac.OrderSeriesID}','{Ac.SecuritySealNumber}','{Ac.CustomerName}','{Ac.ChallanDate}','open')";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();


                return true;

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


        public List<DeliveryChallan> ListDeliveryChallan()
        {
            try
            {
                List<DeliveryChallan> PI = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "select DeliveryChallanID,DeliveryChallanType,Branch,OrderSeriesID,SecuritySealNumber,CustomerName,ChallanDate,Status from DeliveryChallan Where Status='open'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI.Add(new DeliveryChallan()
                    {
                        DeliveryChallanID = reader["DeliveryChallanID"] != DBNull.Value ? (Guid)reader["DeliveryChallanID"] : Guid.Empty,
                        DeliveryChallanType = reader["DeliveryChallanType"] != DBNull.Value ? (string)reader["DeliveryChallanType"] : string.Empty,
                        Branch = reader["Branch"] != DBNull.Value ? (string)reader["Branch"] : string.Empty,
                        OrderSeriesID = reader["OrderSeriesID"] != DBNull.Value ? (string)reader["OrderSeriesID"] : string.Empty,
                        SecuritySealNumber = reader["SecuritySealNumber"] != DBNull.Value ? (string)reader["SecuritySealNumber"] : string.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ChallanDate = reader["ChallanDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ChallanDate"]) : DateOnly.MinValue,
                        Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty

                    });
                }

                return PI;

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

        public bool DeleteDeliveryChallan(DeliveryChallan id, Guid DeliveryChallanID)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "DELETE FROM DeliveryChallan WHERE DeliveryChallanID = @DeliveryChallanID";
                cmd.Parameters.AddWithValue("@DeliveryChallanID", DeliveryChallanID);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

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

        public DeliveryChallan EditDeliveryChallan(DeliveryChallan PI, Guid DeliveryChallanID)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"select DeliveryChallanID,DeliveryChallanType,Branch,OrderSeriesID,SecuritySealNumber,CustomerName,ChallanDate FROM DeliveryChallan WHERE DeliveryChallanID ='{DeliveryChallanID}'";


                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI = new DeliveryChallan
                    {
                        DeliveryChallanID = reader["DeliveryChallanID"] != DBNull.Value ? (Guid)reader["DeliveryChallanID"] : Guid.Empty,
                        DeliveryChallanType = reader["DeliveryChallanType"] != DBNull.Value ? (string)reader["DeliveryChallanType"] : string.Empty,
                        Branch = reader["Branch"] != DBNull.Value ? (string)reader["Branch"] : string.Empty,
                        OrderSeriesID = reader["OrderSeriesID"] != DBNull.Value ? (string)reader["OrderSeriesID"] : string.Empty,
                        SecuritySealNumber = reader["SecuritySealNumber"] != DBNull.Value ? (string)reader["SecuritySealNumber"] : string.Empty,


                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ChallanDate = reader["ChallanDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ChallanDate"]) : DateOnly.MinValue,

                    };
                }

                return PI;

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




        public bool UpdateDeliveryChallan(DeliveryChallan Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update DeliveryChallan Set DeliveryChallanType='{Aq.DeliveryChallanType}', Branch='{Aq.Branch}' , OrderSeriesID='{Aq.OrderSeriesID}'  , SecuritySealNumber='{Aq.SecuritySealNumber}', CustomerName = '{Aq.CustomerName}' where DeliveryChallanID = @DeliveryChallanID ";
                cmd.Parameters.AddWithValue("@DeliveryChallanID", Aq.DeliveryChallanID);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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


        public bool CreateReturnNote(ReturnNote Ac)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"insert into ReturnNote ([DeliveryChallanID],[ItemName],[Quantity],[UOM],[ReturnableQuantity],[Status])values('{Ac.DeliveryChallanID}','{Ac.ItemName}','{Ac.Quantity}','{Ac.UOM}','{Ac.ReturnableQuantity}','open')";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                //connection.Close();



                return true;

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



        public List<ReturnNote> ListReturnNote()
        {
            try
            {
                List<ReturnNote> PI = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "select ReturnNoteId,DeliveryChallanID,ItemName,Quantity,UOM,ReturnableQuantity,Status from ReturnNote Where Status='open'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI.Add(new ReturnNote()
                    {
                        ReturnNoteId = reader["ReturnNoteId"] != DBNull.Value ? (Guid)reader["ReturnNoteId"] : Guid.Empty,
                        DeliveryChallanID = reader["DeliveryChallanID"] != DBNull.Value ? (string)reader["DeliveryChallanID"] : string.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UOM = reader["UOM"] != DBNull.Value ? (string)reader["UOM"] : string.Empty,
                        ReturnableQuantity = reader["ReturnableQuantity"] != DBNull.Value ? (int)reader["ReturnableQuantity"] : 0,
                        Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty,




                    });
                }

                return PI;

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

        public bool DeleteReturnNote(ReturnNote id, Guid ReturnNoteId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"Delete From  ProformaInvoice  where ProformaInvoiceID = {ProformaInvoiceID}";
                cmd.CommandText = "DELETE FROM ReturnNote WHERE ReturnNoteId = @ReturnNoteId";
                cmd.Parameters.AddWithValue("@ReturnNoteId", ReturnNoteId);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
                return true;

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

        public ReturnNote EditReturnNote(ReturnNote PI, Guid ReturnNoteId)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select ReturnNoteId,DeliveryChallanID,ItemName,Quantity,UOM,ReturnableQuantity from ReturnNote Where ReturnNoteId ='{ReturnNoteId}'";



                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PI = new ReturnNote
                    {
                        ReturnNoteId = reader["ReturnNoteId"] != DBNull.Value ? (Guid)reader["ReturnNoteId"] : Guid.Empty,
                        DeliveryChallanID = reader["DeliveryChallanID"] != DBNull.Value ? (string)reader["DeliveryChallanID"] : string.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        UOM = reader["UOM"] != DBNull.Value ? (string)reader["UOM"] : string.Empty,
                        ReturnableQuantity = reader["ReturnableQuantity"] != DBNull.Value ? (int)reader["ReturnableQuantity"] : 0,

                    };
                }

                return PI;

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
        public bool UpdateReturnNote(ReturnNote Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"update ReturnNote Set DeliveryChallanID='{Aq.DeliveryChallanID}', ItemName='{Aq.ItemName}' , Quantity='{Aq.Quantity}'  , UOM='{Aq.UOM}', ReturnableQuantity = '{Aq.ReturnableQuantity}' where ReturnNoteId = @ReturnNoteId ";
                cmd.Parameters.AddWithValue("@ReturnNoteId", Aq.ReturnNoteId);



                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public List<CustomerHistory> ListCoustomer()
        {
            try
            {
                List<CustomerHistory> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT Q.QuotationID,c.CustomerName, SUM(P.TaxableAmount) AS TotalTaxableAmount,Q.GrossTotal, I.InvoiceDate, I.InvoiceID FROM CustomerQuotation Q JOIN Invoice I ON Q.QuotationID = I.QuotationID JOIN QuotationProduct P ON Q.QuotationID = P.QuotationID JOIN Customers c ON c.CustomerID = Q.CustomerID GROUP BY Q.QuotationID, c.CustomerName, Q.GrossTotal, I.InvoiceDate, I.InvoiceID;";


                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new CustomerHistory()
                    {
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        TotalTaxableAmount = reader["TotalTaxableAmount"] != DBNull.Value ? (decimal)reader["TotalTaxableAmount"] : 0m,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? (decimal)reader["GrossTotal"] : 0m,
                        InvoiceDate = reader["InvoiceDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["InvoiceDate"]) : DateOnly.MinValue,
                        InvoiceID = reader["InvoiceID"] != DBNull.Value ? (Guid)reader["InvoiceID"] : Guid.Empty,

                    });
                }

                return CL;

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



        //SaleForCasting 
        public Guid AddSFDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = "INSERT INTO Requisitions ([Description],[RequisitionSeries]) OUTPUT INSERTED.RequisitionID VALUES (@Description, @RequisitionSeries)";
                cmd1.Parameters.AddWithValue("@Description", Aq.Description ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                //cmd1.Parameters.AddWithValue("@RequisitionType", Aq.RequisitionType);


                cmd1.Connection = connection;
                connection.Open();
                Guid RequisitionID = (Guid)cmd1.ExecuteScalar();

                return RequisitionID;
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
        public bool AddSFItems(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "INSERT INTO RequisitionItems ([ItemID],[Quantity],[RequisitionID]) VALUES (@ItemID,@Quantity,@RequisitionID)";

                cmd.Parameters.AddWithValue("@ItemID", Aq.ItemId);
                cmd.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
                //cmd.Parameters.AddWithValue("@ItemName", Aq.ItemName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", Aq.Quantity);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public Guid AddMRDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = "INSERT INTO Requisitions ([Description],[RequisitionSeries]) OUTPUT INSERTED.RequisitionID VALUES (@Description, @RequisitionSeries)";
                cmd1.Parameters.AddWithValue("@Description", Aq.Description ?? (object)DBNull.Value);
                cmd1.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                //cmd1.Parameters.AddWithValue("@RequisitionType", Aq.RequisitionType);


                cmd1.Connection = connection;
                connection.Open();
                Guid RequisitionID = (Guid)cmd1.ExecuteScalar();

                return RequisitionID;
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
        public bool AddMRItems(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "INSERT INTO RequisitionItems ([ItemID],[Quantity],[RequisitionID]) VALUES (@ItemID,@Quantity,@RequisitionID)";

                cmd.Parameters.AddWithValue("@ItemID", Aq.ItemId);
                cmd.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
                //cmd.Parameters.AddWithValue("@ItemName", Aq.ItemName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Quantity", Aq.Quantity);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

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
        public List<QuotationModel> OrderTable(Guid RequisitionID)
        {
            try
            {
                List<QuotationModel> OL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.ItemName,R.Quantity from Items I  join RequisitionItems R  ON R.ItemId=I.ItemId where RequisitionID= '{RequisitionID}'";
                //cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);

                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OL.Add(new QuotationModel()
                    {
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,


                    });
                }

                return OL;

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
        public bool updateMRDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);



                SqlCommand cmd2 = new SqlCommand(@"
    UPDATE Requisitions 
    SET 
        Description = @Description, 
        RequisitionSeries = @RequisitionSeries, 
        RequisitionType = 2,
RequisitionStatus=1
    WHERE RequisitionID = @RequisitionID", connection);

                cmd2.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@Description", Aq.Description);
                cmd2.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
                cmd2.Parameters.AddWithValue("@RequisitionType", Aq.RequisitionType);
                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();

                return true;

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

        public bool updateSFDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);



                SqlCommand cmd2 = new SqlCommand(@"
    UPDATE Requisitions 
    SET 
        Description = @Description, 
        RequisitionSeries = @RequisitionSeries, 
        RequisitionType = 1,
RequisitionStatus=1
    WHERE RequisitionID = @RequisitionID", connection);

                cmd2.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@Description", Aq.Description);
                cmd2.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
                cmd2.Parameters.AddWithValue("@RequisitionType", Aq.RequisitionType);
                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();

                return true;

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
        public List<QuotationModel> AddSFItemName()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select ItemName,ItemId from Items where ItemType=1";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new QuotationModel()
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        ItemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
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
         public List<QuotationModel> AddMRItemName()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select ItemName,ItemId from Items where ItemType=2";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new QuotationModel()
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        ItemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
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
        public List<QuotationModel> AddBillItemName()
        {
            try
            {
                List<QuotationModel> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.ItemName,I.ItemId,I.HSN,I.UnitOFMeasure, L.SellingPrice from Items I Join LotBatch L on I.ItemId=L.ItemId  ";

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

        public List<Items> ViewWarehouseInventory()
        {
            try
            {
                List<Items> item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"   SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId Where iv.InventoryCenter = 1";
                ;
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                        SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0,
                        HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,
                    });
                }

                return item;

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