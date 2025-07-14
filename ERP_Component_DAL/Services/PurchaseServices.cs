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
    public class PurchaseServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;
        string _connectionString;

        public PurchaseServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = config.GetConnectionString("DefaultConnectionString");
        }




        public bool AddPurchaseRequisition(AddPurchaseRequisition AdPR)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into PurchaseRequisitionItems (ItemID,Quantity,UnitPrice,PurchaseRequisitionID ) values (@ItemId,@Quantity,@UnitPrice,@PurchaseRequisitionID)";
                cmd.Parameters.AddWithValue("@PurchaseRequisitionID", AdPR.RequisitionId);
                cmd.Parameters.AddWithValue("@ItemId", AdPR.itemId);
                cmd.Parameters.AddWithValue("@Quantity", AdPR.quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", AdPR.unitPrice);

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
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




        public List<AddPurchaseRequisition> GetItemNames()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select ItemId, ItemName From Items Where ItemType = 2";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

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


        public AddPurchaseRequisition GetItemsData(Guid itemId)
        {
            try
            {
                AddPurchaseRequisition Item = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT it.ItemId, it.ItemName, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, l.Id, l.CostPrice, c.CategoryName, i.TypeName FROM Items it JOIN LotBatch l ON it.ItemId = l.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where it.ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    Item.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                    Item.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    Item.itemtype = reader["TypeName"] != DBNull.Value ? (string)reader["TypeName"] : string.Empty;
                    Item.hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty;


                    Item.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
                    Item.unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty;
                    Item.category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;

                    Item.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;
                    Item.unitPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m;

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


        public Guid AddRequisition(AddPurchaseRequisition Add)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into PurchaseRequisitions([Description],[TotalAmount],[RequisitionSeries]) " + "OUTPUT INSERTED.PurchaseRequisitionID" + " values (@description,@totalAmount,@RequisitionSeries)";

                cmd.Parameters.AddWithValue("@description", Add.Descripion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RequisitionSeries", Add.requisitionSeries ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@totalAmount", Add.TotalAmount);
              


                cmd.Connection = connection;
                connection.Open();
                Guid PurchaseRequisitionID = (Guid)cmd.ExecuteScalar();


                return PurchaseRequisitionID;

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


        public bool AddPurchaseOrder(Vendor vendor, Guid purchaseOrderID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = $"INSERT INTO PurchaseOrders(PurchaseOrderID,VendorID,PurchaseRequisitionId,Description,TaxableAmount, OrderStatus, AdvancePercent) " +
                    $"VALUES (@PurchaseOrderID, @VendorID, @RequisitionID,'{vendor.description}','{vendor.amount}', 1, @AdvancedRate)";


                cmd.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderID);
                cmd.Parameters.AddWithValue("@VendorID", vendor.vendorId);
                cmd.Parameters.AddWithValue("@RequisitionID", vendor.requisitionId);
                cmd.Parameters.AddWithValue("@AdvancedRate", vendor.advanceRate);

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


        //public AddPurchaseRequisition GetItemsData(Guid itemId)
        //{
        //    try
        //    {
        //        AddPurchaseRequisition Item = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $" SELECT it.ItemId, it.ItemName, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, l.Id, l.CostPrice, c.CategoryName, i.TypeName FROM Items it JOIN LotBatch l ON it.ItemId = l.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where it.ItemId = '{itemId}'";

        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {


        //            Item.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
        //            Item.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
        //            Item.itemtype = reader["TypeName"] != DBNull.Value ? (string)reader["TypeName"] : string.Empty;

        //            Item.hsn = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0;

        //            Item.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
        //            Item.unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty;
        //            Item.category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;

        //            Item.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;
        //            Item.unitPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m;

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



        public void AddPurchaseOrderItems(List<AddPurchaseRequisition> Items, Guid purchaseOrderID)
        {

            try
            {
                //Guid purchasOrderID = Guid.NewGuid();
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionString);
                connection.Open();

                foreach (var item in Items)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "INSERT INTO PurchaseItems (PurchaseOrderId, ItemId, Quantity, UnitPrice) " +
                                      "VALUES (@PurchaseOrderId, @ItemId, @Quantity, @UnitPrice)";

                    cmd.Parameters.AddWithValue("@PurchaseOrderId", purchaseOrderID);
                    cmd.Parameters.AddWithValue("@ItemId", item.itemId);
                    cmd.Parameters.AddWithValue("@Quantity", item.quantity);
                    cmd.Parameters.AddWithValue("@UnitPrice", item.unitPrice);
                    cmd.Parameters.AddWithValue("@TaxableAmount", item.TotalAmount);

                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                }




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





        //<---------------------RequistionLIst----------------->
        public List<AddPurchaseRequisition> RequisitionListItems(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT  it.ItemName, it.HSN, it.Specification, it.UnitOfMeasure,ri.PurchaseRequisitionID, ri.UnitPrice,ri.TotalPrice,ri.Quantity, c.CategoryName, i.TypeName FROM Items it JOIN PurchaseRequisitionItems ri ON it.ItemId = ri.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where ri.PurchaseRequisitionID = '{RequisitionID}'";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {

                        RequisitionId = reader["PurchaseRequisitionID"] != DBNull.Value ? (Guid)reader["PurchaseRequisitionID"] : Guid.Empty,

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




        //public List<Vendor> VendorQuotationLists(Guid RequisitionID)
        //{
        //    try
        //    {
        //        List<AddPurchaseRequisition> prod = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $" SELECT   RequisitionID = '{RequisitionID}'";
        //        cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            prod.Add(new AddPurchaseRequisition
        //            {

        //                RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,

        //                itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
        //                itemtype = reader["TypeName"] != DBNull.Value ? (string)reader["TypeName"] : string.Empty,

        //                hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
        //                quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
        //                specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
        //                unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
        //                category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


        //                unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
        //                TotalAmount = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m,

        //            });
        //        }

        //        return prod;

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

        public bool AddPurchaseBill(Vendor vendor)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into PurchaseBills([BillNumber],[PurchaseOrderID],[BillDate])values('{vendor.invoiceNumber}','{vendor.purchaseOrderId}','{vendor.createdAt}');" +
                    $" UPDATE PurchaseOrders SET OrderStatus = 4 WHERE PurchaseOrderID = '{vendor.purchaseOrderId}'";

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
        public bool UpdateRequisition(AddPurchaseRequisition Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update PurchaseRequisitions Set RequisitionSeries = '{Aq.requisitionSeries}', Description='{Aq.Descripion}' , TotalAmount='{Aq.TotalAmount}', RequisitionStatus = 1 where PurchaseRequisitionID = '{Aq.RequisitionId}' ";



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

        public List<AddPurchaseRequisition> ViewRequisitions()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select PurchaseRequisitionID,RequisitionSeries, Description, CreatedAt, TotalAmount From PurchaseRequisitions Where RequisitionStatus = 1";
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
        public List<AddPurchaseRequisition> ViewRequisitionsForQuotation()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select RequisitionID, Description,RequisitionSeries, CreatedAt, TotalAmount From Requisitions Where RequisitionStatus = 3 and requisitiontype = 3";
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
                cmd.CommandText = $" SELECT ve.VendorName,vq.DiscountAmount, vq.Amount, vq.PaymentTerms, vq.DeliveryTerms,vq.AdvancedAmount,vq.FinalAmount,vq.DiscountRate  FROM VendorQuotations vq JOIN Vendors ve ON vq.VendorID = Ve.VendorID  Where vq.PurchaseRequisitionID = '{RequisitionID}' ORDER BY vq.FinalAmount";
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


        public bool UpdateRequisitionStatusToPending(Guid requisitionId)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update PurchaseRequisitions Set  RequisitionStatus = 2 where PurchaseRequisitionID = '{requisitionId}'";



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

        public bool UpdateRequisitionStatusToClosed(Vendor vendor)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update PurchaseRequisitions Set  RequisitionStatus = 4 where PurchaseRequisitionID = '{vendor.requisitionId}'";



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
        public bool UpdateRequisitionStatusToQuotationAdded(Vendor vendor)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Requisitions Set  RequisitionStatus = 4 where RequisitionID = '{vendor.requisitionId}'";



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

        public bool UpdateRequisitionStatusToQuotationApproved(Guid RequisitionID)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Requisitions Set  RequisitionStatus = 5 where RequisitionID = '{RequisitionID}'";



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

        public bool DeleteRequisition(Guid requisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  Requisitions  where RequisitionID = '{requisitionId}'";

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








        public List<AddPurchaseRequisition> ApprovedRequisitions()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select PurchaseRequisitionID, RequisitionSeries, Description, CreatedAt, TotalAmount From PurchaseRequisitions Where RequisitionStatus = 3 Order by CreatedAt desc";
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

        public List<AddPurchaseRequisition> ApprovedRequisitionsForQuotation()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select PurchaseRequisitionID, Description,RequisitionSeries, CreatedAt, TotalAmount From PurchaseRequisitions Where RequisitionStatus = 1";
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

        public Vendor GetVendorDataForOrder(Guid requisitionId)
        {
            try
            {
                Vendor vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ve.VendorID, ve.VendorName, vq.VendorQuotationID,vq.Amount,vq.PaymentTerms,vq.DeliveryTerms, vq.AdvancedRate  from Vendors ve Join VendorQuotations vq On ve.VendorID = vq.VendorID WHERE vq.PurchaseRequisitionID = '{requisitionId}'";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty;
                    vendor.vendorQuotationId = reader["VendorQuotationID"] != DBNull.Value ? (Guid)reader["VendorQuotationID"] : Guid.Empty;
                    vendor.requisitionId = requisitionId;
                    vendor.amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m;
                    vendor.vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty;
                    vendor.deliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty;
                    vendor.advanceRate = reader.GetDecimal("AdvancedRate");
                    vendor.paymentTerms = reader["PaymentTerms"] != DBNull.Value ? (string)reader["PaymentTerms"] : string.Empty;


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


        public List<AddPurchaseRequisition> GetPurchasedOrdersItemsLists(Guid requisitionId)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName,it.specification, vqi.UnitPrice, vqi.Quantity, vqi.TotalPrice FROM VendorQuotationItems vqi " +
                                  $"JOIN Items it ON it.ItemId = vqi.ItemID JOIN VendorQuotations vq ON vq.VendorQuotationID = vqi.VendorQuotationID " +
                                  $"WHERE vq.PurchaseRequisitionID = '{requisitionId}' AND vq.QuotationStatus = 3";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0,
                        RequisitionId = requisitionId,
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        TotalAmount = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m,
                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
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
        public List<Vendor> GetVendorNames()
        {
            try
            {
                List<Vendor> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select VendorID, VendorName From Vendors";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Vendor
                    {

                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,



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
        public bool AddTaxDetails(Guid PurchaseOrderId, Vendor vendor)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    string spName = "PurchaseOrderProcesses";

                    using (SqlCommand cmd = new SqlCommand(spName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@PurchaseOrderID", PurchaseOrderId);
                        cmd.Parameters.AddWithValue("@RequisitionID", vendor.requisitionId);
                        cmd.Parameters.AddWithValue("@VendorID", vendor.vendorId);




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

        public List<AddPurchaseRequisition> PurchaseOrderItemsList(Guid vendorId, Guid purchaseOrderId, PurchaseOrderStatus orderStatus)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT  it.ItemName,it.UnitOfMeasure, it.HSN, pu.Quantity, pu.UnitPrice, pu.DiscountRate, pu.CGST,pu.SGST, pu.IGST, pu.TotalPrice, pu.TaxableAmount," +
                                  $" (pu.TaxableAmount * pu.IGST / 100.0) AS IGSTAmount, (pu.TaxableAmount * pu.CGST / 100.0) AS CGSTAmount, (pu.TaxableAmount * pu.SGST / 100.0) AS SGSTAmount" +
                                  $" FROM Items it JOIN  PurchaseItems pu ON it.ItemId = pu.ItemID Left Join PurchaseOrders po On pu.PurchaseOrderID = po.PurchaseOrderID Where po.VendorId = '{vendorId}'" +
                                  $" AND po.PurchaseOrderID = '{purchaseOrderId}' AND po.OrderStatus = {(byte)orderStatus} ";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        //RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        TotalAmount = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m,
                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,
                        unitofmeasure = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,
                        taxableValue = reader["TaxableAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TaxableAmount"]) : 0m,
                        cgstRate = reader["CGST"] != DBNull.Value ? Convert.ToDecimal(reader["CGST"]) : 0m,
                        cgstAmount = reader["CGSTAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CGSTAmount"]) : 0m,
                        sgstRate = reader["SGST"] != DBNull.Value ? Convert.ToDecimal(reader["SGST"]) : 0m,
                        sgstAmount = reader["SGSTAmount"] != DBNull.Value ? Convert.ToDecimal(reader["SGSTAmount"]) : 0m,
                        igstRate = reader["IGST"] != DBNull.Value ? Convert.ToDecimal(reader["IGST"]) : 0m,
                        igstAmount = reader["IGSTAmount"] != DBNull.Value ? Convert.ToDecimal(reader["IGSTAmount"]) : 0m,
                        NetAmount = reader["TaxableAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TaxableAmount"]) : 0m,
                        discount = reader["DiscountRate"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountRate"]) : 0m,
                        quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,

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




        public Vendor GetVendorAddressDetails(Guid VendorId)
        {
            try
            {
                Vendor vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select vn.VendorName, vn.VendorCode,ad.AddressLine1,ad.District,ad.State,ad.Country,ad.Pincode,vn.TaxID  from Vendors vn Join Address ad On vn.AddressID = ad.AddressID Where vn.VendorID = '{VendorId}'";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    vendor.vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty;
                    vendor.vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty;
                    vendor.Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    vendor.District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty;
                    vendor.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    vendor.Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty;
                    vendor.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;
                    vendor.GstIn = reader["TaxID"] != DBNull.Value ? (string)reader["TaxID"] : string.Empty;



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

        public Vendor GetPurchaseBills(Guid purchaseOrderId)
        {
            try
            {
                Vendor vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select vn.VendorName, vn.VendorCode,ad.AddressLine1,ad.District,ad.State,ad.Country,ad.Pincode,vn.TaxID,pb.BillNumber,pb.BillDate from Vendors vn Join Address ad On vn.AddressID = ad.AddressID Left Join PurchaseOrders po On vn.vendorId = po.VendorId Left Join PurchaseBills pb On po.PurchaseOrderID = pb.PurchaseOrderID Where pb.PurchaseOrderID = '{purchaseOrderId}'";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    vendor.vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty;
                    vendor.vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty;
                    vendor.Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    vendor.District = reader["District"] != DBNull.Value ? (string)reader["District"] : string.Empty;
                    vendor.invoiceNumber = reader["BillNumber"] != DBNull.Value ? (string)reader["BillNumber"] : string.Empty;
                    vendor.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    vendor.Country = reader["Country"] != DBNull.Value ? (string)reader["Country"] : string.Empty;
                    vendor.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;
                    vendor.GstIn = reader["TaxID"] != DBNull.Value ? (string)reader["TaxID"] : string.Empty;
                    vendor.createdAt = reader["BillDate"] != DBNull.Value ? ((DateTime)reader["BillDate"]).Date : default(DateTime);



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

        public List<Vendor> GeneratePurchaseOrder()
        {
            try
            {
                List<Vendor> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = @"Select ve.VendorID, ve.VendorName, ve.VendorCode,po.PurchaseOrderID, po.CreatedAt,po.Description,po.TaxableAmount from Vendors ve 
                    join PurchaseOrders po ON ve.VendorID = po.VendorId  Where po.OrderStatus = 2 Order By po.CreatedAt desc";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Vendor
                    {
                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        purchaseOrderId = reader["PurchaseOrderID"] != DBNull.Value ? (Guid)reader["PurchaseOrderID"] : Guid.Empty,
                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty,
                        description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        createdAt = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),

                        amount = reader["TaxableAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TaxableAmount"]) : 0m,




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


        public List<Vendor> GeneratePurchaseBill()
        {
            try
            {
                List<Vendor> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ve.VendorID, ve.VendorName, ve.VendorCode,po.PurchaseOrderID,pb.BillNumber,pb.BillDate, pb.PaidAmount from PurchaseBills pb join PurchaseOrders po ON po.PurchaseOrderID= pb.PurchaseOrderID  Left Join Vendors ve On po.VendorId = ve.VendorID where orderstatus = 4 Order By pb.BillDate desc";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Vendor
                    {
                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        purchaseOrderId = reader["PurchaseOrderID"] != DBNull.Value ? (Guid)reader["PurchaseOrderID"] : Guid.Empty,
                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        vendorCode = reader["VendorCode"] != DBNull.Value ? (string)reader["VendorCode"] : string.Empty,
                        invoiceNumber = reader["BillNumber"] != DBNull.Value ? (string)reader["BillNumber"] : string.Empty,
                        createdAt = reader["BillDate"] != DBNull.Value ? ((DateTime)reader["BillDate"]).Date : default(DateTime),
                        amount = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m,




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



        public List<AddPurchaseRequisition> GetRequisitionItemsListData(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
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
                    prod.Add(new AddPurchaseRequisition
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

        //public void AddVendorQuotation(Vendor vendor)
        //{

        //    try
        //    {

        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionString);
        //        connection.Open();

        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandType = CommandType.Text;

        //            cmd.CommandText = "INSERT INTO VendorQuotations(VendorID, Amount, PaymentTerms, DeliveryTerms, nRequisitionID) " +
        //                              "VALUES (@VendorId, @Amount, @PaymentTerms, @DeliveryTerms, @RequisitionId)";

        //            cmd.Parameters.AddWithValue("@VendorId",vendor.vendorId);
        //            cmd.Parameters.AddWithValue("@RequisitionId", vendor.requisitionId);
        //            cmd.Parameters.AddWithValue("@PaymentTerms", vendor.paymentTerms);
        //            cmd.Parameters.AddWithValue("@DeliveryTerms", vendor.deliveryTerms);
        //            cmd.Parameters.AddWithValue("@Amount",vendor.amount);

        //            cmd.Connection = connection;





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



        public Guid AddVendorQuotation(Vendor vendor)
        {
            Guid vendorQuotationID = Guid.NewGuid();
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                
                cmd.CommandText = $"INSERT INTO VendorQuotations(VendorQuotationID, VendorID,Amount,DiscountRate, AdvancedRate, PaymentTerms, DeliveryTerms, PurchaseRequisitionID) " +
                    $" VALUES('{vendorQuotationID}', '{vendor.vendorId}','{vendor.amount}','{vendor.discountRate}','{vendor.advanceRate}','{vendor.paymentTerms}'," +
                    $"'{vendor.deliveryTerms}','{vendor.requisitionId}') ";

                cmd.Connection = connection;
                cmd.Connection.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return vendorQuotationID;
        }

        public void AddVendorQuotationItems(Vendor vendor, Guid vendorQuotationID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("VendorQuatoationID", typeof(Guid));
            dataTable.Columns.Add("ItemId", typeof(Guid));
            dataTable.Columns.Add("Quantity", typeof(decimal));
            dataTable.Columns.Add("UnitPrice", typeof(decimal));
            

            vendor.Items.ForEach(item => dataTable.Rows.Add(vendorQuotationID, item.itemId, item.quantity, item.unitPrice));
            using (connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "VendorQuotationItems";
                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }

        public List<Vendor> ViewFinalPurchaseOrder()
        {
            try
            {
                List<Vendor> vendor = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ve.VendorID, ve.VendorName,vq.RequisitionID, vq.VendorQuotationID,vq.Amount,vq.PaymentTerms,vq.DeliveryTerms" +
                    $" from Vendors ve Join VendorQuotations vq On ve.VendorID = vq.VendorID Left Join Requisitions rq On vq.RequisitionID = rq.RequisitionID Where rq.RequisitionStatus = 6";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vendor.Add(new Vendor
                    {
                        vendorId = reader["VendorID"] != DBNull.Value ? (Guid)reader["VendorID"] : Guid.Empty,
                        vendorQuotationId = reader["VendorQuotationID"] != DBNull.Value ? (Guid)reader["VendorQuotationID"] : Guid.Empty,
                        requisitionId = reader["VendorQuotationID"] != DBNull.Value ? (Guid)reader["VendorQuotationID"] : Guid.Empty,
                        amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m,
                        vendorName = reader["VendorName"] != DBNull.Value ? (string)reader["VendorName"] : string.Empty,
                        deliveryTerms = reader["DeliveryTerms"] != DBNull.Value ? (string)reader["DeliveryTerms"] : string.Empty,

                        paymentTerms = reader["PaymentTerms"] != DBNull.Value ? (string)reader["PaymentTerms"] : string.Empty,

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



        //<-------------------------------------------------Dashboard------------------------------------------------------------>

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

        public List<AddPurchaseRequisition> GetPurchaseRequisitionItems(Guid requisitionId)
        {

            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName,it.specification, pri.UnitPrice, pri.Quantity, pri.TotalPrice FROM PurchaseRequisitionItems pri " +
                                  $"JOIN Items it ON it.ItemId = pri.ItemID WHERE pri.PurchaseRequisitionID = '{requisitionId}'";
                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0,
                        RequisitionId = requisitionId,
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        TotalAmount = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m,
                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
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

        public List<Store_PR> FindStorePR()
        {
            try
            {
                List<Store_PR> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT sr.StorePRID, sr.StorePRSeries, sr.ItemID, sr.RequisitionID, i.ItemName, i.Specification, sr.Quantity, sr.CreatedAt FROM Store_PR sr JOIN Items i ON sr.ItemID=i.ItemId";
                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Store_PR
                    {
                        StorePRSeries = reader["StorePRSeries"] != DBNull.Value ? (string)reader["StorePRSeries"] : string.Empty,
                        StorePRID = reader["StorePRID"] != DBNull.Value ? (Guid)reader["StorePRID"] : Guid.Empty,
                        RequisitionID = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        CreatedAt = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),
                        item = new Item
                        {
                            itemId = reader["ItemID"] != DBNull.Value ? (Guid)reader["ItemID"] : Guid.Empty,
                            itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                            specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty
                        }
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

        public void UpdateStorePRStatus(List<Store_PR>? store_PRs, StorePRStatus status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Store_PR SET StorePRStatus = @Status WHERE StorePRID = @ID";
                    foreach (var pr in store_PRs)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {

                            cmd.Parameters.AddWithValue("@Status", (byte)status); 
                            cmd.Parameters.AddWithValue("@ID", pr.StorePRID);

                            cmd.ExecuteNonQuery(); 
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePurchaseRequisition(PurchaseRequisition requisition, RequisitionStatus status)
        {
            requisition.requisitionId = Guid.NewGuid();

            DataTable requisitionItemsTable = new DataTable();
            requisitionItemsTable.Columns.Add("ItemID", typeof(Guid));
            requisitionItemsTable.Columns.Add("Quantity", typeof(decimal));
            requisitionItemsTable.Columns.Add("UnitPrice", typeof (decimal));
            requisitionItemsTable.Columns.Add("PurchaseRequisitionID", typeof(Guid));
            //requisitionItemsTable.Columns.Add("Total")

            requisition.requisitionItems?
                .ForEach(item => requisitionItemsTable.Rows.Add(item.itemId, item.quantity, requisition.requisitionId));
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.DestinationTableName = "PurchaseRequisitionItems";
                        bulkCopy.WriteToServer(requisitionItemsTable);
                    }

                    string query = $"INSERT INTO PurchaseRequisitions(RequisitionID, Description, RequisitionStatus, RequisitionSeries) " +
                                   $"VALUES (@PurchaseRequisitionID, @Description, @RequisitionStatus, @RequisitionSeries); ";

                    using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@RequisitionID", requisition.requisitionId);
                        cmd.Parameters.AddWithValue("@Description", requisition.description);
                        cmd.Parameters.AddWithValue("@RequisitionStatus", (byte)RequisitionStatus.PENDING);
                        cmd.Parameters.AddWithValue("@RequisitionSeries", requisition.requisitionSeries);

                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }
        }
    }
}
