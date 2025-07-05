using ERP_Component_DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
