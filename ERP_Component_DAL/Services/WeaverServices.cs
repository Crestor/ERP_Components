using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ERP_Component_DAL.Models.Weaver;

namespace ERP_Component_DAL.Services
{
    public class WeaverServices
    {
        private readonly string connectionString;
        public WeaverServices(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DefaultConnectionString");
        }

        public bool addWeaver(Weaver weaver)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AddWeaver", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@WorkerName", weaver.WeaverName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirmName", weaver.Firm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ContactNumber", weaver.ContactNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmailID", weaver.Email ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Country", weaver.Country ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", weaver.State ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", weaver.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pincode", weaver.Pincode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddressLine1", weaver.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@District", weaver.District ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@PANCard", GetBytesFromIFormFile(weaver.DocPANCard) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PANCardNumber", weaver.PANNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharImage", GetBytesFromIFormFile(weaver.DocAadhar) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharNumber", weaver.AadharNumber ?? (object)DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private byte[]? GetBytesFromIFormFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public List<Weaver> ViewWeaver()
        {
            List<Weaver> weavers = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT WorkerID, WorkerName, FirmName, ContactNumber FROM Workers", connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                weavers.Add(new Weaver
                                {
                                    WeaverId = reader["WorkerID"] != DBNull.Value ? (Guid)reader["WorkerID"] : Guid.Empty,
                                    WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty,
                                    Firm = reader["FirmName"] != DBNull.Value ? reader["FirmName"].ToString() : string.Empty,
                                    ContactNumber = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                }

                return weavers;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Weaver GetWeaverDetailsById(Guid workerId)
        {
            Weaver weaver = new Weaver();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT w.WorkerName, w.FirmName, w.ContactNumber, w.EmailID, a.Country, a.State, a.City, a.Pincode, a.AddressLine1, a.District, d.PANCardNumber, d.AadharNumber
                         FROM Workers w JOIN Address a ON w.AddressID = a.AddressID
                    JOIN Documents d ON w.DocumentID = d.DocumentID where WorkerID =@WorkerID";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkerID", workerId);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                weaver.WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty;
                                weaver.Firm = reader["FirmName"] != DBNull.Value ? reader["FirmName"].ToString() : string.Empty;
                                weaver.ContactNumber = reader["ContactNumber"] != DBNull.Value ? reader["ContactNumber"].ToString() : string.Empty;
                                weaver.Email = reader["EmailID"] != DBNull.Value ? reader["EmailID"].ToString() : string.Empty;
                                weaver.Country = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : string.Empty;
                                weaver.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : string.Empty;
                                weaver.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : string.Empty;
                                weaver.Pincode = reader["Pincode"] != DBNull.Value ? reader["Pincode"].ToString() : string.Empty;
                                weaver.Address = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : string.Empty;
                                weaver.District = reader["District"] != DBNull.Value ? reader["District"].ToString() : string.Empty;
                                weaver.PANNumber = reader["PANCardNumber"] != DBNull.Value ? reader["PANCardNumber"].ToString() : string.Empty;
                                weaver.AadharNumber = reader["AadharNumber"] != DBNull.Value ? reader["AadharNumber"].ToString() : string.Empty;



                            }
                        }
                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateWeaver(Weaver weaver)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UpdateWeaver", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WorkerID", weaver.WeaverId);
                        cmd.Parameters.AddWithValue("@WorkerName", weaver.WeaverName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirmName", weaver.Firm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ContactNumber", weaver.ContactNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmailID", weaver.Email ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Country", weaver.Country ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", weaver.State ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", weaver.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pincode", weaver.Pincode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddressLine1", weaver.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@District", weaver.District ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@PANCard", GetBytesFromIFormFile(weaver.DocPANCard) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PANCardNumber", weaver.PANNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharImage", GetBytesFromIFormFile(weaver.DocAadhar) ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AadharNumber", weaver.AadharNumber ?? (object)DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteWeaver(Guid weaverId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Workers WHERE WorkerID = @WorkerID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkerID", weaverId);
                        connection.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Weaver> ViewWorkOrder(WorkOrderStatuses workOrderStatus)
        {
           List <Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"select wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,ws.StatusName,wo.WorkOrderStatus,it.ItemName from WorkOrder wo 
                                   join Items it on wo.ProductID = it.ItemId join WorkOrderStatuses ws on ws.WorkOrderStatus = wo.WorkOrderStatus 
                                   where wo.WorkOrderStatus = @WorkOrderStatus ORDER BY wo.WorkOrderSeries";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkOrderStatus", (byte)workOrderStatus);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                        
                                while (reader.Read())
                                {
                                    weaver.Add(new Weaver
                                    {
                                        WorkOrderSeries = reader["WorkOrderSeries"] != DBNull.Value ? reader["WorkOrderSeries"].ToString() : string.Empty,
                                        WorkOrderId = reader["WorkOrderID"] != DBNull.Value ? (Guid)reader["WorkOrderID"] : Guid.Empty,
                                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                                        Status = reader["StatusName"] != DBNull.Value ? reader["StatusName"].ToString() : string.Empty,
                                        ProductName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty
                                    });
                                }


                        }
                        
                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public Weaver ViewProductOfWorkOrder(Guid WorkOrederId)
        {
            Weaver weaver = new Weaver();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,wo.WorkOrderStatus,i.ItemName,iv.InStock FROM WorkOrder wo 
                                     JOIN Items i ON wo.ProductID = i.ItemId JOIN Inventory iv ON iv.ItemId = i.ItemId JOIN DistributionCenter dc ON
                                     dc.CenterID = iv.CenterID WHERE WorkOrderID = @WorkOrderId AND dc.CenterType = 6";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkOrderId", WorkOrederId);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            
                                while (reader.Read())
                                {
                                weaver = new Weaver
                                {
                                    WorkOrderSeries = reader["WorkOrderSeries"]?.ToString(),
                                    WorkOrderId = reader["WorkOrderID"] != DBNull.Value ? (Guid)reader["WorkOrderID"] : Guid.Empty,
                                    requiredQuantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                                    availableQuantity = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0,
                                    Status = reader["WorkOrderStatus"]?.ToString(),
                                    ProductName = reader["ItemName"]?.ToString()
                                };
                            }


                        }
                        
                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Weaver ViewProductOfStartWeaving(Guid WorkOrederId)
        {
            Weaver weaver = new Weaver();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT wo.WorkOrderSeries,wo.Quantity,wo.WorkOrderStatus,i.ItemName, i.ItemID,i.Specification,iv.InStock, 
                        (SELECT SUM(Quantity) FROM AllocatedWork WHERE WorkOrderID = @WorkOrderId) AS AllocatedQuantity, 
                        (SELECT SUM(Quantity) FROM DyeingOrder WHERE WorkOrderID = @WorkOrderId) AS DyeingQuantity FROM WorkOrder wo 
                        JOIN Items i on wo.ProductID = i.ItemId JOIN Inventory iv ON iv.ItemId = i.ItemId 
                        JOIN DistributionCenter dc ON dc.CenterID = iv.CenterID where WorkOrderID = @WorkOrderId AND dc.CenterType = 6;";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkOrderId", WorkOrederId);
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if(reader.Read())
                            {
                                weaver = new Weaver
                                {
                                    WorkOrderSeries = reader["WorkOrderSeries"]?.ToString(),
                                    WorkOrderId = WorkOrederId,
                                    requiredQuantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                                    availableQuantity = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0,
                                    Status = reader["WorkOrderStatus"]?.ToString(),
                                    ProductName = reader["ItemName"]?.ToString(),
                                    Specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : string.Empty,
                                    AllocatedQuantity = reader["AllocatedQuantity"] != DBNull.Value ? Convert.ToInt32(reader["AllocatedQuantity"]) : 0,
                                    dyeingQuantity = reader["DyeingQuantity"] != DBNull.Value ? Convert.ToInt32(reader["DyeingQuantity"]): 0,
                                    ProductId = reader.GetGuid(reader.GetOrdinal("ItemID"))
                                };
                            }
                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Weaver GetPhases(Weaver workOrders)
        {
            try
            {
                workOrders.workOrderPhases = new List<WorkOrderPhases>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetPhases", connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WorkOrderID", workOrders.WorkOrderId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                workOrders.workOrderPhases.Add(new Weaver.WorkOrderPhases
                                    {
                                        phase = reader.GetByte("Phase"),
                                        phaseTime = reader.GetDecimal("PhaseTime"),
                                        phaseWork = reader.GetString("PhaseWork"),
                                        status = reader.GetString("Status")

                                    }
                                );
                            }


                        }

                    }
                }

                return workOrders;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public bool AllocateToWarehouse(Guid WorkOrderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AllocateToWarehouseFromWeaver", connection))
                    {
                        connection.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Items> GetProductionMaterial()
        {
            List<Items> weaver = new List<Items>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT it.ItemName,it.ItemId,it.Specification,i.InStock,it.UnitOFMeasure FROM Inventory i 
                                    JOIN DistributionCenter dc ON dc.CenterId = i.CenterId 
                                    JOIN Items it ON it.ItemId = i.ItemId
                                    WHERE dc.CenterType = 3 and it.ItemType = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Items
                                {
                                    itemName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty,
                                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                                    specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : string.Empty,
                                    inStock = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0,
                                    UOM = reader["UnitOFMeasure"] != DBNull.Value ? reader["UnitOFMeasure"].ToString() : string.Empty

                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Items> GetWeaverMaterial()
        {
            List<Items> weaver = new List<Items>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT it.ItemName,it.ItemId,it.Specification,i.InStock,it.UnitOFMeasure FROM Inventory i 
                                        JOIN DistributionCenter dc ON dc.CenterId = i.CenterId 
                                        JOIN Items it ON it.ItemId = i.ItemId
                                        WHERE dc.CenterType = 6 and it.ItemType = 2";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Items
                                {
                                    itemName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty,
                                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                                    specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : string.Empty,
                                    inStock = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0,
                                    UOM = reader["UnitOFMeasure"] != DBNull.Value ? reader["UnitOFMeasure"].ToString() : string.Empty

                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Items> GetWeaverProduct()
        {
            List<Items> weaver = new List<Items>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT it.ItemName,it.ItemId,it.Specification,i.InStock,it.UnitOFMeasure FROM Inventory i 
                                        JOIN DistributionCenter dc ON dc.CenterId = i.CenterId 
                                        JOIN Items it ON it.ItemId = i.ItemId
                                        WHERE dc.CenterType = 6 and it.ItemType = 1";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Items
                                {
                                    itemName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty,
                                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                                    specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : string.Empty,
                                    inStock = reader["InStock"] != DBNull.Value ? Convert.ToInt32(reader["InStock"]) : 0,
                                    UOM = reader["UnitOFMeasure"] != DBNull.Value ? reader["UnitOFMeasure"].ToString() : string.Empty

                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Weaver> ViewCompletedWorkOrder()
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"select wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,ws.StatusName,wo.WorkOrderStatus,it.ItemName  from WorkOrder wo 
                                    join Items it on wo.ProductID = it.ItemId join WorkOrderStatuses ws on ws.WorkOrderStatus = wo.WorkOrderStatus where wo.WorkOrderStatus = 3";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Weaver
                                {
                                    WorkOrderSeries = reader["WorkOrderSeries"] != DBNull.Value ? reader["WorkOrderSeries"].ToString() : string.Empty,
                                    WorkOrderId = reader["WorkOrderID"] != DBNull.Value ? (Guid)reader["WorkOrderID"] : Guid.Empty,
                                    Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                                    Status = reader["StatusName"] != DBNull.Value ? reader["StatusName"].ToString() : string.Empty,
                                    ProductName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty
                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Weaver> ViewOngoingWorkOrder()
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"select wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,ws.StatusName,wo.WorkOrderStatus,it.ItemName  from WorkOrder wo 
                                    join Items it on wo.ProductID = it.ItemId join WorkOrderStatuses ws on ws.WorkOrderStatus = wo.WorkOrderStatus where wo.WorkOrderStatus = 2";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Weaver
                                {
                                    WorkOrderSeries = reader["WorkOrderSeries"] != DBNull.Value ? reader["WorkOrderSeries"].ToString() : string.Empty,
                                    WorkOrderId = reader["WorkOrderID"] != DBNull.Value ? (Guid)reader["WorkOrderID"] : Guid.Empty,
                                    Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                                    Status = reader["StatusName"] != DBNull.Value ? reader["StatusName"].ToString() : string.Empty,
                                    ProductName = reader["ItemName"] != DBNull.Value ? reader["ItemName"].ToString() : string.Empty
                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Weaver> GetRequiredMaterial(Guid WorkOrderID)
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT p.ItemName AS ProductName,m.ItemId, m.ItemName AS RequiredMaterial, (pmm.Quantity * wo.Quantity) AS RequiredQuantity, i.InStock AS AvailableQuantity
                                    FROM WorkOrder wo JOIN Items p ON p.ItemId = wo.ProductID
                                    JOIN ProductMaterialMapping pmm ON wo.ProductID = pmm.ProductID
                                    JOIN Items m ON m.ItemId = pmm.MaterialID
                                    JOIN Inventory i ON pmm.MaterialID = i.ItemId
                                    JOIN DistributionCenter dc ON i.CenterId = dc.CenterId
                                    WHERE dc.CenterType = 6 AND wo.WorkOrderID = @WorkOrderID";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Weaver
                                {
                                    ProductName = reader["ProductName"] != DBNull.Value ? reader["ProductName"].ToString() : string.Empty,
                                    requiredQuantity = reader["RequiredQuantity"] != DBNull.Value ? Convert.ToInt32(reader["RequiredQuantity"]) : 0,
                                    availableQuantity = reader["AvailableQuantity"] != DBNull.Value ? Convert.ToInt32(reader["AvailableQuantity"]) : 0,
                                    MaterialName = reader["RequiredMaterial"] != DBNull.Value ? reader["RequiredMaterial"].ToString() : string.Empty,
                                    MaterialId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty
                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public bool InsertMaterialRequisition(Weaver model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertRequisitionQuery = @"
                INSERT INTO Requisitions (Description, RequisitionType, RequisitionSeries, RequisitionStatus)
                OUTPUT INSERTED.RequisitionID
                VALUES (@Description, 3, @Series, 1)";

                    SqlCommand cmd1 = new SqlCommand(insertRequisitionQuery, connection);
                    cmd1.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd1.Parameters.AddWithValue("@Series", model.RequisitionSeries ?? "");

                    Guid requisitionId = (Guid)cmd1.ExecuteScalar();

                    string bridgeQuery = @"
                INSERT INTO WorkOrderRequisitonBridge (WorkOrderID, RequisitionID)
                VALUES (@WorkOrderID, @RequisitionID)";

                    SqlCommand cmd2 = new SqlCommand(bridgeQuery, connection);
                    cmd2.Parameters.AddWithValue("@WorkOrderID", model.WorkOrderId);
                    cmd2.Parameters.AddWithValue("@RequisitionID", requisitionId);
                    cmd2.ExecuteNonQuery();

                    if (model.MaterialRequired != null)
                    {
                        foreach (var item in model.MaterialRequired)
                        {
                            string insertItemQuery = @"
                        INSERT INTO RequisitionItems (ItemID, Quantity, RequisitionID)
                        VALUES (@ItemID, @Quantity, @RequisitionID)";

                            SqlCommand itemCmd = new SqlCommand(insertItemQuery, connection);
                            itemCmd.Parameters.AddWithValue("@ItemID", item.MaterialId);
                            itemCmd.Parameters.AddWithValue("@Quantity", item.requiredQuantity);
                            itemCmd.Parameters.AddWithValue("@RequisitionID", requisitionId);
                            itemCmd.ExecuteNonQuery();
                        }
                    }

                    string updateWorkOrderQuery = @"
                UPDATE WorkOrder
                SET WorkOrderStatus = 4
                WHERE WorkOrderID = @WorkOrderID";

                    SqlCommand cmd3 = new SqlCommand(updateWorkOrderQuery, connection);
                    cmd3.Parameters.AddWithValue("@WorkOrderID", model.WorkOrderId);
                    cmd3.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<Weaver> GetWeavers()
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT WorkerID, WorkerName FROM Workers WHERE WorkerType = 1";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Weaver
                                {
                                    WeaverId = reader["WorkerID"] != DBNull.Value ? (Guid)reader["WorkerID"] : Guid.Empty,
                                    WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty
                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Weaver> GetDyer()
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT WorkerID, WorkerName FROM Workers WHERE WorkerType = 2";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                weaver.Add(new Weaver
                                {
                                    WeaverId = reader["WorkerID"] != DBNull.Value ? (Guid)reader["WorkerID"] : Guid.Empty,
                                    WeaverName = reader["WorkerName"] != DBNull.Value ? reader["WorkerName"].ToString() : string.Empty
                                });
                            }


                        }

                    }
                }

                return weaver;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AllocateToWeaver(AllocatedWork allocatedWork)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO AllocatedWork (WorkOrderID, AllocationCode, WorkerID, Quantity, RatePerPeices, AllocatedYarnID)
                                     VALUES (@WorkOrderID, @AllocationCode, @WorkerID, @Quantity, @RatePerPieces, @AllocatedYarnID);
                                      UPDATE Inventory SET InStock = InStock - @Quantity WHERE ItemID = @AllocatedYarnID AND CenterId = 'ee874827-af6d-4f9f-8bde-066ad03bfb86'";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkOrderID", allocatedWork.WorkOrderID);
                        cmd.Parameters.AddWithValue("@AllocationCode", allocatedWork.AllocationCode ?? (object)DBNull.Value); 
                        cmd.Parameters.AddWithValue("@WorkerID", allocatedWork.WorkerID);
                        cmd.Parameters.AddWithValue("@Quantity", allocatedWork.Quantity);
                        cmd.Parameters.AddWithValue("@RatePerPieces", allocatedWork.RatePerPeices); 
                        cmd.Parameters.AddWithValue("@AllocatedYarnID", allocatedWork.AllocatedYarnID ?? (object)DBNull.Value); 

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public VeiwOrdersReadyForDyeing GetOrdersWithCompletedWeavingProducts()
        {
            
            VeiwOrdersReadyForDyeing eiwOrdersReadyForDyeing = new VeiwOrdersReadyForDyeing();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT wo.WorkOrderID, wo.WorkOrderSeries, i.ItemName, wo.Quantity, i.specification, (SUM(aw.RecievedQuantity) - ISNULL(SUM(dom.Quantity), 0)) AS TotalReceived
                                    FROM WorkOrder wo JOIN AllocatedWork aw ON aw.WorkOrderID = wo.WorkOrderID
                                    JOIN Items i ON wo.ProductID = i.ItemID
                                    LEFT JOIN DyeingOrderMapping dom ON wo.WorkOrderID = dom.WorkOrderID
                                    LEFT JOIN DyeingOrder do ON do.DyeingOrderID = dom.DyeingOrderID
                                    GROUP BY wo.WorkOrderID, wo.WorkOrderSeries, i.ItemName, i.specification, wo.Quantity
                                    HAVING (SUM(aw.RecievedQuantity) - ISNULL(SUM(dom.Quantity), 0)) > 0;
                                    ";

                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        eiwOrdersReadyForDyeing.orders = new List<VeiwOrdersReadyForDyeing.ReadyToDye>();
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                eiwOrdersReadyForDyeing.orders.Add(
                                    new VeiwOrdersReadyForDyeing.ReadyToDye
                                    {
                                        workOrderId = reader.GetGuid("WorkOrderID"),
                                        workOrderSeries = reader.GetString("WorkOrderSeries"),
                                        productName = reader.GetString("ItemName"),
                                        specifications = reader.GetString("specification"),
                                        totalQuantity = reader.GetDecimal("Quantity"),
                                        totalReceived = reader.GetInt32("TotalReceived")
                                    }
                                );
                            }
                        }
                    }                    
                }
                return eiwOrdersReadyForDyeing;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateWorkOrderStatus(Guid workOrderId, WorkOrderStatuses status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE WorkOrder SET WorkOrderStatus = @Status WHERE WorkOrderID = @WorkOrderID";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Status", (byte)status);
                        cmd.Parameters.AddWithValue("@WorkOrderID", workOrderId);
                        cmd.ExecuteScalar();
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void AllocateToDyer(DyeingOrder dyeingOrder)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO DyeingOrder (WorkerID, Quantity, OrderStatus, WorkOrderID)
                                 VALUES (@WorkerID, @Quantity, @OrderStatus, @WorkOrderID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkerID", dyeingOrder.WorkerID);
                        cmd.Parameters.AddWithValue("@Quantity", dyeingOrder.Quantity);
                        cmd.Parameters.AddWithValue("@OrderStatus", (byte)WorkOrderStatuses.PENDING);
                        cmd.Parameters.AddWithValue("@WorkOrderID", dyeingOrder.WorkOrderID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {            
                throw;
            }
        }

        public List<AllocatedWork> FindWeavingOrders()
        {
            List<AllocatedWork> weavingOrders = new List<AllocatedWork>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT aw.WorkOrderID, aw.AllocationCode, aw.WorkerID, w.WorkerName, aw.Quantity, aw.RatePerPeices, aw.CreatedAt, 
                                    aw.RecievedQuantity, aw.WorkStatus,aw.AllocatedWorkID, aw.AllocatedYarnID FROM AllocatedWork aw JOIN Workers w ON aw.WorkerID = w.WorkerID where aw.RecievedQuantity<aw.Quantity;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AllocatedWork allocatedWork = new AllocatedWork
                                {
                                    WorkOrderID = reader.GetGuid(reader.GetOrdinal("WorkOrderID")),
                                    AllocationCode = reader.GetString(reader.GetOrdinal("AllocationCode")),
                                    WorkerID = reader.GetGuid(reader.GetOrdinal("WorkerID")),
                                    WorkerName = reader.GetString(reader.GetOrdinal("WorkerName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    RatePerPeices = reader.GetDecimal(reader.GetOrdinal("RatePerPeices")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    RecievedQuantity = reader.GetInt32(reader.GetOrdinal("RecievedQuantity")),
                                    WorkStatus = reader.GetByte(reader.GetOrdinal("WorkStatus")),
                                    AllocatedYarnID = reader.IsDBNull(reader.GetOrdinal("AllocatedYarnID")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("AllocatedYarnID")),
                                    AllocatedWorkID= reader.GetGuid(reader.GetOrdinal("AllocatedWorkID"))

                                };
                                weavingOrders.Add(allocatedWork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return weavingOrders;
        }

        public List<DyeingOrder> FindDyeingOrders()
        {
            List<DyeingOrder> dyeingOrders = new List<DyeingOrder>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT do.DyeingOrderID, do.WorkerID, w.WorkerName, do.Quantity, do.OrderStatus, do.WorkOrderID FROM DyeingOrder do JOIN Workers w ON do.WorkerID = w.WorkerID where do.OrderStatus = 1;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DyeingOrder dyeingOrder = new DyeingOrder
                                {
                                    DyeingOrderID = reader.GetGuid(reader.GetOrdinal("DyeingOrderID")),
                                    WorkerID = reader.GetGuid(reader.GetOrdinal("WorkerID")),
                                    WorkerName = reader.GetString(reader.GetOrdinal("WorkerName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    OrderStatus = (WorkOrderStatuses)reader.GetByte(reader.GetOrdinal("OrderStatus")),
                                    WorkOrderID = reader.GetGuid(reader.GetOrdinal("WorkOrderID"))
                                };
                                dyeingOrders.Add(dyeingOrder);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return dyeingOrders;
        }

        public void UpdateDyeingOrder(Guid dyeingOrderID, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE DyeingOrder SET OrderStatus = 3 WHERE DyeingOrderID = @DyeingOrderID
                                    UPDATE Inventory SET InStock = InStock + @Quantity WHERE ItemId = (SELECT wop.OutPutProductID FROM WorkOrderPhases wop 
                                    JOIN WorkOrder wo ON wop.ProductID = wo.ProductID
                                    JOIN DyeingOrder do ON do.WorkOrderID=wo.WorkOrderID
                                    WHERE wop.Phase = 2 AND do.DyeingOrderID = @DyeingOrderID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@DyeingOrderID", dyeingOrderID);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public void UpdateWeavingOrder(AllocatedWork allocated)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE AllocatedWork SET RecievedQuantity = @RecievedQuantity WHERE AllocatedWorkID = @AllocatedWorkID;
                                     UPDATE Inventory SET InStock = InStock + @RecievedQuantity WHERE ItemId = (SELECT wop.OutPutProductID FROM WorkOrderPhases wop 
                                     JOIN WorkOrder wo ON wop.ProductID = wo.ProductID
                                     JOIN AllocatedWork aw ON aw.WorkOrderID=wo.WorkOrderID
                                     WHERE wop.Phase = 1 AND aw.AllocatedWorkID = @AllocatedWorkID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RecievedQuantity", allocated.RecievedQuantity);
                        //cmd.Parameters.AddWithValue("@AllocatedWorkID", allocatedWorkID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<YarnInfo> GetYarnDetails(Guid ProductID)
        {
            List<YarnInfo> yarns = new List<YarnInfo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT it.ItemName AS YarnName, it.ItemId AS YarnID, it.Specification FROM Weaving_BOM wb " +
                                    "JOIN Items it ON wb.MaterialID = it.ItemId " +
                                     "WHERE wb.ProductID = @ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                YarnInfo yarn = new YarnInfo
                                {
                                    YranID = reader.GetGuid(reader.GetOrdinal("YarnID")),
                                    YarnName = reader.GetString(reader.GetOrdinal("YarnName")),
                                    Specification = reader.IsDBNull(reader.GetOrdinal("Specification")) ? null : reader.GetString(reader.GetOrdinal("Specification"))
                                };
                                yarns.Add(yarn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                throw;
            }

            return yarns;
        }
    }
}

        