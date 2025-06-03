using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net;

namespace ERP_Component_DAL.Services
{
    public class CustomerServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;

        public CustomerServices(IConfiguration config)
        {
            this.configuration = config;
        }


        public bool AddCustomer(AddCustomer customer)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    string spName = "AddCustomer";

                    using (SqlCommand cmd = new SqlCommand(spName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding parameters as per your stored procedure
                        cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
                        cmd.Parameters.AddWithValue("@Email", customer.Email);
                        cmd.Parameters.AddWithValue("@ContactNo", customer.ContactNo);
                        cmd.Parameters.AddWithValue("@GSTIN", customer.GstIn);
                        cmd.Parameters.AddWithValue("@Balance", customer.Balance);
                        cmd.Parameters.AddWithValue("@CustomerIndustry", customer.CustomerIndustry);
                        cmd.Parameters.AddWithValue("@PaymentTermsDays", customer.PaymentTermsDays);
                        cmd.Parameters.AddWithValue("@CustomerCode", customer.CustomerCode);
                        cmd.Parameters.AddWithValue("@PAN_Number", customer.PAN);
                        cmd.Parameters.AddWithValue("@OpeningDate", customer.OpeningDate);
                        cmd.Parameters.AddWithValue("@Country", customer.Country);
                        cmd.Parameters.AddWithValue("@State", customer.State);
                        cmd.Parameters.AddWithValue("@City", customer.City);
                        cmd.Parameters.AddWithValue("@Area", customer.Area);
                        cmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                        cmd.Parameters.AddWithValue("@Street", customer.Street);
                        cmd.Parameters.AddWithValue("@District", customer.District);
                        cmd.Parameters.AddWithValue("@AddressLine", customer.Address);
                        cmd.Parameters.AddWithValue("@AccountNo", customer.AccountNo);
                        cmd.Parameters.AddWithValue("@AccountHolderName", customer.AccountHolderName);
                        cmd.Parameters.AddWithValue("@BankName", customer.BankName);
                        cmd.Parameters.AddWithValue("@BranchName", customer.BranchName);
                        cmd.Parameters.AddWithValue("@IFSCCode", customer.IfscCode);
                        cmd.Parameters.AddWithValue("@Mode", customer.Mode);

                        // File parameters (GstCertificate and PanCard)
                        if (customer.GstCertificate != null && customer.GstCertificate.Length > 0)
                        {
                            cmd.Parameters.Add("@GSTCertificate", SqlDbType.VarBinary).Value = GetFileBytes(customer.GstCertificate);
                        }
                        else
                        {
                            cmd.Parameters.Add("@GSTCertificate", SqlDbType.VarBinary).Value = DBNull.Value;
                        }

                        if (customer.PanCard != null && customer.PanCard.Length > 0)
                        {
                            cmd.Parameters.Add("@PanCard", SqlDbType.VarBinary).Value = GetFileBytes(customer.PanCard);
                        }
                        else
                        {
                            cmd.Parameters.Add("@PanCard", SqlDbType.VarBinary).Value = DBNull.Value;
                        }


                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        private byte[] GetFileBytes(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }



        //select Vendor Data
        public List<AddCustomer> SelectCustomer()
        {
            try
            {

                List<AddCustomer> customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT cu.CustomerID, cu.CustomerName,cu.ContactName, cu.Email, cu.Phone,cu.GSTIN, cu.Balance, cu.PAN, cu.CreatedOn,ad.AddressID, ad.State, ad.Area, ad.AddressLine1,ad.City, ad.Pincode,ad.District,ad.Street,ad.Country,acc.AccountID, acc.AccountHolderName, acc.AccountNumber, acc.BranchName, acc.IFSCCode FROM Customers cu JOIN Address ad ON cu.AddressID = ad.AddressID LEFT JOIN AccountDetails acc  ON cu.AccountID = acc.AccountID ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new AddCustomer()
                    {

                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ContactName = reader["ContactName"] != DBNull.Value ? (string)reader["ContactName"] : string.Empty,
                        ContactNo = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : string.Empty,
                        CustomerId = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty,
                        AccountId = reader["AccountID"] != DBNull.Value ? (Guid)reader["AccountID"] : Guid.Empty,
                        AddressId= reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0,
                        City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        Area= reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m,
                        GstIn = reader["GstIn"] != DBNull.Value ? (string)reader["GstIn"] : string.Empty,
                        PAN = reader["PAN"] != DBNull.Value ? (string)reader["PAN"] : string.Empty,
                        Address= reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,
                        
                        Street = reader["Street"] != DBNull.Value ? (string)reader["Street"] : string.Empty,
                        OpeningDate = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

                        Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty,
                        Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty,
                        AccountHolderName = reader["AccountHolderName"] != DBNull.Value ? (string)reader["AccountHolderName"] : string.Empty,
                        AccountNo= reader["AccountNumber"] != DBNull.Value ? (string)reader["AccountNumber"] : string.Empty,
                       
                        BranchName = reader["BranchName"] != DBNull.Value ? (string)reader["BranchName"] : string.Empty,
                        IfscCode = reader["IFSCCode"] != DBNull.Value ? (string)reader["IFSCCode"] : string.Empty,

                    });
                }
                return customer;
            }
            catch (Exception ex)
            {

                throw new Exception("Error fetching customer data", ex);
            }

            finally
            {
                connection.Close();
            }

        }
        //select  Customer
        public AddCustomer SelectCustomers(Guid customerId)
        {
            try
            {
                AddCustomer customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT cu.CustomerID, cu.CustomerName,cu.ContactName, cu.Email, cu.Phone,cu.GSTIN, cu.Balance, cu.PAN, cu.CreatedOn,ad.AddressID, ad.State, ad.Area, ad.AddressLine1,ad.City, ad.Pincode,ad.District,ad.Street,ad.Country,acc.AccountID, acc.AccountHolderName, acc.AccountNumber, acc.BranchName, acc.IFSCCode,dc.DocumentID FROM Customers cu JOIN Address ad ON cu.AddressID = ad.AddressID LEFT JOIN AccountDetails acc  ON cu.AccountID = acc.AccountID Left JOIN Documents dc ON cu.DocumentID = dc.DocumentID Where cu.CustomerID = '{customerId}' "; 
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;
                       customer.ContactName = reader["ContactName"] != DBNull.Value ? (string)reader["ContactName"] : string.Empty;
                        customer.ContactNo = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : string.Empty;
                        customer.CustomerId = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty;
                        customer.DocumentId = reader["DocumentID"] != DBNull.Value ? (Guid)reader["DocumentID"] : Guid.Empty;
                       customer.AccountId = reader["AccountID"] != DBNull.Value ? (Guid)reader["AccountID"] : Guid.Empty;
                       customer.AddressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0;
                        customer.City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                       customer.District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty;
                       customer.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                       customer.Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty;
                       customer.Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m;
                       customer.GstIn = reader["GstIn"] != DBNull.Value ? (string)reader["GstIn"] : string.Empty;
                       customer.PAN = reader["PAN"] != DBNull.Value ? (string)reader["PAN"] : string.Empty;
                        customer.Address = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                        
                       customer.Street = reader["Street"] != DBNull.Value ? (string)reader["Street"] : string.Empty;
                       customer.OpeningDate = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime);

                        customer.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
                        customer.Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty;
                       customer.AccountHolderName = reader["AccountHolderName"] != DBNull.Value ? (string)reader["AccountHolderName"] : string.Empty;
                       customer.AccountNo = reader["AccountNumber"] != DBNull.Value ? (string)reader["AccountNumber"] : string.Empty;
                       
                       customer.BranchName = reader["BranchName"] != DBNull.Value ? (string)reader["BranchName"] : string.Empty;
                        customer.IfscCode = reader["IFSCCode"] != DBNull.Value ? (string)reader["IFSCCode"] : string.Empty;



                }

                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //delete customer
        public bool DeleteCustomer(Guid CustomerId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete from Customers where CustomerId = '{CustomerId}'";

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
        //update Customer
        public bool EditCustomer(AddCustomer customer)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Customers set CustomerName='{customer.CustomerName}',ContactName='{customer.ContactName}',Email='{customer.Email}',Phone ='{customer.ContactNo}', GSTIN='{customer.GstIn}',Balance='{customer.Balance}',CustomerIndustry='{customer.CustomerIndustry}',PaymentTermsDays='{customer.PaymentTermsDays}',CustomerCode='{customer.CustomerCode}',PAN='{customer.PAN}' where CustomerID = '{customer.CustomerId}'; " +
                    $"Update Address Set  State = '{customer.State}',City='{customer.City}',Area='{customer.Area}',Pincode='{customer.PinCode}',AddressLine1='{customer.Address}',District='{customer.District}',Street='{customer.Street}' Where AddressID = '{customer.AddressId}';" +
                    $"Update AccountDetails Set AccountNumber='{customer.AccountNo}',AccountHolderName='{customer.AccountHolderName}',BankName='{customer.BankName}',BranchName='{customer.BranchName}',IFSCCode='{customer.IfscCode}' Where AccountID = '{customer.AccountId}';";
                    //$"Update Documents Set GSTCertificate = '{customer.GstCertificate}', PANCard = '{customer.PanCard}' Where DocumentID = '{customer.DocumentId}'";

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


        //select customer name
        public List<AddCustomer> SelectCustomerName()
        {
            try
            {
                List<AddCustomer> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CustomerId, CustomerName from  Customers";
                cmd.Connection = connection;


                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new AddCustomer()
                    {

                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        CustomerId = reader["CustomerId"] != DBNull.Value ? (Guid)reader["CustomerId"] : Guid.Empty,

                    });
                }

                return cat;

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


        public bool AddContactDetails(AddCustomer add)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into ContactDetails([CustomerId],[ContactNumber],[AlternateContactNUmber],[Email],[CompanyEmail],[SocialMedia],[WhatsappNumber])values('{add.CustomerId}','{add.ContactNo}','{add.alternate}','{add.Email}','{add.CompanyEmail}','{add.social}','{add.whatsapp}')";

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

        public List<AddCustomer> ContactDetailsView()
        {
            try
            {

                List<AddCustomer> customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT cu.CustomerID, cu.CustomerName,cd.ContactNumber,cd.ContactDetailID,cd.AlternateContactNumber,cd.Email,cd.CompanyEmail,cd.SocialMedia,cd.WhatsappNumber FROM Customers cu JOIN ContactDetails cd ON cu.CustomerID = cd.CustomerID  ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new AddCustomer()
                    {

                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        alternate = reader["AlternateContactNumber"] != DBNull.Value ? (string)reader["AlternateContactNumber"] : string.Empty,
                        ContactNo = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty,
                        CustomerId = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty,
                        ContactDetailId = reader["ContactDetailID"] != DBNull.Value ? (Guid)reader["ContactDetailID"] : Guid.Empty,
                      
                        CompanyEmail = reader["CompanyEmail"] != DBNull.Value ? (string)reader["CompanyEmail"] : string.Empty,
                        social = reader["SocialMedia"] != DBNull.Value ? (string)reader["SocialMedia"] : string.Empty,
                        whatsapp= reader["WhatsappNumber"] != DBNull.Value ? (string)reader["WhatsappNumber"] : string.Empty,
                      
                        Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty,
                       
                    });
                }
                return customer;
            }
            catch (Exception ex)
            {

                throw new Exception("Error fetching customer data", ex);
            }

