using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using Microsoft.VisualBasic;

namespace ERP_Component_DAL.Services
{
    public class AccountServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;
        private readonly string _connectionString;


        public AccountServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = configuration.GetConnectionString("DefaultConnectionString") ?? throw new ArgumentNullException(nameof(_connectionString));
            connection = new SqlConnection(_connectionString); // Initialize connection
        }

     
        public bool createexpense(Expense e)
        {
            var date =e.entrydate;
            var date2 = date.ToString("yyyy-MM-dd");
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into expense([entrydate],[project],[particular],[debited],[credited],[reference],[narration]) values('{date2}','{e.project}','{e.particular}','{e.debit}','{e.credit}','{e.reference}','{e.narration}');";
  
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
        public bool creategeneral(Expense e)
        {
            var date = e.entrydate;
            var date2 = date.ToString("yyyy-MM-dd");
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into Generalentry([entrydate],[project],[particular],[debited],[credited],[reference],[narration]) values('{date2}','{e.project}','{e.particular}','{e.debit}','{e.credit}','{e.reference}','{e.narration}');";

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool addaccountdetails(Expense e)
        {
            
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into Accounttype([Accountcode],[AccountName],[Accounttype]) output inserted.AccountId values('{e.accountcode}','{e.accountname}','{e.groupname}')";

                cmd.Connection = connection;

                connection.Open();
                int accountid = (int)cmd.ExecuteScalar();
                connection.Close();


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"insert into Accountdetails(AccountId,[AccountNo],[AccountHolder],[Bank],[Branch],[IFSC],[balance])values( @accountid,'{e.accountnumber}','{e.Accountholdername}','{e.bank}','{e.branch}','{e.ifsc}','{e.balance}')";
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@accountid", accountid);
                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
     
        }



        public List<Expense> getexpense()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select expenseid,entrydate,project,particular,debited,credited,reference,narration from expense";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                entrydate = reader["entrydate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["entrydate"]) : DateOnly.MinValue,
                                project = reader["project"] != DBNull.Value ? (string)reader["project"] : string.Empty,
                                particular = reader["particular"] != DBNull.Value ? (string)reader["particular"] : string.Empty,
                                debit = reader["debited"] != DBNull.Value ? (string)reader["debited"] : string.Empty,
                                credit = reader["credited"] != DBNull.Value ? (string)reader["credited"] : string.Empty,
                                reference = reader["reference"] != DBNull.Value ? (string)reader["reference"] : string.Empty,
                                narration = reader["narration"] != DBNull.Value ? (string)reader["narration"] : string.Empty,
                                expenseid = reader["expenseid"] != DBNull.Value ? (int)reader["expenseid"] : 0,
                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public List<Expense> showacountgroup()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select Accountcode,AccountName,Accounttype from Accounttype ";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                accountcode = reader["Accountcode"] != DBNull.Value ? (string)reader["Accountcode"] : string.Empty,
                                accountname = reader["AccountName"] != DBNull.Value ? (string)reader["AccountName"] : string.Empty,
                                groupname = reader["Accounttype"] != DBNull.Value ? (string)reader["Accounttype"] : string.Empty,

                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public List<Expense> getgeneral()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select generalid,entrydate,project,particular,debited,credited,reference,narration from Generalentry";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                entrydate = reader["entrydate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["entrydate"]) : DateOnly.MinValue,
                                project = reader["project"] != DBNull.Value ? (string)reader["project"] : string.Empty,
                                particular = reader["particular"] != DBNull.Value ? (string)reader["particular"] : string.Empty,
                                debit = reader["debited"] != DBNull.Value ? (string)reader["debited"] : string.Empty,
                                credit = reader["credited"] != DBNull.Value ? (string)reader["credited"] : string.Empty,
                                reference = reader["reference"] != DBNull.Value ? (string)reader["reference"] : string.Empty,
                                narration = reader["narration"] != DBNull.Value ? (string)reader["narration"] : string.Empty,
                                generalid = reader["generalid"] != DBNull.Value ? (int)reader["generalid"] : 0,
                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }

        //public List<MakePayment> GetVendorNameList()
        //{
        //    try
        //    {
        //        List<MakePayment> CL = new();
        //        String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(ConnectionString);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = "select VendorID,VendorName from Vendors";


        //        cmd.Connection = connection;


        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            CL.Add(new MakePayment()
        //            {

        //              VendorID = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
        //                VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty

        //            });
        //        }

        //        return CL;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }


        //}
        public List<MakePayment> GetVendorNameList(Guid VendorID)
        {
            try
            {
                List<MakePayment> vendorList = new();
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;

                    if (VendorID == Guid.Empty)
                    {
                        cmd.CommandText = "SELECT VendorID, VendorName FROM Vendors";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT VendorID, VendorName FROM Vendors WHERE VendorID = @VendorID";
                        cmd.Parameters.AddWithValue("@VendorID", VendorID);
                    }

                    cmd.Connection = connection;
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vendorList.Add(new MakePayment()
                            {
                                VendorID = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                                VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty
                            });
                        }
                    }
                }

                return vendorList;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }


        public MakePayment GetVendorPendingAmount( Guid VendorID)
        {
            try
            {
                MakePayment Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT SUM(po.NetTotal) AS TotalBalance, SUM(pb.PaidAmount) AS TotalPaid, SUM(po.NetTotal) - SUM(pb.PaidAmount) AS PendingAmount FROM PurchaseOrders po JOIN PurchaseBills pb ON po.PurchaseOrderID = pb.PurchaseOrderID WHERE po.VendorId = '{VendorID}' AND pb.BillStatus = 1";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    Item.PendingAmount = reader["PendingAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PendingAmount"]) : 0m;
                    Item.TotalBalance = reader["TotalBalance"] != DBNull.Value ? Convert.ToDecimal(reader["TotalBalance"]) : 0m;
                    Item.TotalPaid = reader["TotalPaid"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPaid"]) : 0m;
                }


                return Item;

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
   
        public List<MakePayment> BalanceSummary(Guid VendorID)
        {
            try
            {
                List<MakePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"SELECT po.PurchaseOrderID,po.NetTotal AS TotalAmount , pb.PaidAmount AS PaidAmount, (po.NetTotal - pb.PaidAmount) AS RemainingAmount FROM PurchaseBills pb JOIN PurchaseOrders po  ON pb.PurchaseOrderID = po.PurchaseOrderID WHERE po.VendorId = '{VendorID}' AND pb.BillStatus = 1";
                cmd.CommandText = $"SELECT pb.BillNumber,po.PurchaseOrderID,po.NetTotal AS TotalAmount , pb.PaidAmount AS PaidAmount, (po.NetTotal - pb.PaidAmount) AS RemainingAmount FROM PurchaseBills pb JOIN PurchaseOrders po  ON pb.PurchaseOrderID = po.PurchaseOrderID WHERE po.VendorId = '{VendorID}' AND pb.BillStatus = 1";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new MakePayment()
                    {   BillNumber = reader["BillNumber"] != DBNull.Value ? (string)reader["BillNumber"] : string.Empty,
                        PurchaseOrderID= reader["PurchaseOrderID"] != DBNull.Value ? (Guid)reader["PurchaseOrderID"] : Guid.Empty,
                        RemainingAmount = reader["RemainingAmount"] != DBNull.Value ? Convert.ToDecimal(reader["RemainingAmount"]) : 0m,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : 0m,
                        PaidAmount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,

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


        public List<MakePayment> VendorAmountDetails()
        {
            try
            {
                List<MakePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Select V.Balance,P.TaxableAmount,P.NetTotal,B.PaidAmount,B.DueDate from Vendors V join PurchaseOrders P on P.VendorID=V.VendorID join PurchaseBills B on B.PurchaseOrderID=P.PurchaseOrderID ";


                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new MakePayment()
                    {

                      
                            VendorID = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                            Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m,
                            TaxableAmount = reader["TaxableAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TaxableAmount"]) : 0m,
                            NetTotal = reader["NetTotal"] != DBNull.Value ? Convert.ToDecimal(reader["NetTotal"]) : 0m,
                            PaidAmount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,
                            DueDate = reader["DueDate"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["DueDate"])) : DateOnly.MinValue
                            //VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty

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
        
        public bool UpdateAmount(MakePayment mp)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
              

                cmd.CommandText = $"update PurchaseBills Set PaidAmount = PaidAmount + @PaidAmount where PurchaseOrderID = @PurchaseOrderID" +
                    $" UPDATE PurchaseBills SET BillStatus = 3 WHERE PurchaseOrderID = @PurchaseOrderID AND PaidAmount = " +
                    $"(SELECT NetTotal FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)";

                cmd.Parameters.AddWithValue("@PaidAmount", mp.finalPaidAmount);
                cmd.Parameters.AddWithValue("@PurchaseOrderID", mp.PurchaseOrderID);



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
   public List<MakePayment> GetAdvancedPaymentDetails(Guid VendorID)
        {
            try
            {
                List<MakePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"SELECT OrderDate,Description,NetTotal FROM PurchaseOrders WHERE VendorID = '{VendorID}'";
                cmd.CommandText = $"SELECT (po.NetTotal * (po.AdvancePercent / 100.0)) AS AdvanceAmount,po.PurchaseOrderID, po.AmountPaid, po.NetTotal FROM PurchaseOrders po WHERE po.VendorID = '{VendorID}'";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new MakePayment()
                    {
                        PurchaseOrderID = reader["PurchaseOrderID"] != DBNull.Value ? (Guid)reader["PurchaseOrderID"] : Guid.Empty,

                        //OrderDate = reader["OrderDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["OrderDate"]) : DateOnly.MinValue,
                        NetTotal = reader["NetTotal"] != DBNull.Value ? Convert.ToDecimal(reader["NetTotal"]) : 0m,
                        AmountPaid = reader["AmountPaid"] != DBNull.Value ? Convert.ToDecimal(reader["AmountPaid"]) : 0m,
                        AdvanceAmount = reader["AdvanceAmount"] != DBNull.Value ? Convert.ToDecimal(reader["AdvanceAmount"]) : 0m,
                      

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
        

    public bool UpdateAdvancedAmount(MakePayment mp)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"update PurchaseOrders Set AmountPaid= AmountPaid + @AmountPaid WHERE PurchaseOrderID = @PurchaseOrderID; UPDATE PurchaseOrders SET OrderStatus = 3 WHERE PurchaseOrderID = @PurchaseOrderID";

                cmd.Parameters.AddWithValue("@AmountPaid", mp.AdvanceAmount);
                cmd.Parameters.AddWithValue("@PurchaseOrderID", mp.PurchaseOrderID);



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

        public ReceivePayment GetOutstandingPaymentAmount(Guid CustomerID)
        {
            try
            {
                ReceivePayment Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"select Balance From Customers where CustomerID='{CustomerID}'";
                cmd.CommandText = $"select sum(C.GrossTotal) as Balance from customerQuotation C join Invoice I on C.QuotationID=I.QuotationID  where C.CustomerID='{CustomerID}'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    Item.Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m;
                    //Item.TotalBalance = reader["TotalBalance"] != DBNull.Value ? Convert.ToDecimal(reader["TotalBalance"]) : 0m;
                    //Item.TotalPaid = reader["TotalPaid"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPaid"]) : 0m;
                }


                return Item;

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



        //
        public List<ReceivePayment> GetListOfCustomer(Guid CustomerID)
        {
            try
            {
                List<ReceivePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                if (CustomerID == Guid.Empty)
                {
                    cmd.CommandText = "select CustomerID,CustomerName from Customers";
                }
                else
                {
                    cmd.CommandText = "select CustomerID,CustomerName from Customers where CustomerID = @CustomerID";
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                }

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new ReceivePayment()
                    {

                        CustomerID = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty,
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty

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
        public List<ReceivePayment> GetCustomerPaymentDetails(Guid CustomerID)
        {
            try
            {
                List<ReceivePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"select I.AdvanceAmount, I.InvoiceID,P.TaxableAmount ,I.InvoiceNumber from QuotationProduct P Join Invoice I on P.QuotationID=I.QuotationID where I.CustomerID='{CustomerID}'";
                cmd.CommandText = $"select I.AdvanceAmount, I.InvoiceID,P.GrossTotal ,I.InvoiceNumber from CustomerQuotation P Join Invoice I on P.QuotationID = I.QuotationID where P.CustomerID = '{CustomerID}'";


                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new ReceivePayment()
                    {

                        InvoiceID = reader["InvoiceID"] != DBNull.Value ? (Guid)reader["InvoiceID"] : Guid.Empty,
                        InvoiceNumber = reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        TotalAmount = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        AdvanceAmount = reader["AdvanceAmount"] != DBNull.Value ? Convert.ToDecimal(reader["AdvanceAmount"]) : 0m,

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
        

      public bool UpdateInvoiceWithNewBalance(ReceivePayment mp)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                //cmd.CommandText = $" update Customers set Balance=@Balance where CustomerID=@CustomerID"; 
                cmd.CommandText = $" update customerQuotation set GrossTotal=GrossTotal-@OldGrossTotal where QuotationID=@QuotationID";
                //cmd.Parameters.AddWithValue("@GrossTotal", mp.NewGrossTotal);
                cmd.Parameters.AddWithValue("@QuotationID", mp.QuotationID);
                cmd.Parameters.AddWithValue("@OldGrossTotal", mp.OldGrossTotal);


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

        public bool UpdateAdvancedAmountOFCustomer(ReceivePayment mp)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"update Invoice Set AdvanceAmount= AdvanceAmount + @AdvanceAmount WHERE InvoiceID = @InvoiceID";

                cmd.Parameters.AddWithValue("@AdvanceAmount", mp.AdvanceAmount);
                cmd.Parameters.AddWithValue("@InvoiceID", mp.InvoiceID);



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


        public List<ReceivePayment> getAmountSummaryOfCustomer(Guid CustomerID)
        {
            try
            {
                List<ReceivePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"select Q.GrossTotal,Q.QuotationID ,I.InvoiceNumber from Customers c join CustomerQuotation Q on c.CustomerID=Q.CustomerID join Invoice I on Q.QuotationID=I.QuotationID where Q.CustomerID ='{CustomerID}'";
                cmd.CommandText = $"select Q.GrossTotal,Q.QuotationID ,I.InvoiceNumber ,I.InvoiceID from customerQuotation Q join Invoice I on  Q.QuotationID=I.QuotationID where Q.CustomerID = '{CustomerID}'";
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new ReceivePayment()
                    {

                        InvoiceID = reader["InvoiceID"] != DBNull.Value ? (Guid)reader["InvoiceID"] : Guid.Empty,
                        //CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        InvoiceNumber =reader["InvoiceNumber"] != DBNull.Value ? (string)reader["InvoiceNumber"] : string.Empty,
                        QuotationID = reader["QuotationID"] != DBNull.Value ? (Guid)reader["QuotationID"] : Guid.Empty,
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


        public bool SetJournalEntry(JournalEntry e)
        {

            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into Transactions([TransactionDate],[Amount],[TransactionType],[Remarks]) values('{e.TransactionDate}','{e.Amount}','{e.TransactionType}','{e.Remarks}')";

                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool SetExpenseEntry(JournalEntry e)
        {

            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into Transactions([TransactionDate],[Amount],[TransactionType],[Remarks]) output inserted.TransactionID values('{e.TransactionDate}','{e.Amount}','{e.TransactionType}','{e.Remarks}')";

                cmd.Connection = connection;

                connection.Open();
                Guid TransactionID = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"insert into TransactionsExpensesTypesBridge([TransactionID])values(@TransactionID)";
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@TransactionID", TransactionID);
                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool SetChartOfAccount(Account Account)
        {

            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into AccountDetails([AccountNumber],[AccountHolderName],[BankName],[BranchName],[IFSCCode]) values('{Account.AccountNumber}','{Account.AccountHolderName}','{Account.BankName}','{Account.BranchName}','{Account.IFSCCode}')";

                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<ReceivePayment> receivableAccountHistory()
        {
            try
            {
                List<ReceivePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.CommandText = $"select C.CustomerID, C.CustomerName,Sum(I.AdvanceAmount) AS TotalAdvanceAmount from Invoice I join Customers C on C.CustomerID=C.CustomerID Group By C.CustomerName C.CustomerID";
                cmd.CommandText = $"select Q.CustomerID, C.CustomerName,Sum(I.AdvanceAmount) AS TotalAdvanceAmount from Invoice I join CustomerQuotation Q on Q.QuotationID=I.QuotationID join Customers C on C.CustomerID=Q.CustomerID Group By C.CustomerName, Q.CustomerID";
                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new ReceivePayment()
                    {
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        CustomerID = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty,
                        AdvanceAmount = reader["TotalAdvanceAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAdvanceAmount"]) : 0m,
                      
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
        public List<MakePayment> MakePaymentAccountHistory()
        {
            try
            {
                List<MakePayment> CL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select V.VendorID,V.VendorName,sum(P.AmountPaid) As TotalAmountPaid from PurchaseOrders P join Vendors V on V.VendorID=P.VendorID Group By V.VendorName, V.VendorID  ";

                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CL.Add(new MakePayment()
                    {
                        VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        VendorID = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        TotalAmountPaid = reader["TotalAmountPaid"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmountPaid"]) : 0m,

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

    }
}
