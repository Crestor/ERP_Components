using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Services
{
    public class WarehouseServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;


        public WarehouseServices(IConfiguration config)
        {
            this.configuration = config;
        }



        public bool WarehouseLocation(Warehouse warehouse)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO WarehouseLocation([WarehouseId],[ZoneName],[Description],[AisleNumber],[CreatedOn],[LocationCode],[ShelfNumber])  VALUES('{warehouse.warehouseId}','{warehouse.zoneName}','{warehouse.description}','{warehouse.aisle}','{DateTime.Now.ToShortDateString()}','{warehouse.locationCode}','{warehouse.shelfNumber}')";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;



                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public List<Items> GetItemsNames()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


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

        public List<Warehouse> getWarehouseName()
        {
            try
            {
                List<Warehouse> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CenterName, CenterId from DistributionCenter";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Warehouse()
                    {
                        warehouseId = reader["CenterId"] != DBNull.Value ? (Guid)reader["CenterId"] : Guid.Empty,


                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,


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

      public List<Warehouse> ViewWarehouseLocation()
        {
            try
            {
                List<Warehouse> warehouse = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ln.LocationId,ln.ZoneName,ln.ShelfNumber,ln.AisleNumber,ln.Description,ln.LocationCode,ln.CreatedOn,wh.WarehouseType,wh.WarehouseName FROM  Warehouses wh JOIN  WarehouseLocation ln ON wh.warehouseId = ln.WarehouseId";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    warehouse.Add(new Warehouse()
                    {
                        locationId = reader["LocationId"] != DBNull.Value ? (int)reader["LocationId"] : 0,
                        //PostalCode = reader["Pincode"] != DBNull.Value ? ()reader["Pincode"] : 0,
                        warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty,
                        //type = reader["WarehouseType"] != DBNull.Value ? (string)reader["WarehouseType"] : string.Empty,
                        shelfNumber = reader["ShelfNumber"] != DBNull.Value ? (string)reader["ShelfNumber"] : string.Empty,
                        aisle = reader["AisleNumber"] != DBNull.Value ? (string)reader["AisleNumber"] : string.Empty,
                        zoneName = reader["ZoneName"] != DBNull.Value ? (string)reader["ZoneName"] : string.Empty,
                        description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        locationCode = reader["LocationCode"] != DBNull.Value ? (string)reader["LocationCode"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),


                    });
                }

                return warehouse;

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
                cmd.CommandText = $"   SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId Where iv.InventoryCenter = 2";
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



        public List<AddPurchaseRequisition> ViewSalesForCasting()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select RequisitionID, RequisitionSeries,Description,CreatedAt From Requisitions  Where RequisitionStatus = 1 AND RequisitionType = 1";
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



        public List<AddPurchaseRequisition> ViewCompletedProductionOrder()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT r.RequisitionID, r.Description, r.RequisitionSeries, r.CreatedAt FROM Requisitions r JOIN ProductionOrder po ON po.SalesForcastID = r.RequisitionID WHERE r.RequisitionType = 1 AND r.RequisitionStatus = 4 GROUP BY r.RequisitionID, r.Description, r.RequisitionSeries,r.CreatedAt HAVING COUNT(*) = SUM(CASE WHEN po.ProductionStatus = 4 THEN 1 ELSE 0 END)";
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

        public List<AddPurchaseRequisition> ViewSentRequisitions()
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select RequisitionID, RequisitionSeries,Description,CreatedAt From Requisitions  Where RequisitionStatus = 4 AND RequisitionType = 1";
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




        public List<AddPurchaseRequisition> GetRequisitionItemsListData(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT  it.ItemName, it.HSN, it.Specification, it.UnitOfMeasure,ri.RequisitionID, ri.Quantity FROM Items it JOIN RequisitionItems ri ON it.ItemId = ri.ItemId Where ri.RequisitionID = '{RequisitionID}'";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {

                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                     
                        hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,


                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,

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



        public List<AddPurchaseRequisition> GetRequisitionItemsListStatus(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemName, it.HSN, it.Specification,ps.StatusName, it.UnitOfMeasure,ri.RequisitionID, ri.Quantity"
                 + " FROM Items it"
                 + $" JOIN ProductionOrder po ON it.ItemId = po.ProductID"
                 + $" LEFT JOIN ProductionStatuses ps ON po.ProductionStatus = ps.ProductionStatus"
                 + $" LEFT JOIN RequisitionItems ri ON po.SalesForcastID = ri.RequisitionID"
                 + $" Where po.SalesForcastID = '{RequisitionID}'"
                 + $" GROUP BY it.ItemName, it.HSN, it.Specification,ps.StatusName, it.UnitOfMeasure,ri.RequisitionID, ri.Quantity;";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new AddPurchaseRequisition
                    {

                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

                        hsn = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,


                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                         status = reader["StatusName"] != DBNull.Value ? (string)reader["StatusName"] : string.Empty,
                        unitofmeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,

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





        public bool UpdateRequisitionTypeAndSentToProduction(Guid RequisitionId)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Requisitions Set  RequisitionStatus = 4 where RequisitionID = '{RequisitionId}'";



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

        public Warehouse GetWarehouseLocation(int locationId)
        {
            try
            {
                Warehouse warehouse = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select LocationId,ZoneName,ShelfNumber,AisleNumber,LocationCode From WarehouseLocation Where LocationId = '{locationId}' ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {



                    warehouse.locationId = reader["LocationId"] != DBNull.Value ? (int)reader["LocationId"] : 0;
                    warehouse.shelfNumber = reader["ShelfNumber"] != DBNull.Value ? (string)reader["ShelfNumber"] : string.Empty;
                    warehouse.aisle = reader["AisleNumber"] != DBNull.Value ? (string)reader["AisleNumber"] : string.Empty;
                    warehouse.zoneName = reader["ZoneName"] != DBNull.Value ? (string)reader["ZoneName"] : string.Empty;

                    warehouse.locationCode = reader["LocationCode"] != DBNull.Value ? (string)reader["LocationCode"] : string.Empty;
                        



                }


                return warehouse;

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



        public bool CreateProductionOrder(Guid RequisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateProductionOrder", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@RequisitionId", RequisitionId);

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



        public bool UpdateWarehouseLocation(Warehouse warehouse)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update WarehouseLocation set  WarehouseId ='{warehouse.warehouseId}',ZoneName='{warehouse.zoneName}',ShelfNumber='{warehouse.shelfNumber}',AisleNumber='{warehouse.aisle}',LocationCode='{warehouse.locationCode}',Description='{warehouse.description}' where LocationId = '{warehouse.locationId}'";

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

        public bool DeleteWarehouseLocation(int locationId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  WarehouseLocation  where LocationId = '{locationId}'";

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


        public int GetItemSKU(Guid itemId)
        {
            try
            {
                int sku = 0;
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select SKU from Items where ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    sku = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0;

                }


                return sku;

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

        public List<Warehouse> GetWarehouseZoneNames(Guid warehouseId)
        {
            try
            {
                List<Warehouse> warehouse = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT LocationId,  ZoneName From WarehouseLocation Where WarehouseId  = '{warehouseId}'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    warehouse.Add(new Warehouse()
                    {
                        locationId = reader["LocationId"] != DBNull.Value ? (int)reader["LocationId"] : 0,

                        zoneName = reader["ZoneName"] != DBNull.Value ? (string)reader["ZoneName"] : string.Empty,
                       


                    });
                }

                return warehouse;

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

        public bool AddWarehouseStockDetails(Items item)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO WarehouseStockDetails([ItemId],[LocationId],[Quantity],[BatchNumber],[CreatedDate])  VALUES('{item.itemId}','{item.locationId}','{item.quantity}','{item.batchSeries}','{DateTime.Now.ToShortDateString()}')";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;



                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public List<Warehouse> WarehouseStockView()
        {
            try
            {
                List<Warehouse> StockDetail = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemName,wh.Quantity, wh.StockDetailId,wl.AisleNumber,wl.ZoneName,wl.ShelfNumber,wh.CreatedDate FROM Items it JOIN WarehouseStockDetails wh ON it.ItemId = wh.ItemId LEFT JOIN WarehouseLocation wl ON wh.LocationId = wl.LocationId";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StockDetail.Add(new Warehouse()
                    {
                        detailId = reader["StockDetailId"] != DBNull.Value ? (Guid)reader["StockDetailId"] : Guid.Empty,
                        
                        quantity = reader["quantity"] != DBNull.Value ? (int)reader["quantity"] : 0,
                        zoneName = reader["ZoneName"] != DBNull.Value ? (string)reader["ZoneName"] : string.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

                        shelfNumber = reader["ShelfNumber"] != DBNull.Value ? (string)reader["ShelfNumber"] : string.Empty,
                        aisle = reader["AisleNumber"] != DBNull.Value ? (string)reader["AisleNumber"] : string.Empty,

                    });
                }

                return StockDetail;

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

        public Warehouse getWarehouseStock(Guid detailId)
        {
            try
            {
                Warehouse warehouse = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT wh.StockDetailId, wh.Quantity, it.ItemName,wl.AisleNumber,wl.ZoneName,wl.ShelfNumber,wh.CreatedDate FROM Items it JOIN WarehouseStockDetails wh ON it.ItemId = wh.ItemId LEFT JOIN WarehouseLocation wl ON wh.LocationId = wl.LocationId Where wh.StockDetailId = '{detailId}' ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    warehouse.quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0;
                    warehouse.detailId = reader["StockDetailId"] != DBNull.Value ? (Guid)reader["StockDetailId"] : Guid.Empty;
                    warehouse.shelfNumber = reader["ShelfNumber"] != DBNull.Value ? (string)reader["ShelfNumber"] : string.Empty;
                    warehouse.aisle = reader["AisleNumber"] != DBNull.Value ? (string)reader["AisleNumber"] : string.Empty;
                    warehouse.zoneName = reader["ZoneName"] != DBNull.Value ? (string)reader["ZoneName"] : string.Empty;
                    warehouse.CreatedOn = reader["CreatedDate"] != DBNull.Value ? ((DateTime)reader["CreatedDate"]).Date : default(DateTime);
                    warehouse.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;




                }


                return warehouse;

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


        public bool UpdateWarehouseStock(Warehouse warehouse)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update WarehouseStockDetails set  ItemId ='{warehouse.itemId}',LocationId='{warehouse.locationId}',Quantity='{warehouse.quantity}',BatchNumber='{warehouse.batchSeries}',UpdatedAt='{DateTime.Now.ToShortDateString()}' where StockDetailId = '{warehouse.detailId}'";

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

        public bool DeleteWarehouseStock(Guid detailId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  WarehouseStockDetails  where StockDetailId = '{detailId}'";

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



        public bool AllocateToSalesFromWarehouse(Guid salesForcastId)
        {

            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("AllocateToSalesFromWarhouse", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("SalesForecastID", salesForcastId);


                        connection.Open();
                        cmd.ExecuteNonQuery();


                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
