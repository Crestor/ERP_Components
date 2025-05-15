using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data;


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

                cmd.CommandText = $"Insert into Items([ItemName],[ItemType],[CategoryId]) OUTPUT Inserted.ItemId values('{asset.itemName}','{asset.itemType}','{asset.categoryId}')";

                cmd.Connection = connection;

                connection.Open();
                Guid ItemId = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"Insert into Asset ([ItemId],[WarrentyEndDate],[SerialNumber],[Description],[AssetStatus],[Price],[AssignTo],[Depreciation]) values(@ItemId,'{asset.warrantyEndDate}','{asset.serialNumber}','{asset.description}','{asset.status}','{asset.price}','{asset.assignto}','{asset.depreciation}')";

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
        a.AssignTo,
        i.ItemName,
       c.CategoryName
       
    FROM Asset a
    LEFT JOIN Items i ON a.ItemId = i.ItemId
    LEFT JOIN Categories c ON i.CategoryId = c.CategoryId
   
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
                        category = reader["categoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty,
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
                cmd.CommandText = $" SELECT i.ItemId, a.AssetId, a.Price, a.WarrentyEndDate,a.AssignTo, a.Description,a.SerialNumber, i.ItemName FROM Asset a  LEFT JOIN Items i ON a.ItemId = i.ItemId where a.AssetId = '{assetId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    asset.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                    asset.assetId = reader["AssetId"] != DBNull.Value ? (Guid)reader["AssetId"] : Guid.Empty;
                    asset.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    asset.assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty;
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
                cmd.CommandText = $"Update Asset set Price='{asset.price}',WarrentyEndDate='{asset.warrantyEndDate}',Description='{asset.description}',AssignTo ='{asset.assignto}', SerialNumber='{asset.serialNumber}' where ItemId = '{asset.itemId}'; Update Items Set ItemName = '{asset.itemName}' Where ItemId = '{asset.itemId}'";

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

        public List<Category> getAssetCategoriesName()
        {
            try
            {
                List<Category> category = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories Where Type = 'asset'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    category.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,

                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,


                    });
                }

                return category;

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

        public List<Asset> getAssetNames()
        {
            try
            {
                List<Asset> category = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, ItemId from Items Where ItemType = 3";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    category.Add(new Asset()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
                }

                return category;

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

        public List<Category> ViewAssetCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from Categories Where Type = 'asset'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        categoryCode = reader["CategoryCode"] != DBNull.Value ? (string)reader["CategoryCode"] : string.Empty,
                        categoryDescription = reader["CategoryDescription"] != DBNull.Value ? (string)reader["CategoryDescription"] : string.Empty,
                        isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty,
                        createdOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),


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

        public Category GetEditCategory(int categoryId)
        {
            try
            {
                Category c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName, CategoryCode, CategoryDescription, IsActive From Categories where CategoryID = '{categoryId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;
                    c.categoryCode = reader["CategoryCode"] != DBNull.Value ? (string)reader["CategoryCode"] : string.Empty;

                    c.categoryDescription = reader["CategoryDescription"] != DBNull.Value ? (string)reader["CategoryDescription"] : string.Empty;
                    c.isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty;

                    c.categoryId = reader["CategoryID"] != DBNull.Value ? (Int32)reader["CategoryID"] : 0;


                }


                return c;

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



        public bool UpdateCategory(Category category)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Categories set  CategoryName ='{category.categoryName}', CategoryCode ='{category.categoryCode}', CategoryDescription='{category.categoryDescription}' ,IsActive='{category.isActive}'  where CategoryID = '{category.categoryId}'";

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

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  Categories  where CategoryID = {categoryId}";

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


        public List<Asset> AssetListing()
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
        a.SerialNumber,
        a.AssetStatus,
        a.AssignTo,
    
        i.ItemName,
        c.CategoryName
    FROM Asset a
    LEFT JOIN Items i ON a.ItemId = i.ItemId
    LEFT JOIN Categories c ON i.CategoryId = c.CategoryId
   
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
                        assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty,
                        serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        status = reader["AssetStatus"] != DBNull.Value ? (string)reader["AssetStatus"] : string.Empty,


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




        public Asset GetAssetAssign(Guid assetId)
        {
            try
            {
                Asset asset = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select AssetId, AssignTo, SerialNumber, AssetStatus from Asset where AssetId = '{assetId}'";


                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    asset.assetId = reader["AssetId"] != DBNull.Value ? (Guid)reader["AssetId"] : Guid.Empty;

                    asset.assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty;
                    asset.serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty;

                    asset.status = reader["AssetStatus"] != DBNull.Value ? (string)reader["AssetStatus"] : string.Empty;


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

        public bool AddAssetAssignment(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into AssetAssignment([AssetId],[AssignTo],[ExpectedReturn],[Purpose],[AssignStatus])values('{asset.assetId}','{asset.assignto}','{asset.expectedReturn}','{asset.description}','{asset.status}')";

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
        public List<Asset> ViewAssignAsset()
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
       
        a.SerialNumber,
       
        a.AssignTo,
        aa.AssignedOn,
        aa.ExpectedReturn,
        i.ItemName,
        c.CategoryName
    FROM Asset a
    LEFT JOIN Items i ON a.ItemId = i.ItemId
    LEFT JOIN Categories c ON i.CategoryId = c.CategoryId
    LEFT JOIN AssetAssignment aa ON a.AssetId = aa.AssetID
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Asset()
                    {

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty,
                        serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        assignDate = reader["AssignedOn"] != DBNull.Value ? ((DateTime)reader["AssignedOn"]).Date : default(DateTime),
                        expectedReturn = reader["ExpectedReturn"] != DBNull.Value ? ((DateTime)reader["ExpectedReturn"]).Date : default(DateTime),


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


        public bool AddAssetMaintenance(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into AssetMaintenance([itemId],[Technician],[Cost],[IssueDescription],[NextMaintenance])values('{asset.itemId}','{asset.vendorName}','{asset.cost}','{asset.description}','{asset.expectedReturn}')";

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

        public List<Asset> AssetMaintenanceView()
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
   
    am.Technician,
    am.MaintenanceId,
    am.IssueDescription,
    am.Cost,
    am.MaintenanceDate,
    am.NextMaintenance,
    i.ItemName
       FROM AssetMaintenance am
LEFT JOIN Items i ON am.itemId = i.ItemId
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Asset()
                    {
                        maintenanceId = reader["MaintenanceId"] != DBNull.Value ? (Guid)reader["MaintenanceId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        vendorName = reader["Technician"] != DBNull.Value ? (string)reader["Technician"] : string.Empty,
                        description = reader["IssueDescription"] != DBNull.Value ? (string)reader["IssueDescription"] : string.Empty,
                        cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0m,
                        assignDate = reader["MaintenanceDate"] != DBNull.Value ? ((DateTime)reader["MaintenanceDate"]).Date : default(DateTime),
                        expectedReturn = reader["NextMaintenance"] != DBNull.Value ? ((DateTime)reader["NextMaintenance"]).Date : default(DateTime),


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

        public Asset EditMaintenance(Guid maintenanceId)
        {
            try
            {
                Asset asset = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT am.Technician,am.MaintenanceId,am.IssueDescription,am.Cost,am.MaintenanceDate,am.NextMaintenance,i.ItemName FROM AssetMaintenance am LEFT JOIN Items i ON am.itemId = i.ItemId Where am.MaintenanceId = {asset.maintenanceId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    asset.maintenanceId = reader["MaintenanceId"] != DBNull.Value ? (Guid)reader["MaintenanceId"] : Guid.Empty;
                    asset.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    asset.vendorName = reader["Technician"] != DBNull.Value ? (string)reader["Technician"] : string.Empty;
                    asset.description = reader["IssueDescription"] != DBNull.Value ? (string)reader["IssueDescription"] : string.Empty;
                    asset.cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0m;
                    asset.assignDate = reader["MaintenanceDate"] != DBNull.Value ? ((DateTime)reader["MaintenanceDate"]).Date : default(DateTime);
                    asset.expectedReturn = reader["NextMaintenance"] != DBNull.Value ? ((DateTime)reader["NextMaintenance"]).Date : default(DateTime);


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

        public List<Asset> AssetDepreciation()
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
        a.SerialNumber,
        a.AssetStatus,
        a.AssignTo,
    
        i.ItemName,
        c.CategoryName
    FROM Asset a
    LEFT JOIN Items i ON a.ItemId = i.ItemId
    LEFT JOIN Categories c ON i.CategoryId = c.CategoryId
   
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
                        assignto = reader["AssignTo"] != DBNull.Value ? (string)reader["AssignTo"] : string.Empty,
                        serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        status = reader["AssetStatus"] != DBNull.Value ? (string)reader["AssetStatus"] : string.Empty,
                        assignDate = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

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




        public List<Asset> DepreciationTracking()
        {
            List<Asset> asset = new();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GetDepreciation", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                asset.Add(new Asset()
                                {
                                    assetId = reader["AssetId"] != DBNull.Value ? (Guid)reader["AssetId"] : Guid.Empty,
                                    serialNumber = reader["SerialNumber"] != DBNull.Value ? (string)reader["SerialNumber"] : string.Empty,
                                    itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                                    currentValue = reader["CurrentValue"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentValue"]) : 0,
                                    price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                                    depreciation = reader["Depreciation"] != DBNull.Value ? Convert.ToInt32(reader["Depreciation"]) : 0,

                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return asset;
        }

        public List<Asset> getAssetName(int categoryId)
        {
            try
            {
                List<Asset> asset = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName, ItemId from Items Where ItemType = 3 AND  CategoryId = {categoryId}";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    asset.Add(new Asset()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,


                    });
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

        public bool AddAssetTransfer(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into AssetTransfer([itemId],[CurrentUser],[NewUser],[Reason],[TransferType])values('{asset.itemId}','{asset.currentOwner}','{asset.newOwner}','{asset.description}','{asset.type}')";

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

        public bool AllotAssetToNewUser(Asset asset)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Asset set AssignTo ='{asset.newOwner}' Where ItemId = '{asset.itemId}'";

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


    
