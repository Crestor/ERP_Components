using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ERP_Component_DAL.Models;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Diagnostics.Metrics;
using System.Net;


namespace ERP_Component_DAL.Services
{
    public class VendorServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;

        public VendorServices(IConfiguration config)
        {
            this.configuration = config;
        }
        //create vendor
        public bool CreateVendor(AddVendor vendor)
    {
        try
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand cmd = new SqlCommand("AddVendor", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@VendorName", vendor.VendorName);
                    cmd.Parameters.AddWithValue("@ContactName", vendor.ContactName);
                    cmd.Parameters.AddWithValue("@Email", vendor.Email);
                    cmd.Parameters.AddWithValue("@Phone", vendor.Phone);
                    cmd.Parameters.AddWithValue("@GST", string.IsNullOrEmpty(vendor.GST) ? (object)DBNull.Value : vendor.GST);
                    cmd.Parameters.AddWithValue("@Balance", vendor.Balance);
                    cmd.Parameters.AddWithValue("@VendorIndustry", vendor.VendorIndustry);
                    cmd.Parameters.AddWithValue("@PaymentTermsDays", vendor.PaymentTermsDays);
                    cmd.Parameters.AddWithValue("@VendorCode", vendor.VendorCode);
                    cmd.Parameters.AddWithValue("@PAN", string.IsNullOrEmpty(vendor.PAN) ? (object)DBNull.Value : vendor.PAN);
                    cmd.Parameters.AddWithValue("@Country", vendor.Country);
                    cmd.Parameters.AddWithValue("@State", vendor.State);
                    cmd.Parameters.AddWithValue("@City", vendor.City);
                    cmd.Parameters.AddWithValue("@Pincode", vendor.Pincode);
                    cmd.Parameters.AddWithValue("@District", vendor.District);
                    cmd.Parameters.AddWithValue("@AddressLine", vendor.AddressLine);
                    cmd.Parameters.AddWithValue("@AccountNumber", vendor.AccountNumber);
                    cmd.Parameters.AddWithValue("@AccountHolderName", vendor.AccountHolderName);
                    cmd.Parameters.AddWithValue("@BankName", vendor.BankName);
                    cmd.Parameters.AddWithValue("@BranchName", vendor.BranchName);
                    cmd.Parameters.AddWithValue("@IFSCCode", vendor.IFSCCode);


                    cmd.Parameters.Add("@GSTCertificate", SqlDbType.VarBinary).Value =
                    (vendor.GSTCertificate != null && vendor.GSTCertificate.Length > 0)
                        ? GetFileBytes(vendor.GSTCertificate)
                            : (object)DBNull.Value;

                    cmd.Parameters.Add("@PANCard", SqlDbType.VarBinary).Value =
                        (vendor.PANCard != null && vendor.PANCard.Length > 0)
                        ? GetFileBytes(vendor.PANCard)
                        : (object)DBNull.Value;


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

        //private object GetFileBytes(IFormFile gSTCertificate)
        //{
        //    throw new NotImplementedException();
        //}

        private static byte[] GetFileBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }





        public List<AddCustomer> SelectVendor()
        {
            try
            {

                List<AddCustomer> customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT vn.VendorID, vn.VendorName,vn.ContactName, vn.Email,vn.Phone,vn.TaxID, vn.Balance,vn.VendorIndustry,vn.PAN,vn.VendorCode, vn.CreatedOn,ad.AddressID, ad.State, ad.Area, ad.AddressLine1,ad.City, ad.Pincode,ad.District,ad.Country,acc.AccountID, acc.AccountHolderName, acc.AccountNumber, acc.BranchName, acc.IFSCCode FROM Vendors vn JOIN Address ad ON vn.AddressID = ad.AddressID LEFT JOIN AccountDetails acc  ON vn.AccountID = acc.AccountID ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new AddCustomer()
                    {

                        CustomerName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        ContactName = reader["ContactName"] != DBNull.Value ? (string)reader["ContactName"] : string.Empty,
                        ContactNo = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : string.Empty,
                        CustomerId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        AccountId = reader["AccountID"] != DBNull.Value ? (Guid)reader["AccountID"] : Guid.Empty,
                        AddressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0,
                        City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m,
                        GstIn = reader["TaxID"] != DBNull.Value ? (string)reader["TaxID"] : string.Empty,
                        PAN = reader["PAN"] != DBNull.Value ? (string)reader["PAN"] : string.Empty,
                        vendorIndustry = reader["VendorIndustry"] != DBNull.Value ? (string)reader["VendorIndustry"] : string.Empty,
                        Address = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,

                       
                        OpeningDate = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

                        Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty,
                        Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty,
                        AccountHolderName = reader["AccountHolderName"] != DBNull.Value ? (string)reader["AccountHolderName"] : string.Empty,
                        AccountNo = reader["AccountNumber"] != DBNull.Value ? (string)reader["AccountNumber"] : string.Empty,

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


        //public bool UpdateVendor(AddVendor Vendor)
        //{
        //    try
        //    {
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Update Vendors set VendorName='{Vendor.VendorName}',TaxID='{Vendor.GST}',PAN='{Vendor.PAN}',Phone ='{Vendor.Phone}', Balance='{Vendor.Balance}',VendorIndustry = '{Vendor.VendorIndustry}',VendorCode = '{Vendor.VendorCode}' where VendorID = '{Vendor.VendorId}'; Update Address Set ItemName = '{asset.itemName}' Where ItemId = '{asset.itemId}'";

        //        cmd.Connection = connection;
        //        connection.Open();
        //        cmd.ExecuteScalar();
        //        connection.Close();
        //        return true;

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


        //Edit Vendor

        public AddVendor EditVendor(Guid VendorId)
    {
        try
        {

            AddVendor vendor = new();
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

            connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"SELECT vn.VendorID, vn.VendorName,vn.ContactName, vn.Email,vn.Phone,vn.TaxID, vn.Balance,vn.VendorIndustry,vn.PAN,vn.VendorCode, ad.State,ad.AddressID, ad.AddressLine1,ad.City, ad.Pincode,ad.District,ad.Country, acc.AccountHolderName, acc.AccountNumber, acc.BranchName, acc.IFSCCode FROM Vendors vn JOIN Address ad ON vn.AddressID = ad.AddressID LEFT JOIN AccountDetails acc  ON vn.AccountID = acc.AccountID Where vn.VendorID = '{VendorId}'";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                    vendor.VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty;
                    vendor.ContactName = reader["ContactName"] != DBNull.Value ? (string)reader["ContactName"] : string.Empty;
                    vendor.Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : string.Empty;
                    vendor.VendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty;
                
                    vendor.City = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                    vendor.District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty;
                    vendor.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    vendor.Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0m;
                    vendor.GST = reader["TaxID"] != DBNull.Value ? (string)reader["TaxID"] : string.Empty;
                    vendor.PAN = reader["PAN"] != DBNull.Value ? (string)reader["PAN"] : string.Empty;
                    vendor.VendorIndustry = reader["VendorIndustry"] != DBNull.Value ? (string)reader["VendorIndustry"] : string.Empty;
                    vendor.AddressLine = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;

                    vendor.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : string.Empty;
                    vendor.Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty;
                    vendor.AccountHolderName = reader["AccountHolderName"] != DBNull.Value ? (string)reader["AccountHolderName"] : string.Empty;
                    vendor.AccountNumber = reader["AccountNumber"] != DBNull.Value ? (string)reader["AccountNumber"] : string.Empty;

                    vendor.BranchName = reader["BranchName"] != DBNull.Value ? (string)reader["BranchName"] : string.Empty;
                    vendor.IFSCCode = reader["IFSCCode"] != DBNull.Value ? (string)reader["IFSCCode"] : string.Empty;

            }

                return vendor;
        }
        catch (Exception ex)
        {

            throw new Exception("Error fetching vendor data", ex);
        }

        finally
        {
            connection.Close();
        }

    }
    //delete vendor
    public bool DeleteVendor(int VendorId)
    {
        try
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
            connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"Delete from Vendors where VendorId = '{VendorId}'";

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

    //select vendor
    public List<AddVendor> SelectVendorName()
    {
        try
        {
            List<AddVendor> cat = new();
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
            connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"select VendorId, VendorName from  Vendors";
            cmd.Connection = connection;


            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cat.Add(new AddVendor()
                {

                    VendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                    //VendorId = reader["VendorId"] != DBNull.Value ? (int)reader["VendorId"] : 0,

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









    //select list of vendor code
    public List<AddVendor> SelectVendorCode()
    {
        try
        {
            List<AddVendor> code = new();
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
            connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Select VendorCode from Vendors";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                code.Add(new AddVendor
                {
                    VendorCode = reader["vendorCode"] != DBNull.Value ? (string)reader["vendorCode"] : string.Empty,


                });
            }
            return code;
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    //vendor document//
    //public bool VendorDoc(AddVendor doc)
    //{

    //    try
    //    {
    //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

    //        using (SqlConnection connection = new SqlConnection(connectionString))
    //        {
    //            string query = @"INSERT INTO VendorDoc 
    //                        ( GstCertificate) 
    //                        VALUES ( @gst)";

    //            using (SqlCommand cmd = new SqlCommand(query, connection))
    //            {
    //                //cmd.Parameters.AddWithValue("@VendorName", doc.VendorName);


    //                //cmd.Parameters.Add("@pan", SqlDbType.VarBinary).Value = GetFileBytes(doc.PanCard);
    //                //cmd.Parameters.Add("@tax", SqlDbType.VarBinary).Value = GetFileBytes(doc.TaxProof);

    //                connection.Open();
    //                cmd.ExecuteNonQuery();
    //            }
    //        }

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Error uploading vendor documents", ex);
    //    }
    //}



    //block Vendor
    public bool BlockVendor(int VendorId)
    {
        try
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
            connection = new SqlConnection(connectionstring);
            SqlCommand cmd = new();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = $"UPDATE Vendors SET Status = 2 WHERE VendorId = {VendorId}";


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
    }
}
}
