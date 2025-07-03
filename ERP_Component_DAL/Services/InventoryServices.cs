using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class InventoryServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;
        private string _connectionString;


        public InventoryServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = config.GetConnectionString("DefaultConnectionString");

        }



        public bool AddCategory(Category category)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO Categories([CategoryName],[CategoryCode],[CategoryDescription],[CreatedOn],[Type])  VALUES('{category.categoryName}','{category.categoryCode}','{category.categoryDescription}','{DateTime.Now.ToShortDateString()}','{category.formType}')";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        //cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                        //cmd.Parameters.AddWithValue("@categoryCode", category.categoryCode);
                        //cmd.Parameters.AddWithValue("@categoryDesc", category.categoryDescription);
                        //cmd.Parameters.AddWithValue("@isActive", category.isActive);




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


        public List<Category> ViewCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryName, CategoryId From Categories";
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
                        //categoryCode = reader["CategoryCode"] != DBNull.Value ? (string)reader["CategoryCode"] : string.Empty,
                        //categoryDescription = reader["CategoryDescription"] != DBNull.Value ? (string)reader["CategoryDescription"] : string.Empty,
                        //isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty,
                        //createdOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),


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

        public List<Category> ViewMaterialCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from Categories Where Type = 'material'";
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

        //SubCategory

        public bool AddSubCategory(Category category)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO SubCategories([SubCategoryName],[SubCategoryCode],[SubCategoryDescription],[CreatedOn],[CategoryID])  VALUES('{category.subCategoryName}','{category.categoryCode}','{category.categoryDescription}','{DateTime.Now.ToShortDateString()}',{category.categoryId})";


                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        //cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                        //cmd.Parameters.AddWithValue("@categoryCode", category.categoryCode);
                        //cmd.Parameters.AddWithValue("@categoryDesc", category.categoryDescription);
                        //cmd.Parameters.AddWithValue("@isActive", category.isActive);




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

        public List<Category> ViewSubCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId= reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        //categoryCode = reader["SubCategoryCode"] != DBNull.Value ? (string)reader["SubCategoryCode"] : string.Empty,
                        //categoryDescription = reader["SubCategoryDescription"] != DBNull.Value ? (string)reader["SubCategoryDescription"] : string.Empty,
                        //isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty


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

        public List<Category> ViewMaterialSubCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName,sc.SubCategoryCode, sc.SubCategoryDescription, sc.IsActive, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID Where c.Type = 'material'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,
                        categoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                        subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,
                        categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        categoryCode = reader["SubCategoryCode"] != DBNull.Value ? (string)reader["SubCategoryCode"] : string.Empty,
                        categoryDescription = reader["SubCategoryDescription"] != DBNull.Value ? (string)reader["SubCategoryDescription"] : string.Empty,
                        isActive = reader["IsActive"] != DBNull.Value ? (string)reader["IsActive"] : string.Empty


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

        public List<Category> CategoryProductNames()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName from Categories Where Type = 'product' ";
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

        public List<Category> CategoryMaterialNames()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select CategoryID, CategoryName from Categories Where Type = 'material' ";
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

        public Category GetEditSubCategory(int subCategoryId)
        {
            try
            {
                Category c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName,sc.SubCategoryCode, sc.SubCategoryDescription, sc.IsActive, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID Where sc.SubCategoryID = {subCategoryId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0;
                    c.categoryName = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty;
                    c.categoryCode = reader["SubCategoryCode"] != DBNull.Value ? (string)reader["SubCategoryCode"] : string.Empty;
                    c.subCategoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty;
                    c.categoryDescription = reader["SubCategoryDescription"] != DBNull.Value ? (string)reader["SubCategoryDescription"] : string.Empty;
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

        public bool UpdateSubCategory(Category category)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update SubCategories set  SubCategoryName ='{category.subCategoryName}', SubCategoryCode ='{category.categoryCode}', SubCategoryDescription='{category.categoryDescription}' ,IsActive='{category.isActive}'  where SubCategoryID = '{category.SubCategoryId}'";

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


        public bool DeleteSubCategory(int subCategoryId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  SubCategories  where SubCategoryID = {subCategoryId}";

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


       

            public bool AddWarehouses(Warehouse warehouse)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Address ([AddressLine1],[AddressLine2],[Country],[State] ,[City],[Area],[Pincode],[CreatedOn]) OUTPUT Inserted.AddressID values(@address1,@address2,@country,@state,@district,@area,@postalcode,'{DateTime.Now.ToShortDateString()}')";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@address1", warehouse.Address1);
                cmd.Parameters.AddWithValue("@address2", warehouse.Address2);
                cmd.Parameters.AddWithValue("@country", warehouse.Country);
                cmd.Parameters.AddWithValue("@state", warehouse.State);
                cmd.Parameters.AddWithValue("@district", warehouse.District);
                cmd.Parameters.AddWithValue("@area", warehouse.Area);
                cmd.Parameters.AddWithValue("@postalcode", warehouse.PostalCode);
                connection.Open();
                int addressId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"Insert into DistributionCenter([CenterType],[CenterName],[AddressId]) values(@centerType,@centerName,@addressId)";
                cmd2.Connection = connection;
                cmd2.Parameters.AddWithValue("@centerType", warehouse.type);
                cmd2.Parameters.AddWithValue("@centerName", warehouse.warehouseName);
                cmd2.Parameters.AddWithValue("@addressId", addressId);





                connection.Open();
                cmd2.ExecuteNonQuery();
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



        public List<Warehouse> ViewWarehouse()
        {
            try
            {
                List<Warehouse> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.AddressID,ad.Pincode,ad.CreatedOn,dc.CenterName FROM DistributionCenter dc JOIN  Address ad ON dc.AddressID = ad.AddressID Where dc.CenterType=1";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Warehouse()
                    {
                        addressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0,
                        //PostalCode = reader["Pincode"] != DBNull.Value ? ()reader["Pincode"] : 0,
                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,
                       
                        Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,
                        Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

                        PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty,

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

        public Warehouse GetWarehouse(int addressId)
        {
            try
            {
                Warehouse c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.Pincode,ad.AddressID,ad.CreatedOn,dc.CenterName FROM  DistributionCenter dc JOIN Address ad ON dc.AddressID = ad.AddressID Where ad.AddressID = {addressId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.addressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0;
                   
                    c.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;
                    c.PostalCode = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;
                    c.warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;
                    //c.warehouseType = reader["WarehouseType"] != DBNull.Value ? (string)reader["WarehouseType"] : string.Empty;
                    c.Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    c.Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty;
                    c.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    c.District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                    c.Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty;
                    c.CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime);

                    c.PostalCode = reader["Pincode"] != DBNull.Value ? (string)reader["Pincode"] : string.Empty;





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


        public List<Warehouse> getWarehouseName()
        {
            try
            {
                List<Warehouse> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CenterName, CenterId from DistributionCenter Where CenterType = 3";
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

        //public List<Stock> WarehouseName()
        //{
        //    try
        //    {
        //        List<Stock> cat = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Select WarehouseName from Warehouses Where WarehouseId = ";
        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            cat.Add(new Stock()
        //            {
        //                warehouseId = reader["warehouseId"] != DBNull.Value ? (Guid)reader["warehouseId"] : Guid.Empty,


        //                warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty,


        //            });
        //        }

        //        return cat;

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

        public List<Items> GetMaterialNamesFromItems()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items where ItemType = 2";
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


        public List<Items> GetProductNamesFromItems()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemId from Items where ItemType = 1";
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



        //public bool UpdateWarehouse(Warehouse wh)
        //{
        //    try
        //    {
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Update Warehouses set  WarehouseName ='{wh.warehouseName}', WarehouseType ='{wh.warehouseType}'  where AddressID = '{wh.addressId}'";

        //        cmd.Connection = connection;
        //        connection.Open();
        //        cmd.ExecuteScalar();
        //        connection.Close();
        //        return true;

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

        public bool UpdateWarehouse(Warehouse wh)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);

                string query1 = $"UPDATE DistributionCenter SET CenterName = '{wh.warehouseName}'WHERE AddressID = '{wh.addressId}'";

               
                string query2 = $"UPDATE Address SET AddressLine1 = '{wh.Address1}',AddressLine2='{wh.Address2}', State='{wh.State}',City='{wh.District}',Area='{wh.Area}' WHERE AddressID = '{wh.addressId}'";

                SqlCommand cmd = new SqlCommand(query1 + ";" + query2, connection);
                cmd.CommandType = System.Data.CommandType.Text;

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


        public bool DeleteWarehouse(int addressId)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                   
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            
                            SqlCommand cmd1 = new SqlCommand("DELETE FROM DistributionCenter WHERE AddressID = @addressId", connection, transaction);
                            cmd1.Parameters.AddWithValue("@addressId", addressId);
                            cmd1.ExecuteNonQuery();

                            
                            SqlCommand cmd2 = new SqlCommand("DELETE FROM Address WHERE AddressID = @addressId", connection, transaction);
                            cmd2.Parameters.AddWithValue("@addressId", addressId);
                            cmd2.ExecuteNonQuery();

                        
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public List<Category> getProductCategoriesName()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories";
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

        public List<Category> getMaterialCategoriesName()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories Where Type = 'material'";
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




        public List<Category> getSubCategoriesName(int categoryId)
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select SubCategoryName, SubCategoryID from SubCategories Where CategoryID = {categoryId}";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Category()
                    {
                        SubCategoryId = reader["SubCategoryID"] != DBNull.Value ? (int)reader["SubCategoryID"] : 0,

                        categoryName = reader["SubCategoryName"] != DBNull.Value ? (string)reader["SubCategoryName"] : string.Empty,


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


        public bool AddItem(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Items([ItemName],[SKU],[HSN],[CategoryId],[SubCategoryId],[Specification],[UnitOfMeasure],[GstRate],[ItemType]) OUTPUT Inserted.ItemId values('{item.itemName}','{item.SKU}','{item.HSN}','{item.categoryId}','{item.subCategoryId}','{item.specification}','{item.UOM}','{item.gst}','{item.itemType}')";
               
                cmd.Connection = connection;
              
                connection.Open();
                Guid ItemId = (Guid)cmd.ExecuteScalar();
                connection.Close();

                //SqlCommand cmd1 = new SqlCommand();
                //cmd1.CommandType = System.Data.CommandType.Text;
                ////cmd1.CommandText = $"Insert into LotBatch ([ItemId],[ArrivalDate],[Quantity],[ExpiryDate],[CostPrice],[MRP],[SellingPrice],[Type]) values(@ItemId,'{DateTime.Now.ToShortDateString()}','{item.quantity}','{item.expiry}','{item.costPrice}','{item.sellingPrice}','{item.mrp}','{item.type}')";

                //cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@ItemId", ItemId);
                ////Guid lotId = (Guid)cmd1.ExecuteScalar();

                //connection.Open();
                //cmd1.ExecuteScalar();
                //connection.Close();



                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"INSERT INTO Inventory([ItemId],[InStock],[StockAlert],[InventoryType]) VALUES(@ItemId,'{item.inStock}','{item.stockAlert}','{item.itemType}')";

                cmd2.Connection = connection;

                cmd2.Parameters.AddWithValue("@ItemId", ItemId);
                //cmd2.Parameters.AddWithValue("@lotId", lotId);

                connection.Open();
                cmd2.ExecuteScalar();
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

        public List<Items> ViewMaterial()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate,  dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId WHERE it.ItemType = 2";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                        SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0,
                        HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0,


                        //SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        //HSN = reader["HSN"] != DBNull.Value ? (int)reader["HSN"] : 0,
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,





                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,

                        //costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,
                        //sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,
                        //mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,
                        arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                        //expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateOnly)reader["ExpiryDate"]) : default(DateOnly),
                        expiry = reader["ExpiryDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ExpiryDate"]) : default,



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


        public List<Items> ViewProduct()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate, l.CostPrice, l.SellingPrice, l.MRP, dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId WHERE it.ItemType = 1 ";
;
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                        SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0,
                        HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0,


                        //SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        //HSN = reader["HSN"] != DBNull.Value ? (int)reader["HSN"] : 0,
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,
                      
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty,

                       
                     
                      
                       
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,
                       
                        costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,
                        sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,
                        mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,
                        arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                        //expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateOnly)reader["ExpiryDate"]) : default(DateOnly),
                        expiry = reader["ExpiryDate"] != DBNull.Value? DateOnly.FromDateTime((DateTime)reader["ExpiryDate"]): default,
                        //deliverd = reader["DeliverdDate"] != DBNull.Value ? ((DateTime)reader["DeliverdDate"]).Date : default(DateTime),



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



        //public bool AddStock(Stock stock)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO StockIn([ItemId],[BatchSeries],[InvoiceNo],[StockDate],[ManufacturingDate],[ExpiryDate],[WarehouseId],[UnitPrice],[TotalPrice],[QuantityPurchased])  VALUES('{stock.itemId}','{stock.batchSeries}','{stock.invoice}',{stock.stockDate},{stock.manufacture},{stock.expiry},'{stock.warehouseId}','{stock.unitPrice}','{stock.totalPrice}','{stock.quantity}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;





        //                connection.Open();
        //                 cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}
        public bool AddStock(Stock stock)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                //cmd.CommandText = $"Insert into StockTransactions([ItemId],[ItemType]) OUTPUT Inserted.ItemId values('{asset.itemName}','{asset.itemType}')";
                cmd.CommandText = $"INSERT INTO LotBatch([ItemId],[ArrivalDate], [ExpiryDate],[CostPrice],[Quantity],[Type]) OUTPUT Inserted.Id VALUES ('{stock.itemId}', {DateTime.Now.ToShortDateString()},'{stock.expiry}', '{stock.costPrice}', '{stock.quantity}','{stock.itemType}')";

                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO StockTransactions([ItemId],[TransactionDate], [TransactionType],[Quantity],[Reason]) VALUES ('{stock.itemId}', {DateTime.Now.ToShortDateString()},'{stock.type}', '{stock.quantity}','{stock.reason}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"Update  Inventory Set InStock = (Select Sum(Quantity) As Quantity From LotBatch Where ItemId = '{stock.itemId}') where ItemId = '{stock.itemId}'";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();
                return true;

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

        //public bool AddStock(Stock stock)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = @"INSERT INTO LotBatch([ItemId],[ArrivalDate], [ExpiryDate],[CostPrice],[Quantity]) VALUES
        //        (@ItemId, @ArrivalDate, @ExpiryDate, @CostPrice, @Quantity)";

        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@ItemId", stock.itemId);


        //                cmd.Parameters.AddWithValue("@ArrivalDate", DateTime.Now.ToShortDateString());
                       
        //                cmd.Parameters.AddWithValue("@ExpiryDate", stock.expiry);
                      
        //                cmd.Parameters.AddWithValue("@CostPrice", stock.costPrice);
                        
        //                cmd.Parameters.AddWithValue("@Quantity", stock.quantity);

        //                connection.Open();
        //                cmd.ExecuteNonQuery();
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw;
        //    }
        //}


        public List<Stock> ViewProductStock()
        {
            try
            {
                List<Stock> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT 
        s.ItemId,
        s.Id,
        s.CostPrice,
        s.ArrivalDate,
        s.ExpiryDate,
        st.Quantity,
        i.ItemName
        
    FROM LotBatch s
    LEFT JOIN Items i ON s.ItemId = i.ItemId
    LEFT JOIN StockTransactions st ON s.ItemId = st.ItemId Where st.Reason = 'product'
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Stock()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        stockId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                     
                       
                        //quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                       
                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                       
                        costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,
                        


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




        public List<Stock> ViewMaterialStock()
        {
            try
            {
                List<Stock> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT 
        s.ItemId,
        s.Id,
        s.CostPrice,
        s.ArrivalDate,
        s.ExpiryDate,
        st.Quantity,
        i.ItemName
        
    FROM LotBatch s
    LEFT JOIN Items i ON s.ItemId = i.ItemId
    LEFT JOIN StockTransactions st ON s.ItemId = st.ItemId Where st.Reason = 'material'
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Stock()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        stockId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,


                        //quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),

                        costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,



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





        public Stock GetStock(Guid stockId)
        {
            try
            {
                Stock c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT CostPrice,ExpiryDate,Quantity,Id from LotBatch where Id = '{stockId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.stockId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        
                        c.quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m;

                    c.expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime);
                   
                    c.costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m;
                    





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

        public bool UpdateStock(Stock stock)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update LotBatch set  ItemId ='{stock.itemId}',CostPrice='{stock.costPrice}',ExpiryDate='{stock.expiry}',Quantity='{stock.quantity}' where Id = '{stock.stockId}'";

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

        public bool DeleteStock(Guid stockId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  LotBatch  where Id = '{stockId}'";

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


        public bool AddStockTransfer(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                
                cmd.CommandText = $"INSERT INTO LotBatch([ItemId], [ArrivalDate], [Quantity], [SellingPrice])  VALUES('{order.itemId}', '{DateTime.Now.ToShortDateString()}', '{order.quantity}', '{order.sellingPrice}')";


                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO StockTransactions([ItemId],[TransactionDate], [Transaction],[Quantity]) VALUES ('{order.itemId}', {DateTime.Now.ToShortDateString()},'{order.type}', '{order.quantity}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();


                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"Update  Inventory Set InStock = (Select Sum(Quantity) As Quantity From LotBatch Where ItemId = '{order.itemId}') where ItemId = '{order.itemId}'";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
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



        //public bool AddStockTransfer(Stock order)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO LotBatch([ItemId],[ArrivalDate],[Quantity],[SellingPrice])  VALUES('{order.itemId}','{DateTime.Now.ToShortDateString()}','{order.quantity}','{order.sellingPrice}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;





        //                connection.Open();
        //                cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}


        public List<Order> ViewStockTransfer()
        {
            try
            {
                List<Order> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"
    SELECT 
        lb.ItemId,
        lb.Id,
        
        lb.ArrivalDate,
        lb.SellingPrice,
        lb.ExpiryDate,
        
        lb.Quantity,
    
       
        i.ItemName
       
    FROM LotBatch lb
    LEFT JOIN Items i ON lb.ItemId = i.ItemId 
    LEFT JOIN StockTransactions st ON lb.ItemId = st.ItemId Where st.TransactionType = 2; 
   
";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Order()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        OrderId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty,
                       
                       
                     
                        quantity = reader["Quantity"] != DBNull.Value ? Convert.ToDecimal(reader["Quantity"]) : 0m,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        
                        
                       
                        orderDate = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime),
                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        //...unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,


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
        public Order GetStockTransfer(Guid orderId)
        {
            try
            {
                Order c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT OrderId,CustomerName,TotalPrice,AvailableStock,OrderDate,UnitPrice,QuantityOrdered,BatchSeries from StockOut where orderId = '{orderId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.OrderId = reader["OrderId"] != DBNull.Value ? (Guid)reader["OrderId"] : Guid.Empty;

                    c.quantity = reader["QuantityOrdered"] != DBNull.Value ? (int)reader["QuantityOrdered"] : 0;
                    c.available = reader["AvailableStock"] != DBNull.Value ? (int)reader["AvailableStock"] : 0;
                   
                    //c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    //c.warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty;
                    c.customerName = reader["CustomerName"] != DBNull.Value ? (string)reader["CustomerName"] : string.Empty;
                    c.batchSeries = reader["BatchSeries"] != DBNull.Value ? (string)reader["BatchSeries"] : string.Empty;
                   
                    c.orderDate = reader["OrderDate"] != DBNull.Value ? ((DateTime)reader["OrderDate"]).Date : default(DateTime);
                    //c.unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m;
                    c.totalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : 0m;





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


        public Items GetMaterialData(Guid itemId)
        {
            try
            {
                Items c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate,  dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId where it.ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                        c.lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        c.inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty;
                        c.SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0;
                        c.HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0;


                        //SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        c.inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;
                        c.stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0;
                        //HSN = reader["HSN"] != DBNull.Value ? (int)reader["HSN"] : 0,
                        c.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;

                        c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                        c.warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;





                        c.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
                        c.UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty;

                     
                        c.arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime);
              
                        c.expiry = reader["ExpiryDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ExpiryDate"]) : default;




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

        public Items GetProductData(Guid itemId)
        {
            try
            {
                Items c = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName, it.SKU, it.HSN,it.GstRate, it.Specification, it.UnitOfMeasure, iv.InventoryId, iv.InStock, iv.StockAlert, l.Id, l.ArrivalDate, l.ExpiryDate, l.CostPrice, l.SellingPrice, l.MRP, dc.CenterName FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN LotBatch l ON iv.ItemId = l.ItemId  LEFT  JOIN  DistributionCenter dc ON iv.CenterId = dc.CenterId  where it.ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty;
                        c.lotId = reader["Id"] != DBNull.Value ? (Guid)reader["Id"] : Guid.Empty;
                        c.inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty;
                        c.SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0;
                        c.HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0;


                        //SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        c.inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;
                        c.stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0;
                       
                        c.gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0;
                      
                        c.itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                        c.warehouseName = reader["CenterName"] != DBNull.Value ? (string)reader["CenterName"] : string.Empty;

                       
                     
                      
                       
                        c.specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty;
                        c.UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty;
                       
                        c.costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m;
                        c.sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m;
                        c.mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m;
                        c.arrival = reader["ArrivalDate"] != DBNull.Value ? ((DateTime)reader["ArrivalDate"]).Date : default(DateTime);
                      
                        c.expiry = reader["ExpiryDate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["ExpiryDate"]) : default;
                       



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

        public bool UpdateStockTransfer(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update StockOut set  ItemId ='{order.itemId}',TotalPrice='{order.totalPrice}',OrderDate='{order.orderDate}',QuantityOrdered='{order.quantity}',AvailableStock='{order.available}',CustomerName='{order.customerName}',BatchSeries='{order.batchSeries}',WarehouseId='{order.warehouseId}' where OrderId = '{order.OrderId}'";

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


        public bool DeleteStockTransfer(Guid orderId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Delete From  StockOut  where OrderId = '{orderId}'";

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

        public bool UpdateMaterialItem(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Items set  ItemName ='{item.itemName}',SKU='{item.SKU}',HSN='{item.HSN}',Specification='{item.specification}',UnitOfMeasure='{item.UOM}',GstRate='{item.gst}' where ItemId = '{item.itemId}'";

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

        public bool UpdateInventory(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Inventory set  CenterId ='{item.warehouseId}',InStock='{item.inStock}',StockAlert='{item.stockAlert}',UpdateDescription='{item.description}' where ItemId = '{item.itemId}'";

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

        
        public bool UpdateProductPrice(Items item)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update LotBatch set CostPrice='{item.costPrice}',SellingPrice='{item.sellingPrice}',MRP='{item.mrp}' where ItemId = '{item.itemId}'";

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

        //public bool AddStockAdjustment(Order order)
        //{
        //    try
        //    {
        //        string connectionString = configuration.GetConnectionString("DefaultConnectionString");

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string query = $"INSERT INTO StockTransactions([ItemId],[SourceDC],[DestinationDC],[Quantity],[Reason],[TransactionType])  VALUES('{order.itemId}','{order.sourceDc}','{order.destinationDc}','{order.quantity}','{order.reason}','{order.type}')";


        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                cmd.CommandType = System.Data.CommandType.Text;

        //                connection.Open();
        //                cmd.ExecuteNonQuery();

        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;

        //    }
        //}


        public bool AddStockAdjustment(Order order)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                //cmd.CommandText = $"Insert into StockTransactions([ItemId],[ItemType]) OUTPUT Inserted.ItemId values('{asset.itemName}','{asset.itemType}')";
                cmd.CommandText = $"Update  Inventory Set InStock = InStock - '{order.quantity}' where ItemId = '{order.itemId}'";

                cmd.Connection = connection;

                connection.Open();
                //Guid Id = (Guid)cmd.ExecuteScalar();
                cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                cmd1.CommandText = $"INSERT INTO Inventory([ItemId],[InStock],[CenterId]) VALUES ('{order.itemId}', '{order.quantity}','{order.destinationDc}')";

                cmd1.Connection = connection;
                //cmd1.Parameters.AddWithValue("@Id", Id);


                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.CommandText = $"INSERT INTO StockTransactions([ItemId],[SourceDC],[DestinationDC],[Quantity],[Reason],[TransactionType])  VALUES('{order.itemId}','{order.sourceDc}','{order.destinationDc}','{order.quantity}','{order.reason}','{order.type}')";
                cmd2.Connection = connection;
                connection.Open();
                cmd2.ExecuteScalar();
                connection.Close();
                return true;

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


        public int GetCurrentStock(Guid itemId)
        {
            try
            {
                int currentStock = 0;
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select InStock from Inventory where ItemId = '{itemId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                   currentStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0;




                }


                return currentStock;

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

        public bool OpeningStockEntryForm(Items item)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO Inventory([ItemId],[InStock])  VALUES('{item.itemId}','{item.inStock}')";


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



        public List<Items> ViewInventoryData()
        {
            try
            {
                List<Items> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = @"SELECT  i.ItemId,i.ItemName, i.SKU,i.Specification,i.HSN, iv.InStock,iv.InventoryId FROM Items i LEFT JOIN Inventory iv ON i.ItemId = iv.ItemId ";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,

                        SKU = reader["SKU"] != DBNull.Value ? Convert.ToInt32(reader["SKU"]) : 0,
                        HSN = reader["HSN"] != DBNull.Value ? Convert.ToInt32(reader["HSN"]) : 0,

                       inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                     



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



        public List<AddPurchaseRequisition> MaterialRequisitionList()
        {
            try
            {
                List<AddPurchaseRequisition> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select Description,CreatedAt,RequisitionSeries,RequisitionID from  Requisitions where RequisitionType = 2 AND RequisitionStatus = 1 Order By CreatedAt desc ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new AddPurchaseRequisition()
                    {

                        Descripion = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        requisitionSeries = reader["RequisitionSeries"] != DBNull.Value ? (string)reader["RequisitionSeries"] : string.Empty,
                        Date = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),
                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                    });
                }

                return sun;

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


        public List<AddPurchaseRequisition> MaterialRequisitionListSeven()
        {
            try
            {
                List<AddPurchaseRequisition> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" select Description,CreatedAt,RequisitionSeries,RequisitionID from  Requisitions where RequisitionType = 2 AND RequisitionStatus = 7 Order By CreatedAt desc";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new AddPurchaseRequisition()
                    {

                        Descripion = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        requisitionSeries = reader["RequisitionSeries"] != DBNull.Value ? (string)reader["RequisitionSeries"] : string.Empty,
                        Date = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),
                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,
                    });
                }

                return sun;

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


        public List<AddPurchaseRequisition> CheckMaterial(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> sun = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT r.Description,r.RequisitionID, r.RequisitionSeries, it.ItemName, ri.Quantity AS RequiredQuantity, i.InStock AS AvailableQuantity " +
                    $"FROM RequisitionItems ri JOIN Inventory i ON ri.ItemID = i.ItemId JOIN Items it ON ri.ItemID = it.ItemId JOIN Requisitions r ON ri.RequisitionID = r.RequisitionID " +
                    $"WHERE ri.RequisitionID = @RequisitionID AND i.CenterId = '839f3550-d40c-4b27-a639-a24b5afb219c' GROUP BY r.Description,r.RequisitionID, r.RequisitionSeries, it.ItemName, ri.Quantity, i.InStock";
                cmd.Parameters.AddWithValue("@RequisitionID", RequisitionID);
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sun.Add(new AddPurchaseRequisition()
                    {
                        RequisitionId = reader["RequisitionID"] != DBNull.Value ? (Guid)reader["RequisitionID"] : Guid.Empty,

                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                       availableQuantity = reader["AvailableQuantity"] != DBNull.Value ? (int)reader["AvailableQuantity"] : 0,

                        requiredQuantity = reader["RequiredQuantity"] != DBNull.Value ? (int)reader["RequiredQuantity"] : 0,

                        Descripion = reader["Description"] != DBNull.Value ? (string)reader["Description"] : string.Empty,
                        requisitionSeries = reader["RequisitionSeries"] != DBNull.Value ? (string)reader["RequisitionSeries"] : string.Empty,
                       
                    });
                }

                return sun;

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

        public List<AddPurchaseRequisition> RequisitionListItems(Guid RequisitionID)
        {
            try
            {
                List<AddPurchaseRequisition> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT it.ItemId, it.ItemName, it.HSN, it.Specification, it.UnitOfMeasure,ri.RequisitionID, ri.UnitPrice,ri.TotalPrice,ri.Quantity, c.CategoryName, i.TypeName FROM Items it JOIN RequisitionItems ri ON it.ItemId = ri.ItemId  Left Join Categories c On it.CategoryId = c.CategoryID Left Join ItemTypes i On it.ItemType = i.ItemTypeId Where ri.RequisitionID = '{RequisitionID}'";
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
                      itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

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



        public Guid AddRequisition(AddPurchaseRequisition Add)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into PurchaseRequisitions([Description],[TotalAmount],[RequisitionSeries],[RequisitionStatus]) " + "OUTPUT INSERTED.PurchaseRequisitionID" + " values (@description,@totalAmount,@RequisitionSeries, 1)";

                cmd.Parameters.AddWithValue("@description", Add.Descripion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RequisitionSeries", Add.requisitionSeries ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@totalAmount", Add.TotalAmount);



                cmd.Connection = connection;
                connection.Open();
                Guid RequisitionID = (Guid)cmd.ExecuteScalar();


                return RequisitionID;

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


        public bool AddPurchaseRequisition(AddPurchaseRequisition requisition)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"insert into PurchaseRequisitionItems (ItemID,Quantity,UnitPrice,PurchaseRequisitionID ) values (@ItemId,@Quantity,@UnitPrice,@PurchaseRequisitionID)";
                cmd.Parameters.AddWithValue("@PurchaseRequisitionID", requisition.RequisitionId);
                cmd.Parameters.AddWithValue("@ItemId", requisition.itemId);
                cmd.Parameters.AddWithValue("@Quantity", requisition.quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", requisition.unitPrice);

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


        public List<Vendor> RecievePurchaseOrder()
        {
            try
            {
                List<Vendor> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ve.VendorID, ve.VendorName, ve.VendorCode,po.PurchaseOrderID, po.CreatedAt,po.Description,po.TaxableAmount from Vendors ve " +
                    $"join PurchaseOrders po ON ve.VendorID = po.VendorId  Where po.OrderStatus = 1 Order By po.CreatedAt desc";
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



        public bool UpdateMaterialRequisitionStatus(Guid requisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update Requisitions set  RequisitionStatus = 7 where RequisitionID = '{requisitionId}'";

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


        public bool AllocateToProductionFromStore(Guid RequisitionId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("AllocateToProductionFromStore", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RequisitionID",RequisitionId);

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


        public bool ReceiveItemsOfPurchaseOrder(Guid purchaseOrderId)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("ReceiveItemsOfPurchaseOrder", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PurchaseOrderID", purchaseOrderId);

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

        //<----------------------------------Dashboard-------------------------------------------->


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

        public List<StockTransaction> GetStockTransactions(TransactionType transactionType)
        {
            List<StockTransaction> stockTransactions = new List<StockTransaction>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT st.TransactionID, i.ItemName, st.Quantity , ddc.CenterName AS DestinationDCName, st.Reason, st.TransactionDate FROM StockTransactions st 
                                    JOIN Items i ON st.ItemId = i.ItemId
                                    JOIN DistributionCenter ddc ON st.DestinationDC = ddc.CenterId
                                    WHERE st.TransactionType = 1";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionType", (byte)transactionType);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StockTransaction transaction = new StockTransaction
                                {
                                    TransactionID = reader.GetGuid(reader.GetOrdinal("TransactionID")),
                                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                                    DestinationDC = reader.IsDBNull(reader.GetOrdinal("DestinationDCName")) ? string.Empty : reader.GetString(reader.GetOrdinal("DestinationDCName")),
                                    Reason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? null : reader.GetString(reader.GetOrdinal("Reason")),
                                    Type = transactionType
                                };
                                stockTransactions.Add(transaction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return stockTransactions;
        }
    }

}

