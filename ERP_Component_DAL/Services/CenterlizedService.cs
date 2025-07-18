using ERP_Component_DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ERP_Component_DAL.Services
{
    public class CenterlizedService
    {
        private string _connectionString;
        private SqlConnection connection;
        public CenterlizedService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnectionString");
        }

        public List<Items> ViewInventory(Guid CenterID)
        {
            try
            {
                List<Items> item = new();
                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId Where iv.CenterID = @CenterID";
                cmd.Parameters.AddWithValue("@CenterID", CenterID);
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

        public bool updateSalesForecastDetails(QuotationModel Aq, Guid CenterID, RequisitionTypes requisitionType)
        {
            try
            {
                connection = new SqlConnection(_connectionString);

                string query = $"UPDATE Requisitions SET Description = @Description, RequisitionSeries = @RequisitionSeries, " +
                    $"RequisitionType = @RequisitionType, RequisitionStatus=1 WHERE RequisitionID = @RequisitionID " +
                    $"INSERT INTO RequisitionsDistributionCenterBridge (RequisitionID, CenterId) VALUES " +
                    $"(@RequisitionID, @CenterID)";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", Aq.Description);
                cmd.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
                cmd.Parameters.AddWithValue("@RequisitionType", (byte)requisitionType);
                cmd.Parameters.AddWithValue("@CenterID", CenterID);
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

        public void SaveRequisition(Requisition requisition, Guid centerID, RequisitionTypes requisitionType)
        {
            requisition.requisitionId = Guid.NewGuid();

            DataTable requisitionItemsTable = new DataTable();
            requisitionItemsTable.Columns.Add("ItemID", typeof(Guid));
            requisitionItemsTable.Columns.Add("Quantity", typeof(decimal));
            requisitionItemsTable.Columns.Add("RequisitionID", typeof(Guid));

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
                        bulkCopy.DestinationTableName = "RequisitionItems";
                        bulkCopy.WriteToServer(requisitionItemsTable);
                    }

                    string query = $"INSERT INTO Requisitions(RequisitionID, Description, RequisitionStatus, RequisitionSeries, RequisitionType) " +
                                   $"VALUES (@RequisitionID, @Description, @RequisitionStatus, @RequisitionSeries, @RequisitionType); " +
                                   $"INSERT INTO RequisitionsDistributionCenterBridge(RequisitionID, CenterID) " +
                                   $"VALUES (@RequisitionID, @CenterID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@RequisitionID", requisition.requisitionId);
                        cmd.Parameters.AddWithValue("@Description", requisition.description);
                        cmd.Parameters.AddWithValue("@RequisitionStatus", (byte)RequisitionStatus.PENDING);
                        cmd.Parameters.AddWithValue("@RequisitionSeries", requisition.requisitionSeries);
                        cmd.Parameters.AddWithValue("@RequisitionType", (byte)requisitionType);
                        cmd.Parameters.AddWithValue("@CenterID", centerID);

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

        //TODO: 
        public List<Requisition> FindRequisitionsByType(RequisitionTypes type)
        {
            List<Requisition> requisitions = new List<Requisition>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT RequisitionID, [Description], CreatedAt, RequisitionSeries, RequisitionStatus, RequisitionType FROM Requisitions WHERE RequisitionType = @Type";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Type", (byte)type);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Requisition req = new Requisition
                                {
                                    requisitionId = reader.GetGuid(reader.GetOrdinal("RequisitionID")),
                                    // Ensure column names match what's selected in the query and type casting is correct
                                    requisitionSeries = reader.IsDBNull(reader.GetOrdinal("RequisitionSeries")) ? null : reader.GetString(reader.GetOrdinal("RequisitionSeries")),
                                    description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                    createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    requisitionType = (RequisitionTypes)reader.GetByte(reader.GetOrdinal("RequisitionType")),
                                    requisitionStatus = (RequisitionStatus)reader.GetByte(reader.GetOrdinal("RequisitionStatus"))
                                    // requisitionItems and items lists are not populated by this query
                                };
                                requisitions.Add(req);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return requisitions;
        }

        public void UpdateRequisitionStatus(Guid requisitionId, RequisitionStatus requisitionStatus)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Requisitions SET RequisitionStatus = @RequisitionStatus WHERE RequisitionID = @RequisitionID";
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RequisitionStatus", (byte)requisitionStatus);
                        cmd.Parameters.AddWithValue("@RequisitionID", requisitionId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateRequisitionItems(Requisition requisition)
        {

            //SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string updateQuery = "UPDATE RequisitionItems SET Quantity = @Quantity WHERE RequisitionID = @RequisitionID AND ItemID = @ItemID";
                    connection.Open();
                    //transaction = connection.BeginTransaction();
                    foreach (var item in requisition.requisitionItems)
                    {
                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection/*,transaction*/))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", item.quantity);
                            cmd.Parameters.AddWithValue("@ItemID", item.itemId);
                            cmd.Parameters.AddWithValue("@RequisitionID", requisition.requisitionId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    //transaction.Commit();
                }
            }
            catch (Exception)
            {
                //transaction?.Rollback();
                throw;
            }
        }
    }
}
