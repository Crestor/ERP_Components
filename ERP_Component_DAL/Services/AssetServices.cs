using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;


namespace ERP_Component_DAL.Services
{

    public class AssetServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;

        public AssetServices(IConfiguration config)
        {
            this.configuration = config;
        }





        public bool AddAsset(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Items([ItemName],[ItemType]) OUTPUT Inserted.ItemId values('{asset.itemName}','{asset.itemType}')";

                cmd.Connection = connection;

                connection.Open();
                Guid ItemId = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"Insert into Asset ([ItemId],[WarrentyEndDate],[SerialNumber],[Description],[Status],[Price]) values(@ItemId,'{asset.warrantyEndDate}','{asset.serialNumber}','{asset.description}','{asset.status}','{asset.price}')";

                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@ItemId", ItemId);
                //Guid lotId = (Guid)cmd1.ExecuteScalar();

                connection.Open();
                cmd1.ExecuteScalar();
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

        public List<Asset> GetAssetView()
        {
            try
            {
                List<Asset> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT 
        i.ItemId,
        a.AssetId,
        a.Price,
        a.WarrentyEndDate,
        a.Description,
        a.SerialNumber,
     
        i.ItemName
       
    FROM Asset a
    LEFT JOIN Items i ON a.ItemId = i.ItemId
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Asset()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        assetId = reader["AssetId"] != DBNull.Value ? (Guid)reader["AssetId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty,
                        warrantyEndDate = reader["WarrentyEndDate"] != DBNull.Value ? ((DateTime)reader["WarrentyEndDate"]).Date : default(DateTime),
                        price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m,

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

        public Asset EditAsset(Guid assetId)
        {
            try
            {
                Asset asset = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT i.ItemId, a.AssetId, a.Price, a.WarrentyEndDate, a.Description,a.SerialNumber, i.ItemName FROM Asset a  LEFT JOIN Items i ON a.ItemId = i.ItemId where a.AssetId = '{assetId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    asset.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                    asset.assetId = reader["AssetId"] != DBNull.Value ? (Guid)reader["AssetId"] : Guid.Empty;
                    asset.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    asset.description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty;
                    asset.serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty;
                    asset.warrantyEndDate = reader["WarrentyEndDate"] != DBNull.Value ? ((DateTime)reader["WarrentyEndDate"]).Date : default(DateTime);
                    asset.price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m;



                }


                return asset;

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


        public bool UpdateAsset(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Asset set Price='{asset.price}',WarrentyEndDate='{asset.warrantyEndDate}',Description='{asset.description}', SerialNumber='{asset.serialNumber}' where ItemId = '{asset.itemId}'; Update Items Set ItemName = '{asset.itemName}' Where ItemId = '{asset.itemId}'";

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


        public bool DeleteAsset(Guid itemId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" Delete From  Asset where ItemId = '{itemId}'; Delete From  Items where ItemId = '{itemId}' ";

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
