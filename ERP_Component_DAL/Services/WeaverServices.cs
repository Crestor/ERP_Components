﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

        public List<Weaver> ViewWorkOrder()
        {
           List <Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"select wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,ws.StatusName,wo.WorkOrderStatus,it.ItemName  from WorkOrder wo 
                                    join Items it on wo.ProductID = it.ItemId join WorkOrderStatuses ws on ws.WorkOrderStatus = wo.WorkOrderStatus where wo.WorkOrderStatus = 1";



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

        
        public Weaver ViewProductOfWorkOrder(Guid WorkOrederId)
        {
            Weaver weaver = new Weaver();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"SELECT wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,wo.WorkOrderStatus,i.ItemName,iv.InStock from WorkOrder wo \r\njoin Items i on wo.ProductID = i.ItemId join Inventory iv on iv.ItemId = i.ItemId JOIN DistributionCenter dc ON dc.CenterID = iv.CenterID where WorkOrderID = '{WorkOrederId}' AND dc.CenterType = 6";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
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
                    string query = $"SELECT wo.WorkOrderSeries,wo.WorkOrderID,wo.Quantity,wo.WorkOrderStatus,i.ItemName,i.Specification,iv.InStock from WorkOrder wo \r\njoin Items i on wo.ProductID = i.ItemId join Inventory iv on iv.ItemId = i.ItemId JOIN DistributionCenter dc ON dc.CenterID = iv.CenterID where WorkOrderID = '{WorkOrederId}' AND dc.CenterType = 6";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
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
                                    ProductName = reader["ItemName"]?.ToString(),
                                    Specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : string.Empty
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
        public bool AllocateToWarehouse(Guid WorkOrderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"update Workorder set WorkOrderStatus = 2  Where WorkOrderId = '{WorkOrderId}' ";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
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
        public List<Weaver> GetRequiredMaterial()
        {
            List<Weaver> weaver = new List<Weaver>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT p.ItemName AS ProductName, m.ItemName AS RequiredMaterial, (pmm.Quantity * wo.Quantity) AS RequiredQuantity, i.InStock AS AvailableQuantity
                                    FROM WorkOrder wo JOIN Items p ON p.ItemId = wo.ProductID
                                    JOIN ProductMaterialMapping pmm ON wo.ProductID = pmm.ProductID
                                    JOIN Items m ON m.ItemId = pmm.MaterialID
                                    JOIN Inventory i ON pmm.MaterialID = i.ItemId
                                    JOIN DistributionCenter dc ON i.CenterId = dc.CenterId
                                    WHERE dc.CenterType = 6";



                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
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


    }
}

        