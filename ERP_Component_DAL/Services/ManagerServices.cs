using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class ManagerServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;

        public ManagerServices(IConfiguration config)
        {
            this.configuration = config;
        }



        public List<AddPurchaseRequisition> RequisitionsForQuotation()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select RequisitionID, Description,RequisitionSeries, CreatedAt, TotalAmount From Requisitions Where RequisitionStatus = 2 ";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                        Descripion = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        requisitionSeries = reader["RequisitionSeries"] != DBNull.Value ? (string)reader["RequisitionSeries"] : string.Empty,
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : 0m,
                        Date = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),

                    });
                }

                return prod;

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
        public List<Vendor> GetRequisitionQuotationListData(Guid RequisitionID)
        {
            try
            {
                List<Vendor> product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT vq.RequisitionID,ve.VendorID, ve.VendorName,vq.DiscountAmount, vq.Amount, vq.PaymentTerms, vq.DeliveryTerms,vq.AdvancedAmount,vq.FinalAmount,vq.DiscountRate  FROM VendorQuotations vq JOIN Vendors ve ON vq.VendorID = Ve.VendorID  Where vq.RequisitionID = '{RequisitionID}' ORDER BY vq.FinalAmount";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new Vendor
                    {

                        requisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,

                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        paymentTerms = reader["PaymentTerms"] != DBNull.Value ? (string)reader["PaymentTerms"] : string.Empty,

                        deliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,

                        amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m,
                        discountAmount = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountAmount"]) : 0m,
                        advancedAmount = reader["AdvancedAmount"] != DBNull.Value ? Convert.ToDecimal(reader["AdvancedAmount"]) : 0m,
                        finalAmount = reader["FinalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["FinalAmount"]) : 0m,

                    });
                }

                return product;

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

        public List<AddPurchaseRequisition> GetRequisitionItemsListData(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT  it.ItemName, it.HSN, it.Specification, it.UnitOfMeasure,ri.RequisitionID, ri.UnitPrice,ri.TotalPrice,ri.Quantity, c.CategoryName, i.TypeName FROM Items it JOIN RequisitionItems ri ON it.ItemId = ri.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where ri.RequisitionID = '{RequisitionID}'";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new AddPurchaseRequisition
                    {

                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        itemtype = reader["TypeName"] != DBNull.Value ? (string)reader["TypeName"] : string.Empty,
                        hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,


                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        TotalAmount = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m,

                    });
                }

                return product;

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

        public bool UpdateVendorQuotationStatusAndRequisitionStatusToApproved(Guid vendorId, Guid requisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update VendorQuotations set QuotationStatus = 3 Where VendorID = '{vendorId}' And RequisitionID = '{requisitionId}'; Update Requisitions set RequisitionStatus = 3 Where RequisitionID = '{requisitionId}' ";

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
