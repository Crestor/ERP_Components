using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Numerics;



namespace ERP_Component_DAL.Services
{
    public class ManagerServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;
        private string _connectionString;

        public ManagerServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = config.GetConnectionString("DefaultConnectionString");
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
                cmd.CommandText = $"Select PurchaseRequisitionID, Description,RequisitionSeries, CreatedAt, TotalAmount From PurchaseRequisitions Where RequisitionStatus = 2";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        RequisitionId = reader["PurchaseRequisitionID"] != DBNull.Value ? (Guid)reader["PurchaseRequisitionID"] : Guid.Empty,
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
                cmd.CommandText = $" SELECT ve.VendorID, vq.VendorQuotationID ,ve.VendorName,vq.DiscountAmount, vq.Amount, vq.PaymentTerms, vq.DeliveryTerms,vq.AdvancedAmount,vq.FinalAmount,vq.DiscountRate  FROM VendorQuotations vq JOIN Vendors ve ON vq.VendorID = Ve.VendorID  Where vq.PurchaseRequisitionID = '{RequisitionID}' ORDER BY vq.FinalAmount";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new Vendor
                    {

                        requisitionId = RequisitionID,
                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        vendorQuotationId = reader["VendorQuotationID"] != DBNull.Value ? (Guid)reader["VendorQuotationID"] : Guid.Empty,
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
                cmd.CommandText = $" SELECT  it.ItemName, it.HSN, it.Specification, it.UnitOfMeasure, ri.UnitPrice,ri.TotalPrice,ri.Quantity, c.CategoryName, i.TypeName FROM Items it JOIN PurchaseRequisitionItems ri ON it.ItemId = ri.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where ri.PurchaseRequisitionID = '{RequisitionID}'";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new AddPurchaseRequisition
                    {

                        RequisitionId = RequisitionID,

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

        public bool UpdateVendorQuotationStatusAndRequisitionStatusToApproved(Guid vendorQuotationID, Guid requisitionID)
        {
            try
            {
                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"Update VendorQuotations set QuotationStatus = 3 Where VendorQuotationID = '{vendorQuotationID}'; " +
                                  $"Update PurchaseRequisitions set RequisitionStatus = 3 Where PurchaseRequisitionID = '{requisitionID}' ";

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
        //public DashBoard GetManagerDashboardData()
        //{
        //    try
        //    {
        //        DashBoard Item = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $" select(select sum(InStock) from Inventory)As InStock,(select count(*) from Vendors) as TotalVendor,(select Count(*) from Categories) As Categories ,(Select Count(RequisitionID) From Requisitions Where RequisitionType = 3) AS TotalPurchaseRequisition,(select sum( R.TotalAmount ) from Requisitions R join PurchaseOrders P on R.RequisitionID=P.RequisitionID) as TotalPurchases,(SELECT COUNT(*) FROM PurchaseOrders WHERE OrderStatus = 1) as PendingOrder ";
        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {


        //            Item.InStock = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0;
        //            Item.TotalVendor = reader["TotalVendor"] != DBNull.Value ? Convert.ToInt32(reader["TotalVendor"]) : 0;
        //            Item.Categories = reader["Categories"] != DBNull.Value ? Convert.ToInt32(reader["Categories"]) : 0;
        //            Item.TotalPurchaseRequisition = reader["TotalPurchaseRequisition"] != DBNull.Value ? Convert.ToInt32(reader["TotalPurchaseRequisition"]) : 0;
        //            Item.PendingOrder = reader["PendingOrder"] != DBNull.Value ? Convert.ToInt32(reader["PendingOrder"]) : 0;
        //            Item.TotalPurchases = reader["TotalPurchases"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPurchases"]) : 0m;

        //        }


        //        return Item;

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
        public List<DashBoard> ManagerSalesAndPurchaseComparison()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select R.TotalAmount,R.CreatedAt from Requisitions R join PurchaseOrders P on R.RequisitionID = P.RequisitionID ";


                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0,

                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedAt"]) : default(DateOnly),

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

        /*public List<DashBoard> SummaryOrderData()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select (SELECT COUNT(*)FROM PurchaseOrders WHERE OrderStatus = 1) as Pending ,(SELECT COUNT(*)FROM PurchaseOrders WHERE OrderStatus = 3) as Complete ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        Pending = reader["Pending"] != DBNull.Value ? Convert.ToInt32(reader["Pending"]) : 0,
                        Complete = reader["Complete"] != DBNull.Value ? Convert.ToInt32(reader["Complete"]) : 0,

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
        }*/



        public bool UpdateVendorQuotationStatusToRejected(Guid requisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update VendorQuotations set QuotationStatus = 0 Where  PurchaseRequisitionID = '{requisitionId}' ";

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



        public List<Vendor> TopVendors()
        {
            try
            {
                List<Vendor> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT TOP 5 v.VendorName, v.TaxId, v.VendorCode, MAX(po.TaxableAmount) AS MaxTaxableAmount FROM  Vendors v JOIN PurchaseOrders po ON v.VendorID = po.VendorId GROUP BY  v.VendorName, v.TaxId, v.VendorCode ORDER BY  MAX(po.TaxableAmount) DESC";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Vendor
                    {

                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        GstIn = reader["TaxId"] != DBNull.Value ? (string)reader["TaxId"] : string.Empty,
                        vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty,
                        amount = reader["MaxTaxableAmount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxTaxableAmount"]) : 0m,


                    });
                }

                return vendor;

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



        public List<Vendor> PendingPurchaseOrders()
        {
            try
            {
                List<Vendor> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" Select v.VendorName, v.VendorCode,pb.BillNumber,pb.BillDate,pb.PaidAmount,po.Description from Vendors v Join PurchaseOrders po ON v.VendorID  = Po.VendorId JOIN PurchaseBills pb ON po.PurchaseOrderID = pb.PurchaseOrderID Where pb.BillStatus = 1";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Vendor
                    {

                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,

                        vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty,
                        invoiceNumber = reader["BillNumber"] != DBNull.Value ? (string)reader["BillNumber"] : string.Empty,
                        description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        amount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,
                        createdAt = reader["BillDate"] != DBNull.Value ? ((DateTime)reader["BillDate"]).Date : default(DateTime),

                    });
                }

                return vendor;

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



        public List<Vendor> PendingApprovedQuotations()
        {
            try
            {
                List<Vendor> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select v.VendorName, v.VendorCode,vq.Amount,vq.QuotationStatus, vq.DeliveryTerms,vq.PaymentTerms,vq.DiscountAmount,vq.FinalAmount from Vendors v JOIN VendorQuotations vq On v.VendorID = vq.VendorID ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Vendor
                    {

                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty,
                        amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m,
                        finalAmount = reader["FinalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["FinalAmount"]) : 0m,
                        discountAmount = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountAmount"]) : 0m,
                        deliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,
                        paymentTerms = reader["PaymentTerms"] != DBNull.Value ? (string)reader["PaymentTerms"] : string.Empty,
                        status = reader["QuotationStatus"] != DBNull.Value ? Convert.ToInt32(reader["QuotationStatus"]) : 0,

                    });
                }

                return vendor;

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


        public List<Items> StockSummary()
        {
            try
            {
                List<Items> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, Instock, StockAlert From Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN DistributionCenter dc ON iv.CenterId = dc.CenterId  Where dc.CenterType = 4";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Items
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,


                    });
                }

                return vendor;

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

        public List<Items> SalesInventory()
        {
            try
            {
                List<Items> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, Instock, StockAlert From Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN DistributionCenter dc ON iv.CenterId = dc.CenterId  Where dc.CenterType = 1";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Items
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,


                    });
                }

                return vendor;

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


        public List<Items> ProductionStockInventory()
        {
            try
            {
                List<Items> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, Instock, StockAlert From Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN DistributionCenter dc ON iv.CenterId = dc.CenterId  Where dc.CenterType = 4";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Items
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,


                    });
                }

                return vendor;

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





        public List<Items> WarehouseStock()
        {
            try
            {
                List<Items> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, Instock, StockAlert From Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN DistributionCenter dc ON iv.CenterId = dc.CenterId  Where dc.CenterType = 2";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Items
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,


                    });
                }

                return vendor;

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




        public List<Items> TopSellingProducts()
        {
            try
            {
                List<Items> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemName, it.SKU, c.CategoryName, it.Specification, it.GstRate, ISNULL(ri.MaxQty, 0) AS MaxQty, pp.MRP, pp.CostPrice,  pp.SellingPrice,   (pp.SellingPrice - pp.CostPrice) AS Remaining FROM  Items it JOIN  Categories c ON it.CategoryId = c.CategoryId LEFT JOIN ( SELECT ItemID, MAX(Quantity) AS MaxQty FROM RequisitionItems  GROUP BY ItemID) ri ON it.ItemId = ri.ItemID OUTER APPLY (SELECT TOP 1 * FROM ProductPrice pp WHERE pp.ProductID = it.ItemId ORDER BY pp.ProductID DESC) pp WHERE it.ItemType = 1 ORDER BY CASE WHEN ri.MaxQty IS NULL THEN 1 ELSE 0 END; ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Items
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        gstRate = reader["GstRate"] != DBNull.Value ? Convert.ToDecimal(reader["GstRate"]) : 0m,
                        costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,
                        sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,
                        revenue = reader["Remaining"] != DBNull.Value ? Convert.ToDecimal(reader["Remaining"]) : 0m,
                        mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,

                    });
                }

                return vendor;

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


      


        public List<ReturnNote> SalesReturnNote()
        {
            try
            {
                List<ReturnNote> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,Status, Quantity, ReturnableQuantity, ReturnNotedate from ReturnNote";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new ReturnNote
                    {

                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        ReturnableQuantity = reader["ReturnableQuantity"] != DBNull.Value ? (int)reader["ReturnableQuantity"] : 0,

                        ReturnNotedate = reader["ReturnNotedate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ReturnNotedate"]) : default,





                    });
                }