            finally
            {
                connection.Close();
            }

        }

        public AddCustomer EditContactDetails(Guid contactDetailId)
        {
            try
            {
                AddCustomer contact = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT cu.CustomerID, cu.CustomerName,cd.ContactNumber,cd.ContactDetailID,cd.AlternateContactNumber,cd.Email,cd.CompanyEmail,cd.SocialMedia,cd.WhatsappNumber FROM Customers cu JOIN ContactDetails cd ON cu.CustomerID = cd.CustomerID  Where cd.ContactDetailID = '{contactDetailId}' ";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contact.CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;
                        contact.alternate = reader["AlternateContactNumber"] != DBNull.Value ? (string)reader["AlternateContactNumber"] : string.Empty;
                        contact.ContactNo = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty;
                        contact.CustomerId = reader["CustomerID"] != DBNull.Value ? (Guid)reader["CustomerID"] : Guid.Empty;
                        contact.ContactDetailId = reader["ContactDetailID"] != DBNull.Value ? (Guid)reader["ContactDetailID"] : Guid.Empty;
                      
                        contact.CompanyEmail = reader["CompanyEmail"] != DBNull.Value ? (string)reader["CompanyEmail"] : string.Empty;
                        contact.social = reader["SocialMedia"] != DBNull.Value ? (string)reader["SocialMedia"] : string.Empty;
                        contact.whatsapp = reader["WhatsappNumber"] != DBNull.Value ? (string)reader["WhatsappNumber"] : string.Empty;
                      
                        contact.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;


                }


                return contact;

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

        public bool UpdateContactDetails(AddCustomer add)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update ContactDetails set  ContactNumber ='{add.ContactNo}', AlternateContactNumber ='{add.alternate}', Email='{add.Email}' ,CompanyEmail='{add.CompanyEmail}',SocialMedia = '{add.social}',whatsappNumber ='{add.whatsapp}',CustomerID ='{add.CustomerId}'  where ContactDetailID = '{add.ContactDetailId}'";

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

        public bool DeleteContactDetails(Guid contactDetailId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete from ContactDetails where ContactDetailID = '{contactDetailId}'";

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
    }
}
