using System;
using System.Collections.Generic;
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


        public InventoryServices(IConfiguration config)
        {
            this.configuration = config;
        }



        public bool AddCategory(Category category)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO Categories([CategoryName],[CategoryCode],[CategoryDescription],[IsActive],[CreatedOn],[Type])  VALUES('{category.categoryName}','{category.categoryCode}','{category.categoryDescription}','{category.isActive}','{DateTime.Now.ToShortDateString()}','{category.formType}')";


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


        public List<Category> ViewProductCategory()
        {
            try
            {
                List<Category> cat = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select * from Categories Where Type = 'product'";
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
                    string query = $"INSERT INTO SubCategories([SubCategoryName],[SubCategoryCode],[SubCategoryDescription],[IsActive],[CreatedOn],[CategoryID])  VALUES('{category.subCategoryName}','{category.categoryCode}','{category.categoryDescription}','{category.isActive}','{DateTime.Now.ToShortDateString()}',{category.categoryId})";


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
                cmd.CommandText = $"SELECT sc.CategoryID,sc.SubCategoryID,sc.SubCategoryName,sc.SubCategoryCode, sc.SubCategoryDescription, sc.IsActive, c.CategoryName FROM  SubCategories sc JOIN  Categories c ON sc.CategoryID = c.CategoryID Where c.Type = 'product'";
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


       

            public bool AddWarehouses(Warehouse wh)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = $"Insert into Address ([AddressLine1],[AddressLine2],[Country],[State] ,[City],[Area],[Pincode],[CreatedOn]) OUTPUT Inserted.AddressID values(@address1,@address2,@country,@state,@district,@area,@postalcode,{DateTime.Now})";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@address1", wh.Address1);
                cmd.Parameters.AddWithValue("@address2", wh.Address2);
                cmd.Parameters.AddWithValue("@country", wh.Country);
                cmd.Parameters.AddWithValue("@state", wh.State);
                cmd.Parameters.AddWithValue("@district", wh.District);
                cmd.Parameters.AddWithValue("@area", wh.Area);
                cmd.Parameters.AddWithValue("@postalcode", wh.PostalCode);
                connection.Open();
                int addressId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"Insert into Warehouses([WarehouseType],[WarehouseName],[AddressId]) values(@warehouseType,@warehouseName,@addressId)";
                cmd2.Connection = connection;
                cmd2.Parameters.AddWithValue("@warehouseType", wh.warehouseType);
                cmd2.Parameters.AddWithValue("@warehouseName", wh.warehouseName);
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
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.AddressID,ad.Pincode,ad.CreatedOn,wh.WarehouseType,wh.WarehouseName FROM  Warehouses wh JOIN  Address ad ON wh.AddressID = ad.AddressID";
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
                        warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty,
                        warehouseType = reader["WarehouseType"] != DBNull.Value ? (string)reader["WarehouseType"] : string.Empty,
                        Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty,
                        Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty,
                        State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty,
                        District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty,
                        Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty,
                        CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime),

                        PostalCode = reader["Pincode"] != DBNull.Value ? Convert.ToInt64(reader["Pincode"]) : 0,

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
                cmd.CommandText = $"SELECT ad.AddressLine1,ad.AddressLine2,ad.State,ad.City,ad.Area,ad.Pincode,ad.AddressID,ad.CreatedOn,wh.WarehouseType,wh.WarehouseName FROM  Warehouses wh JOIN Address ad ON wh.AddressID = ad.AddressID Where ad.AddressID = {addressId}";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    c.addressId = reader["AddressID"] != DBNull.Value ? (int)reader["AddressID"] : 0;
                    //PostalCode = reader["Pincode"] != DBNull.Value ? ()reader["Pincode"] : 0,
                    c.warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty;
                    c.warehouseType = reader["WarehouseType"] != DBNull.Value ? (string)reader["WarehouseType"] : string.Empty;
                    c.Address1 = reader["AddressLine1"] != DBNull.Value ? (string)reader["AddressLine1"] : string.Empty;
                    c.Address2 = reader["AddressLine2"] != DBNull.Value ? (string)reader["AddressLine2"] : string.Empty;
                    c.State = reader["State"] != DBNull.Value ? (string)reader["State"] : string.Empty;
                    c.District = reader["City"] != DBNull.Value ? (string)reader["City"] : string.Empty;
                    c.Area = reader["Area"] != DBNull.Value ? (string)reader["Area"] : string.Empty;
                    c.CreatedOn = reader["CreatedOn"] != DBNull.Value ? ((DateTime)reader["CreatedOn"]).Date : default(DateTime);

                    c.PostalCode = reader["Pincode"] != DBNull.Value ? Convert.ToInt64(reader["Pincode"]) : 0;





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
                cmd.CommandText = $"Select WarehouseName, warehouseId from Warehouses";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Warehouse()
                    {
                        warehouseId = reader["warehouseID"] != DBNull.Value ? (Guid)reader["warehouseID"] : Guid.Empty,


                        warehouseName = reader["WarehouseName"] != DBNull.Value ? (string)reader["WarehouseName"] : string.Empty,


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

                string query1 = $"UPDATE Warehouses SET WarehouseName = '{wh.warehouseName}', WarehouseType = '{wh.warehouseType}' WHERE AddressID = '{wh.addressId}'";

               
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
                            
                            SqlCommand cmd1 = new SqlCommand("DELETE FROM Warehouses WHERE AddressID = @addressId", connection, transaction);
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
                cmd.CommandText = $"Select CategoryName, CategoryID from Categories Where Type = 'product'";
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

                cmd.CommandText = $"Insert into Lot ([LotSeries],[BatchSeries],[ManufacturingDate],[ExpiryDate] ,[DeliverdDate],[Type]) OUTPUT Inserted.LotId values('{item.lotSeries}','{item.batchSeries}','{item.manufacture}','{ item.expiry}','{item.deliverd}','{item.type}')";
                cmd.Connection = connection;
              
                connection.Open();
                Guid lotId = (Guid)cmd.ExecuteScalar();
                connection.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandType = System.Data.CommandType.Text;

                cmd2.CommandText = $"Insert into Items([ItemName],[SKU],[HSN],[CategoryId],[SubCategory],[Specification],[UnitOfMeasure],[GstRate],[ItemType]) OUTPUT Inserted.ItemId values('{item.itemName}','{item.SKU}','{item.HSN}','{item.categoryId}','{item.subCategory}','{item.specification}','{item.UOM}','{item.gst}','{item.itemType}')";
                cmd2.Connection = connection;

                cmd2.Parameters.AddWithValue("@lotId", lotId);

                connection.Open();

                Guid ItemId = (Guid)cmd2.ExecuteScalar();
                connection.Close();

                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = System.Data.CommandType.Text;
                cmd3.Connection = connection;

                cmd3.CommandText = $"INSERT INTO Inventory([ItemId],[WarehouseId],[InStock],[StockAlert],[LotId],[ItemType]) VALUES(@ItemId,'{item.warehouseId}','{item.inStock}','{item.stockAlert}',@LotId,'{item.itemType}')";


                cmd3.Parameters.AddWithValue("@ItemId", ItemId);
                cmd3.Parameters.AddWithValue("@lotId", lotId);

                connection.Open();
                cmd3.ExecuteNonQuery();
                connection.Close();

                SqlCommand cmd4 = new SqlCommand();
                cmd4.CommandType = System.Data.CommandType.Text;
                cmd4.Connection = connection;

                cmd4.CommandText = $"INSERT INTO Price([ItemId],[CostPrice],[MRP],[SellingPrice],[UnitPrice]) VALUES (@ItemId,'{item.costPrice}','{item.mrp}','{item.sellingPrice}','{item.unitPrice}')";

                cmd4.Parameters.AddWithValue("@ItemId", ItemId);
              

                connection.Open();
                cmd4.ExecuteNonQuery();
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
                cmd.CommandText = $" SELECT it.ItemId,it.ItemName,it.SKU,it.HSN,it.Category,it.SubCategory,it.Specification,it.UnitOfMeasure,it.GstRate,iv.InventoryId,iv.InStock,iv.StockAlert,l.LotId,l.LotSeries,l.ManufacturingDate,l.ExpiryDate,l.DeliverdDate,p.PriceId,p.UnitPrice FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN Lot l ON iv.LotId = l.LotId LEFT JOIN Price p ON it.ItemId= p.ItemId Where it.ItemType = 'material'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["LotId"] != DBNull.Value ? (Guid)reader["LotId"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                        priceId = reader["PriceId"] != DBNull.Value ? (Guid)reader["PriceId"] : Guid.Empty,
                        SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        HSN = reader["HSN"] != DBNull.Value ? (int)reader["HSN"] : 0,
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,

                     
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        lotSeries = reader["LotSeries"] != DBNull.Value ? (string)reader["LotSeries"] : string.Empty,
                        category = reader["Category"] != DBNull.Value ? (string)reader["Category"] : string.Empty,
                        subCategory = reader["SubCategory"] != DBNull.Value ? (string)reader["SubCategory"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,
                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        manufacture = reader["ManufacturingDate"] != DBNull.Value ? ((DateTime)reader["ManufacturingDate"]).Date : default(DateTime),
                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        deliverd = reader["DeliverdDate"] != DBNull.Value ? ((DateTime)reader["DeliverdDate"]).Date : default(DateTime),



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
                cmd.CommandText = $" SELECT it.ItemId,it.ItemName,it.SKU,it.HSN,it.Category,it.SubCategory,it.Specification,it.UnitOfMeasure,it.GstRate,iv.InventoryId,iv.InStock,iv.StockAlert,l.LotId,l.BatchSeries,l.ManufacturingDate,l.ExpiryDate,l.DeliverdDate,p.PriceId,p.UnitPrice,p.CostPrice,p.SellingPrice,p.MRP FROM Items it JOIN Inventory iv ON it.ItemId = iv.ItemId LEFT JOIN Lot l ON iv.LotId = l.LotId LEFT JOIN Price p ON it.ItemId= p.ItemId Where it.ItemType = 'product'";
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat.Add(new Items()
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        lotId = reader["LotId"] != DBNull.Value ? (Guid)reader["LotId"] : Guid.Empty,
                        inventoryId = reader["InventoryId"] != DBNull.Value ? (Guid)reader["InventoryId"] : Guid.Empty,
                        priceId = reader["PriceId"] != DBNull.Value ? (Guid)reader["PriceId"] : Guid.Empty,
                       

                        SKU = reader["SKU"] != DBNull.Value ? (int)reader["SKU"] : 0,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        HSN = reader["HSN"] != DBNull.Value ? (int)reader["HSN"] : 0,
                        gst = reader["GstRate"] != DBNull.Value ? Convert.ToInt32(reader["GstRate"]) : 0,


                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        batchSeries = reader["BatchSeries"] != DBNull.Value ? (string)reader["BatchSeries"] : string.Empty,
                        category = reader["Category"] != DBNull.Value ? (string)reader["Category"] : string.Empty,
                        subCategory = reader["SubCategory"] != DBNull.Value ? (string)reader["SubCategory"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        UOM = reader["UnitOfMeasure"] != DBNull.Value ? (string)reader["UnitOfMeasure"] : string.Empty,
                        unitPrice = reader["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(reader["UnitPrice"]) : 0m,
                        costPrice = reader["CostPrice"] != DBNull.Value ? Convert.ToDecimal(reader["CostPrice"]) : 0m,
                        sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0m,
                        mrp = reader["MRP"] != DBNull.Value ? Convert.ToDecimal(reader["MRP"]) : 0m,
                        manufacture = reader["ManufacturingDate"] != DBNull.Value ? ((DateTime)reader["ManufacturingDate"]).Date : default(DateTime),
                        expiry = reader["ExpiryDate"] != DBNull.Value ? ((DateTime)reader["ExpiryDate"]).Date : default(DateTime),
                        deliverd = reader["DeliverdDate"] != DBNull.Value ? ((DateTime)reader["DeliverdDate"]).Date : default(DateTime),



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

    }

}