                return vendor;

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



        public List<AddCustomer> PendingDeliveries()
        {
            try
            {
                List<AddCustomer> customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select cu.CustomerName, cu.CustomerIndustry,cu.CustomerCode,cq.QuotationDate,cq.QuotationSeries,cq.GrossTotal from Customers cu Join CustomerQuotation cq On cu.CustomerID = cq.CustomerID  Where cq.Status = 'open'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new AddCustomer
                    {

                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        CustomerIndustry = reader["CustomerIndustry"] != DBNull.Value ? (string)reader["CustomerIndustry"] : string.Empty,
                        CustomerCode = reader["CustomerCode"] != DBNull.Value ? (string)reader["CustomerCode"] : string.Empty,
                        series = reader["QuotationSeries"] != DBNull.Value ? (string)reader["QuotationSeries"] : string.Empty,
                        Amount = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        OpeningDate = reader["QuotationDate"] != DBNull.Value ? ((DateTime)reader["QuotationDate"]).Date : default(DateTime),
                    });
                }

                return customer;

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


        public List<SalesGrowth> GetSalesGrowth(string duration)
        {
            var result = new List<SalesGrowth>();
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSalesGrowthReport", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Duration", duration);

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new SalesGrowth
                                {
                                    Period = reader["Period"].ToString(),
                                    TotalSales = Convert.ToDecimal(reader["TotalSales"]),
                                    PreviousSales = reader["PreviousSales"] != DBNull.Value ? Convert.ToDecimal(reader["PreviousSales"]) : (decimal?)null,
                                    GrowthPercentage = reader["GrowthPercentage"] != DBNull.Value ? Convert.ToDecimal(reader["GrowthPercentage"]) : (decimal?)null
                                });
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public List<Order> GoodMovements()
        {
            try
            {
                List<Order> customer = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select it.ItemName,st.Quantity,st.TransactionDate,(Select CenterName from DistributionCenter Where st.SourceDC = CenterId)AS SourceName,(Select CenterName from DistributionCenter Where st.DestinationDC = CenterId)AS DestinationName, st.Reason from StockTransactions st JOIN Items it ON st.ItemId = it.ItemId Where st.TransactionType = 3";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer.Add(new Order
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        sourceName = reader["SourceName"] != DBNull.Value ? (string)reader["SourceName"] : string.Empty,
                        destinationName = reader["DestinationName"] != DBNull.Value ? (string)reader["DestinationName"] : string.Empty,
                        reason = reader["Reason"] != DBNull.Value ? (string)reader["Reason"] : string.Empty,
                        orderDate = reader["TransactionDate"] != DBNull.Value ? ((DateTime)reader["TransactionDate"]).Date : default(DateTime),

                    });
                }

                return customer;

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


        public List<DashBoard> SummaryOrderData()
        {
            var materials = new List<DashBoard>();
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("GetOrderSummary", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;



                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materials.Add(new DashBoard
                                {
                                    Pending = reader.GetInt32(0),
                                    Complete = reader.GetInt32(1),

                                    FilterType = reader.GetString(2)
                                });
                            }
                        }
                    }
                }

                return materials;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DashBoard GetManagerDashboardData()
        {
            try
            {
                DashBoard Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select(select sum(InStock) from Inventory)As InStock,(select count(*) from Vendors) as TotalVendor,(select Count(*) from Categories) As Categories ,(Select Count(RequisitionID) From Requisitions Where RequisitionType = 3) AS TotalPurchaseRequisition,(select sum( R.TotalAmount ) from Requisitions R join PurchaseOrders P on R.RequisitionID=P.RequisitionID) as TotalPurchases,(SELECT COUNT(*) FROM PurchaseOrders WHERE OrderStatus = 1) as PendingOrder ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    Item.InStock = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0;
                    Item.TotalVendor = reader["TotalVendor"] != DBNull.Value ? Convert.ToInt32(reader["TotalVendor"]) : 0;
                    Item.Categories = reader["Categories"] != DBNull.Value ? Convert.ToInt32(reader["Categories"]) : 0;
                    Item.TotalPurchaseRequisition = reader["TotalPurchaseRequisition"] != DBNull.Value ? Convert.ToInt32(reader["TotalPurchaseRequisition"]) : 0;
                    Item.PendingOrder = reader["PendingOrder"] != DBNull.Value ? Convert.ToInt32(reader["PendingOrder"]) : 0;
                    Item.TotalPurchases = reader["TotalPurchases"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPurchases"]) : 0m;

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


        public DashBoard CalculateTotalSales()
        {
            try
            {
              DashBoard dashboard  = new();
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT  ISNULL((SELECT SUM(NetTotal) FROM RetailBillHeader), 0) +  ISNULL((SELECT SUM(GrossTotal)  FROM CustomerQuotation cq  JOIN Invoice iv ON cq.QuotationID = iv.QuotationID), 0)  AS TotalSales, ISNULL((SELECT COUNT(*) FROM RetailBillHeader), 0) + ISNULL((SELECT COUNT(*)  FROM CustomerQuotation cq  JOIN Invoice iv ON cq.QuotationID = iv.QuotationID), 0) AS TotalOrders;";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                  dashboard.TotalAmount =  reader["TotalSales"] != DBNull.Value ? (decimal)reader["TotalSales"] : 0m;
                   dashboard.TotalOrder =  reader["TotalOrders"]!= DBNull.Value ? Convert.ToInt32(reader["TotalOrders"]) : 0;
                }

                return dashboard;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }




        /*public List<DashBoard> ManagerSalesAndPurchaseComparison()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select R.TotalAmount,R.CreatedAt from Requisitions R join PurchaseOrders P on R.RequisitionID = P.RequisitionID ";


                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? (decimal)reader["TotalAmount"] : 0,

                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedAt"]) : default(DateOnly),

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
        }*/




        public List<Product> ListOfInStockItems()
        {
            try
            {
                List<Product> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select S.Quantity, I.ItemName, S.TransactionDate, dc.CenterName from Items I join StockTransactions S on I.ItemId=S.ItemId Left JOIN DistributionCenter dc On S.DestinationDC = dc.CenterId where S.TransactionType=1";
                cmd.Connection = connection;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Product()
                    {
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        centerName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,

                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["TransactionDate"]) : DateOnly.MinValue,


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


        public List<Product> ListOfStockOutItems()
        {
            try
            {
                List<Product> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select S.Quantity, I.ItemName, S.TransactionDate, dc.CenterName from Items I join StockTransactions S on I.ItemId=S.ItemId Left JOIN DistributionCenter dc On S.SourceDC = dc.CenterId where S.TransactionType=2";
                cmd.Connection = connection;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Product()
                    {
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        centerName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,

                        TransactionDate = reader["TransactionDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["TransactionDate"]) : DateOnly.MinValue,


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


        //<---------------------------------------Dashboard ------------------------------------------->
        public DashBoard GetPurchaseDashBoardData()
        {
            try
            {
                DashBoard Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT (select count(*) from Vendors) as TotalVendor,(Select Count(RequisitionID) From Requisitions Where RequisitionType = 3) AS TotalPurchaseRequisition,(select sum( R.TotalAmount ) from Requisitions R join PurchaseOrders P on R.RequisitionID=P.RequisitionID) as TotalPurchases,(SELECT COUNT(*) FROM PurchaseOrders WHERE OrderStatus = 1) as PendingOrder";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Item.TotalVendor = reader["TotalVendor"] != DBNull.Value ? Convert.ToInt32(reader["TotalVendor"]) : 0;
                    Item.TotalPurchaseRequisition = reader["TotalPurchaseRequisition"] != DBNull.Value ? Convert.ToInt32(reader["TotalPurchaseRequisition"]) : 0;
                    Item.PendingOrder = reader["PendingOrder"] != DBNull.Value ? Convert.ToInt32(reader["PendingOrder"]) : 0;
                    Item.TotalPurchases = reader["TotalPurchases"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPurchases"]) : 0m;

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


        public DashBoard GetInventryDashBoardData()
        {
            try
            {
                DashBoard Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT (Select Count(*) From Requisitions Where RequisitionType = 2) AS TotalMaterialRequisition ,(Select Count(RequisitionID) From Requisitions Where RequisitionType = 3) AS TotalPurchaseRequisition,(SELECT count(PB.PurchaseOrderID) FROM PurchaseBills PB JOIN PurchaseOrders P ON PB.PurchaseOrderID = P.PurchaseOrderID JOIN Vendors V ON P.VendorID = V.VendorID) AS VenderInvoice,(SELECT sum (i.InStock * l.SellingPrice) FROM  Inventory i JOIN Lotbatch l ON i.ItemId = l.ItemId)  AS TotalStockValue";
                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item.TotalMaterialRequisition = reader["TotalMaterialRequisition"] != DBNull.Value ? Convert.ToInt32(reader["TotalMaterialRequisition"]) : 0;
                    Item.TotalPurchaseRequisition = reader["TotalPurchaseRequisition"] != DBNull.Value ? Convert.ToInt32(reader["TotalPurchaseRequisition"]) : 0;
                    Item.VenderInvoice = reader["VenderInvoice"] != DBNull.Value ? Convert.ToInt32(reader["VenderInvoice"]) : 0;
                    Item.TotalStockValue = reader["TotalStockValue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalStockValue"]) : 0m;
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
        public List<DashBoard> InventryDashBoardStockINStockOUT()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"	select InStock ,LastUpdated from Inventory ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        InStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        LastUpdated = reader["LastUpdated"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["LastUpdated"]) : default(DateOnly),
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
        public List<DashBoard> InventryDashBoardPieChartData()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT C.CategoryName,SUM(IV.InStock) AS TotalStock FROM Categories C JOIN Items IT ON C.CategoryID = IT.CategoryID JOIN Inventory IV ON IT.ItemID = IV.ItemID GROUP BY  C.CategoryName;";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        TotalStock = reader["TotalStock"] != DBNull.Value ? (int)reader["TotalStock"] : 0,
                        CategoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
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




        public DashBoard GetWarehouseDashBoardData()
        {
            try
            {
                DashBoard Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"SELECT (SELECT SUM(InStock) FROM Inventory) AS TotalStorageItems,
                            (
                               SELECT COUNT(*) 
                                 FROM (SELECT r.RequisitionID 
                           FROM Requisitions r 
                         JOIN ProductionOrder po ON po.SalesForcastID = r.RequisitionID 
                          WHERE r.RequisitionType = 1 AND r.RequisitionStatus = 4 
                         GROUP BY r.RequisitionID 
                       HAVING COUNT(*) = SUM(CASE WHEN po.ProductionStatus = 4 THEN 1 ELSE 0 END)
                          ) AS Sub
                       ) AS PendingDispatch";
                cmd.Connection = connection;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    Item.TotalStorageItems = reader["TotalStorageItems"] != DBNull.Value ? Convert.ToInt32(reader["TotalStorageItems"]) : 0;
                    Item.PendingDispatch = reader["PendingDispatch"] != DBNull.Value ? Convert.ToInt32(reader["PendingDispatch"]) : 0;

                    //Item.TotalStockValue = reader["TotalStockValue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalStockValue"]) : 0m;

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
        public List<DashBoard> WareHouseDashBoardPieChartData()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT C.CategoryName,SUM(IV.InStock) AS TotalStock FROM Categories C JOIN Items IT ON C.CategoryID = IT.CategoryID JOIN Inventory IV ON IT.ItemID = IV.ItemID GROUP BY  C.CategoryName;";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        TotalStock = reader["TotalStock"] != DBNull.Value ? (int)reader["TotalStock"] : 0,
                        CategoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


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


        public List<Warehouse> GetStoresNames()
        {
            try
            {
                List<Warehouse> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT CenterID, CenterName From DistributionCenter Where CenterType = 5";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Warehouse
                    {
                        centerId = reader["CenterID"] != DBNull.Value ? (Guid)reader["CenterID"] : Guid.Empty,
                        centerName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,


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


        public List<Items> GetStoreInventoryData(Guid centerId)
        {
            try
            {
                List<Items> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"\r\nSelect ItemName, Instock, StockAlert From Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN DistributionCenter dc ON iv.CenterId = dc.CenterId  Where dc.CenterID = '{centerId}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Items
                    {
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,


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


       






        public List<DashBoard> WareHouseDashBoardItemsINItemsOut()
        {
            try
            {
                List<DashBoard> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select InStock ,LastUpdated from Inventory ";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new DashBoard
                    {
                        InStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,

                        LastUpdated = reader["LastUpdated"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["LastUpdated"]) : default(DateOnly),

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





        public List<MonthlyRetailSales> GetMonthlySalesReport()
        {
            var salesList = new List<MonthlyRetailSales>();

            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                using (SqlCommand cmd = new SqlCommand("GetMonthlySalesReport", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            salesList.Add(new MonthlyRetailSales
                            {
                                StoreName = reader["StoreName"].ToString(),
                                Jan = reader["Jan"] != DBNull.Value ? Convert.ToDecimal(reader["Jan"]) : 0,
                                Feb = reader["Feb"] != DBNull.Value ? Convert.ToDecimal(reader["Feb"]) : 0,
                                Mar = reader["Mar"] != DBNull.Value ? Convert.ToDecimal(reader["Mar"]) : 0,
                                Apr = reader["Apr"] != DBNull.Value ? Convert.ToDecimal(reader["Apr"]) : 0,
                                May = reader["May"] != DBNull.Value ? Convert.ToDecimal(reader["May"]) : 0,
                                Jun = reader["Jun"] != DBNull.Value ? Convert.ToDecimal(reader["Jun"]) : 0,
                                Jul = reader["Jul"] != DBNull.Value ? Convert.ToDecimal(reader["Jul"]) : 0,
                                Aug = reader["Aug"] != DBNull.Value ? Convert.ToDecimal(reader["Aug"]) : 0,
                                Sep = reader["Sep"] != DBNull.Value ? Convert.ToDecimal(reader["Sep"]) : 0,
                                Oct = reader["Oct"] != DBNull.Value ? Convert.ToDecimal(reader["Oct"]) : 0,
                                Nov = reader["Nov"] != DBNull.Value ? Convert.ToDecimal(reader["Nov"]) : 0,
                                Dec = reader["Dec"] != DBNull.Value ? Convert.ToDecimal(reader["Dec"]) : 0,
                                centerType = reader["CenterType"] != DBNull.Value ? Convert.ToInt32(reader["CenterType"]) : 0,
                            });

                        }
                    }
                }

                return salesList;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<MonthlyRetailSales> GetDailyData()
        {
            var materials = new List<MonthlyRetailSales>();
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("GetDailySalesReport", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;



                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materials.Add(new MonthlyRetailSales
                                {

                                    StoreName = reader["StoreName"].ToString(),
                                    CreatedAt = reader["SalesDate"] != DBNull.Value ? ((DateTime)reader["SalesDate"]).Date : default(DateTime),
                                    TotalStore = Convert.ToDecimal(reader["Total_Amount"])

                                });
                            }
                        }
                    }
                }

                return materials;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<MonthlyRetailSales> GetRetailCustomerData()
        {
            try
            {
                List<MonthlyRetailSales> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT rc.CustomerName, rc.ContactNumber, rb.GrossTotal, CAST(rb.CreatedAt AS DATE) AS PurchaseDate FROM RetailCustomers rc JOIN RetailBillHeader rb ON rc.RetailCustomerID = rb.RetailCustomerID";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new MonthlyRetailSales
                    {
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ContactNumber = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        CreatedAt = reader["PurchaseDate"] != DBNull.Value ? ((DateTime)reader["PurchaseDate"]).Date : default(DateTime),


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
        public List<MonthlyRetailSales> GetCustomerRetailData()
        {
            try
            {
                List<MonthlyRetailSales> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT  rc.RetailCustomerID,rc.CustomerName, rc.ContactNumber, MAX(rh.CreatedAt) AS LastBillDate, SUM(rh.GrossTotal) AS GrossTotal,COUNT(rh.RetailBillID) AS BillCount FROM RetailCustomers rc JOIN RetailBillHeader rh  ON rc.RetailCustomerID = rh.RetailCustomerID GROUP BY  rc.RetailCustomerID, rc.CustomerName, rc.ContactNumber ORDER BY  LastBillDate DESC";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new MonthlyRetailSales
                    {
                        CustomerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty,
                        ContactNumber = reader["ContactNumber"] != DBNull.Value ? (string)reader["ContactNumber"] : string.Empty,
                        GrossTotal = reader["GrossTotal"] != DBNull.Value ? Convert.ToDecimal(reader["GrossTotal"]) : 0m,
                        BillCount = reader["BillCount"] != DBNull.Value ? (int)reader["BillCount"] : 0,
                        RetailId = reader["RetailCustomerID"] != DBNull.Value ? (Guid)reader["RetailCustomerID"] : Guid.Empty,
                        CreatedAt = reader["LastBillDate"] != DBNull.Value ? ((DateTime)reader["LastBillDate"]).Date : default(DateTime),
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

  
        public MonthlyRetailSales GetCustomerName(Guid customerId)
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" Select CustomerName From RetailCustomers Where RetailCustomerID = '{customerId}'";

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
       






        public SalesSummary GetSalesSummary(string filterType, int? halfMonthNumber)
        {
            var result = new SalesSummary();

            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSalesSummary", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FilterType", filterType);
                    cmd.Parameters.AddWithValue("@SourceType", "All");

                    // Replaced @WeekNumber with @HalfMonthNumber
                    if (halfMonthNumber.HasValue)
                        cmd.Parameters.AddWithValue("@HalfMonthNumber", halfMonthNumber.Value);
                    else
                        cmd.Parameters.AddWithValue("@HalfMonthNumber", DBNull.Value);

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();
                        var periods = new List<string>();

                        foreach (DataRow row in schema.Rows)
                        {
                            var colName = row["ColumnName"].ToString();
                            if (!string.Equals(colName, "Store", StringComparison.OrdinalIgnoreCase))
                                periods.Add(colName);
                        }

                        result.Periods = periods;

                        while (reader.Read())
                        {
                            var storeRow = new DynamicRetailSales
                            {
                                StoreName = reader["Store"].ToString()
                            };

                            foreach (var period in periods)
                            {
                                decimal value = 0;
                                if (reader[period] != DBNull.Value)
                                {
                                    if (decimal.TryParse(reader[period].ToString(), out var parsedValue))
                                    {
                                        value = parsedValue;
                                    }
                                }
                                storeRow.SalesByPeriod[period] = value;
                            }

                            result.Data.Add(storeRow);
                        }
                    }
                }
            }

            return result;
        }

        public List<VendorQuotationItem> GetVendorQuotationItems(Guid vendorQuotationID)
        {
            List<VendorQuotationItem> vendorQuotationItems = new List<VendorQuotationItem>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT vqi.ItemID, it.ItemName, vqi.Quantity, it.UnitOFMeasure, vqi.UnitPrice, vqi.TotalPrice FROM VendorQuotationItems vqi " +
                                   "JOIN Items it ON vqi.ItemID = it.ItemId " +
                                   "WHERE VendorQuotationID = @VendorQuotationID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@VendorQuotationID", vendorQuotationID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VendorQuotationItem item = new VendorQuotationItem
                                {
                                    ItemID = reader.GetGuid(reader.GetOrdinal("ItemID")),
                                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                    UOM = reader.GetString(reader.GetOrdinal("UnitOFMeasure")),
                                    Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))
                                };
                                vendorQuotationItems.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return vendorQuotationItems;
        }

        public List<VendorQuotationItem> GetRequisitionItems(Guid RequisitionID)
        {
            try
            {
                List<VendorQuotationItem> sales = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ri.ItemID, it.ItemName, ri.Quantity, it.UnitOFMeasure FROM RequisitionItems ri JOIN Items it ON ri.ItemID = it.ItemId Where ri.RequisitionID = '{RequisitionID}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new VendorQuotationItem
                    {
                        ItemID = reader["ItemID"] != DBNull.Value ? (Guid)reader["ItemID"] : Guid.Empty,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        UOM = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        
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

        public void SaveSeries(Series series)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"INSERT INTO SeriesCounter (SeriesPrefix, LastNumber, SeriesName) " +
                    $"VALUES (@Prefix, @StartFrom, @Name)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("Prefix", series.Prifix);
                        cmd.Parameters.AddWithValue("@StartFrom", series.StartFrom);
                        cmd.Parameters.AddWithValue("@Name", series.Series_Type);
                        connection.Open();
                        cmd.ExecuteScalar();
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Series> FindAllSeries()
        {
            var seriesList = new List<Series>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"SELECT SeriesName, SeriesPrefix + '-' + CAST(CurrentYear AS VARCHAR) + '-' + CAST(LastNumber AS VARCHAR) AS CurrentSeries FROM SeriesCounter";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                seriesList.Add(
                                    new Series
                                    {
                                        Series_Type = reader.GetString(reader.GetOrdinal("SeriesName")),
                                        CurrentSeries = reader.GetString(reader.GetOrdinal("CurrentSeries"))
                                    }
                                );

                            }
                        }
                        
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
            return seriesList;
        }

        public List<SalesForecast> ViewSalesForecast()
        {
            try
            {
                List<SalesForecast> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select RequisitionID,Description,CreatedAt,RequisitionStatus from Requisitions where RequisitionType=1 AND RequisitionStatus != @Status";
                cmd.Parameters.AddWithValue("@Status", RequisitionStatus.PENDING);
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new SalesForecast
                    {
                        RequisitionID = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                        Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["CreatedAt"]) : default(DateOnly),
                        RequisitionStatus = reader["RequisitionStatus"] != DBNull.Value ? (byte)Convert.ToInt32(reader["RequisitionStatus"]) : (byte)0
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

      public List<SalesForecast> individualSalesForecastDetails(Guid RequisitionID)
        {
            try
            {
                List<SalesForecast> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT i.ItemName,i.Specification, wo.Quantity, wo.WorkOrderStatus FROM WorkOrder wo JOIN RequisitionItems ri ON ri.RequisitionID=wo.SalesForecastID AND wo.ProductID=ri.ItemID JOIN Items i ON wo.ProductID=i.ItemId WHERE wo.SalesForecastID = '{RequisitionID}'";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new SalesForecast 
                    {
                        Specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        WorkOrderStatus = reader["WorkOrderStatus"] != DBNull.Value ? Convert.ToByte(reader["WorkOrderStatus"]) : (byte)0,
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

        public List<AddPurchaseRequisition> ViewSalesForCasting(RequisitionStatus status)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT r.RequisitionID, r.RequisitionSeries, r.[Description], r.CreatedAt, rt.TypeName, dc.CenterName FROM Requisitions r" +
                    $" JOIN RequisitionTypes rt ON r.RequisitionType = rt.RequisitionType LEFT JOIN RequisitionsDistributionCenterBridge rb ON r.RequisitionID = rb.RequisitionID" +
                    $" LEFT JOIN  DistributionCenter dc ON rb.CenterId = dc.CenterId WHERE r.RequisitionType IN (1,4) AND r.RequisitionStatus = @Status Order By CreatedAt desc";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Status", status);


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
                        Date = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),
                        RequisitionType = reader.GetString(reader.GetOrdinal("TypeName")),
                        CenterName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,
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


        public void SaveCompanyDetails(BusinessSetUp bussinessSetup)
        {
            SqlTransaction transaction = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                try 
                {     
                    connection.Open();
                    using (transaction = connection.BeginTransaction())
                    {

                        string addressQuery = @"
                        INSERT INTO Address(Country, State, District, City, Pincode, AddressLine1)
                        VALUES(@Country, @State, @District, @City, @Pincode, @AddressLine1);
                        SELECT SCOPE_IDENTITY();";

                        int addressId;

                        using (SqlCommand cmd = new SqlCommand(addressQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Country", bussinessSetup.Country ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@State", bussinessSetup.State ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@District", bussinessSetup.District ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@City", bussinessSetup.City ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Pincode", bussinessSetup.PinCode ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@AddressLine1", bussinessSetup.Address ?? (object)DBNull.Value);

                            addressId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string companyQuery = @"
                        INSERT INTO Company (CompanyName, AddressID, Phone, AlternatePhone, Email, GSTIN, PAN, CIN, TAN)
                        VALUES (@CompanyName, @AddressID, @Phone, @AlternatePhone, @Email, @GSTIN, @PAN, @CIN, @TAN)";

                        using (SqlCommand cmd = new SqlCommand(companyQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CompanyName", bussinessSetup.BussinessName ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@AddressID", addressId); // Use the int AddressID here
                            cmd.Parameters.AddWithValue("@Phone", bussinessSetup.Mobile ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@AlternatePhone", bussinessSetup.AlternateMobile ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Email", bussinessSetup.Email ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@GSTIN", bussinessSetup.GstIn ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PAN", bussinessSetup.PAN ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@CIN", bussinessSetup.CIN ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@TAN", bussinessSetup.TAN ?? (object)DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    throw;
                }
            }
            
        }

        public BusinessSetUp FindCompanyDetails()
        {
            BusinessSetUp businessSetUp;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"SELECT c.CompanyID, c.CompanyName, c.Phone, c.AlternatePhone, c.Email, c.GST, c.CIN, c.PAN, c.TAN 
                                     a.Country, a.State, a.District, a.City, a.Pincode, a.AddressLine1 Company c JOIN Address a ON c.AdressID = c.AddressID";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                businessSetUp = new BusinessSetUp
                                {
                                    CompanyID = reader.GetGuid("CompanyID"),
                                    BussinessName = reader.GetString("CompanyName"),
                                    Email = reader.GetString("Email"),
                                    Mobile = reader.GetString("Phone"),
                                    AlternateMobile = reader.GetString("AlternatePhone"),
                                    GstIn = reader.GetString("GST"),
                                    CIN = reader.GetString("CIN"),
                                    PAN = reader.GetString("PAN"),
                                    TAN = reader.GetString("TAN"),
                                    address = new Address
                                    {
                                        Country = reader.GetString("Country"),
                                        State = reader.GetString("State"),
                                        District = reader.GetString("District"),
                                        City = reader.GetString("City"),
                                        Pincode = reader.GetString("Pincode"),
                                        AddressLine1 = reader.GetString("AddressLine1")
                                    }

                                };
                            else
                                throw new Exception("No Business Details Available");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return businessSetUp;
        }



        //public List<VendorQuotationItem> GetRequisitionItems(Guid RequisitionID)
        //{
        //    List<VendorQuotationItem> vendorQuotationItems = new List<VendorQuotationItem>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            string query = "SELECT ri.ItemID, it.ItemName, ri.Quantity, it.UnitOFMeasure FROM RequisitionItems ri JOIN Items it ON ri.ItemID = it.ItemId Where ri.RequisitionID = @RequisitionID";

        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        VendorQuotationItem item = new VendorQuotationItem
        //                        {
        //                            ItemID = reader.GetGuid(reader.GetOrdinal("ItemID")),
        //                            ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
        //                            UOM = reader.GetString(reader.GetOrdinal("UnitOFMeasure")),
        //                            Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")),
        //                            //UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
        //                            //TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"))
        //                        };
        //                        vendorQuotationItems.Add(item);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return vendorQuotationItems;
        //}
    }

}


