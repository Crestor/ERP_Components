using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class ProductionServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;


        public ProductionServices(IConfiguration config)
        {
            this.configuration = config;
        }



        public List<Production> GetProductionOrders()
        {
            try
            {
                List<Production> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select po.ProductID, it.ItemName,po.ProductionOrderID,po.ProductionStatus, po.Quantity, po.CreatedAt, po.SalesForcastID From ProductionOrder po JOIN Items it ON it.itemId = po.ProductID WHERE po.ProductionStatus != 5 order by po.CreatedAt desc";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Production
                    {
                        productionOrderId = reader["ProductionOrderID"] != DBNull.Value ? (Guid)reader["ProductionOrderID"] : Guid.Empty,
                        productId = reader["ProductID"] != DBNull.Value ? (Guid)reader["ProductID"] : Guid.Empty,
                        productStatus = reader["ProductionStatus"] != DBNull.Value ? Convert.ToInt32(reader["ProductionStatus"]) : 0,
                        salesForcastId = reader["SalesForcastID"] != DBNull.Value ? (Guid)reader["SalesForcastID"] : Guid.Empty,
                        productName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,
                        createdAt = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime),



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

        public bool UpdateProductionInventory(Guid materialId, decimal quantity)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"UPDATE Inventory SET InStock = InStock - {Convert.ToInt32(quantity)} WHERE ItemId = '{materialId}' AND InventoryCenter = 3";

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

        public Production GetProductDetail(Guid productionOrderId)
        {
            try
            {
                Production product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select it.ItemName, po.Quantity,po.ProductID, po.CreatedAt,po.ProductionOrderID,po.SalesForcastID from ProductionOrder po Join Items it ON po.ProductID = it.ItemId Where po.ProductionOrderID =  '{productionOrderId}'";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    product.productId = reader["ProductID"] != DBNull.Value ? (Guid)reader["ProductID"] : Guid.Empty;
                    product.productionOrderId = reader["ProductionOrderID"] != DBNull.Value ? (Guid)reader["ProductionOrderID"] : Guid.Empty;
                    product.salesForcastId = reader["SalesForcastID"] != DBNull.Value ? (Guid)reader["SalesForcastID"] : Guid.Empty;

                    product.productName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty;
                    product.quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0;
                    product.createdAt = reader["CreatedAt"] != DBNull.Value ? ((DateTime)reader["CreatedAt"]).Date : default(DateTime);

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
        public List<Production> GetProductDetailforbill()
        {
            try
            {
                List<Production> product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemID from Items Where ItemType=1 ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new Production
                    {
                        productName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        productId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

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
        public List<Production> GetMaterialDetailforbill()
        {
            try
            {
                List<Production> product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select ItemName,ItemID from Items Where ItemType=2 ";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new Production
                    {
                        materialName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        materialId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

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
        public List<Items> ViewInventoryOfProduction()
        {
            try
            {
                List<Items> product = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select it.ItemName,iv.InStock,iv.StockAlert,iv.LastUpdated,it.UnitOFMeasure,it.Specification from Inventory iv Join Items it On iv.ItemId = it.ItemId where InventoryCenter = 3\r\n";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Add(new Items
                    {
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        inStock = reader["InStock"] != DBNull.Value ? (int)reader["InStock"] : 0,
                        stockAlert = reader["StockAlert"] != DBNull.Value ? (int)reader["StockAlert"] : 0,
                        UOM = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        arrival = reader["LastUpdated"] != DBNull.Value ? ((DateTime)reader["LastUpdated"]).Date : default(DateTime),

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

        //public Production GetProductionStages(Guid ProductId)
        //{
        //    try
        //    {
        //        Production product = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"Select Stage, StageWork,StageTime Where ProductID = '{ProductId}'";

        //        cmd.Connection = connection;

        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {

        //            product.stageWork = reader["StageWork"] != DBNull.Value ? (string)reader["StageWork"] : string.Empty;
        //            product.stageTime = reader["StageTime"] != DBNull.Value ? (int)reader["StageTime"] : 0;
        //            product.stage = reader["Stage"] != DBNull.Value ? (int)reader["Stage"] : 0;


        //        }


        //        return product;

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



        public List<Production> GetProductionStages(Guid productionOrderId, Guid productId)
        {
            try
            {
                List<Production> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT pss.ProductID, pss.Stage, pss.StageTime, pss.StageWork, ISNULL(( SELECT ps.StatusName FROM ProductionProcess pp   JOIN ProductionStatuses ps ON pp.ProcessStatus = ps.ProductionStatus  WHERE pp.ProductionOrderID = '{productionOrderId}'   AND pp.ProductID = '{productId}' AND pp.Stage = pss.Stage ), 'PENDING') AS Status FROM ProductionStages pss WHERE pss.ProductID = '{productId}' ORDER BY pss.Stage;";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Production
                    {
                        productId = reader["ProductID"] != DBNull.Value ? (Guid)reader["ProductID"] : Guid.Empty,
                        stageWork = reader["StageWork"] != DBNull.Value ? (string)reader["StageWork"] : string.Empty,
                        status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : string.Empty,
                        //productStatus = reader["Status"] != DBNull.Value ? (Int32)reader["Status"] : 0,
                        stageTime = reader["StageTime"] != DBNull.Value ? (int)reader["StageTime"] : 0,
                        //stage = reader["Stage"] != DBNull.Value ? (Int32)reader["Stage"] : 0,
                        stage = reader["Stage"] != DBNull.Value ? (byte)reader["Stage"] : (byte)0,



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

        //public List<Production> GetMaterialRequisitionItems(Guid productionOrderId)
        //{
        //    try
        //    {
        //        List<Production> prod = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"SELECT it.ItemName AS Material, CASE WHEN pmm.Quantity * po.Quantity - i.InStock < 0 THEN 0 ELSE pmm.Quantity * po.Quantity - i.InStock END AS RequiredQuantity FROM ProductMaterialMapping pmm JOIN ProductionOrder po ON pmm.ProductID = po.ProductID JOIN Inventory i ON i.ItemID = pmm.MaterialID JOIN Items it ON it.ItemId = pmm.MaterialID WHERE po.ProductionOrderID = '{productionOrderId}'";


        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            prod.Add(new Production
        //            {


        //                materialName = reader["Material"] != DBNull.Value ? (string)reader["Material"] : string.Empty,
        //                quantityRequired = reader["RequiredQuantity"] != DBNull.Value ? Convert.ToDecimal(reader["RequiredQuantity"]) : 0m,
                      

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




        public List<Production> GetMaterialRequisitionItems(Guid productionOrderId)
        {
            try
            {
                List<Production> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT it.ItemId, it.ItemName AS Material, CASE WHEN pmm.Quantity * po.Quantity - i.InStock < 0 THEN 0 ELSE pmm.Quantity * po.Quantity - i.InStock END AS RequiredQuantity FROM ProductMaterialMapping pmm JOIN ProductionOrder po ON pmm.ProductID = po.ProductID JOIN Inventory i ON i.ItemID = pmm.MaterialID JOIN Items it ON it.ItemId = pmm.MaterialID WHERE po.ProductionOrderID = '{productionOrderId}' AND i.InventoryCenter = 3 GROUP BY it.ItemId, it.ItemName,pmm.Quantity * po.Quantity - i.InStock";
                
                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Production
                    {
                        materialId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,

                        materialName = reader["Material"] != DBNull.Value ? (string)reader["Material"] : string.Empty,
                        quantityRequired = reader["RequiredQuantity"] != DBNull.Value ? Convert.ToDecimal(reader["RequiredQuantity"]) : 0m,

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




        public bool SendToProduction(Production production)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO ProductionProcess([ProductionOrderID],[ProductID],[Stage],[ProcessStatus])  VALUES('{production.productionOrderId}','{production.productId}',1,2)";


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
        public bool SaveBilOlfMaterial(Production production)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = $"INSERT INTO ProductMaterialMapping (ProductID, MaterialID, Quantity) VALUES ('{production.productId}', '{production.materialId}', '{production.quantity}');";


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





        public bool UpdateProductionOrderStatusToInProgress(Production production)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Update ProductionOrder Set  ProductionStatus = 2 where ProductionOrderID = '{production.productionOrderId}'";



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


        public List<Production> GetListOfMaterials(Guid productionOrderId)
        {
            try
            {
                List<Production> material = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = cmd.CommandText = $"SELECT m.ItemID AS MaterialID, m.ItemName AS Material, (pmm.Quantity * po.Quantity) AS RequiredQuantity, m.UnitOFMeasure, i.InStock AS AvailableQuantity FROM ProductionOrder po JOIN ProductMaterialMapping pmm ON po.ProductID = pmm.ProductID JOIN Items m ON pmm.MaterialID = m.ItemId JOIN Inventory i ON pmm.MaterialID = i.ItemId WHERE po.ProductionOrderID = '{productionOrderId}' AND i.InventoryCenter = 3";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    material.Add(new Production()
                    {
                        materialName = reader["Material"] != DBNull.Value ? (string)reader["Material"] : string.Empty,
                        unitOfMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        quantityRequired = reader["RequiredQuantity"] != DBNull.Value ? Convert.ToDecimal(reader["RequiredQuantity"]) : 0m,
                        availableQuantity = reader["AvailableQuantity"] != DBNull.Value ? (int)reader["AvailableQuantity"] : 0,
                        materialId = reader["MaterialID"] != DBNull.Value ? (Guid)reader["MaterialID"] : Guid.Empty

                    });
                }

                return material;

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

        public List<Production> GetMaterialNames()
        {
            try
            {
                List<Production> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select  = 2";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Production
                    {
                        productId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        productName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

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
            //<--------------------Material Requistions--------------------->


        //    public List<Production> GetMaterialNames()
        //{
        //    try
        //    {
        //        List<Production> prod = new();
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
        //        connection = new SqlConnection(connectionstring);
        //        SqlCommand cmd = new();
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.CommandText = $"select ItemId, ItemName From Items Where ItemType = 2";
        //        cmd.Connection = connection;


        //        cmd.CommandTimeout = 300;
        //        connection.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            prod.Add(new Production
        //            {
        //                productId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
        //                productName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,

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


        public Guid AddMaterialRequisitions(Production production)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;
                Guid RequisitionID = Guid.NewGuid();
                cmd1.CommandText = $"INSERT INTO Requisitions (RequisitionSeries, RequisitionID, [Description],[RequisitionType], [RequisitionStatus]) VALUES ('{production.RequisitionSeries}','{RequisitionID}', '{production.description}', 2, 1);" +
                    $" INSERT INTO ProductionRequisitionMapping(ProductionOrderID, RequisitionID) VALUES ('{production.productionOrderId}','{RequisitionID}');" +
                    $" UPDATE ProductionOrder SET ProductionStatus = 5 WHERE ProductionOrderID = '{production.productionOrderId}';";


                //cmd1.CommandText = $"INSERT INTO Requisitions ([Description],[RequisitionType], [RequisitionStatus],[RequisitionSeries]) OUTPUT INSERTED.RequisitionID VALUES ('{production.description}', 2, 1,'{production.RequisitionSeries}')";




                cmd1.Connection = connection;
                connection.Open();
                cmd1.ExecuteScalar();
                //Guid RequisitionID = (Guid)cmd1.ExecuteScalar();

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


        public bool AddMaterialRequisitionItems(Production product)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.CommandText = "INSERT INTO RequisitionItems ([ItemID],[Quantity],[RequisitionID]) VALUES (@ItemID,@Quantity,@RequisitionID)";

                cmd.Parameters.AddWithValue("@ItemID", product.materialId);
                cmd.Parameters.AddWithValue("@RequisitionID", product.RequisitionID);
       
                cmd.Parameters.AddWithValue("@Quantity", product.quantityRequired);

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

        public bool updateSFDetails(QuotationModel Aq)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);



                SqlCommand cmd2 = new SqlCommand(@"
    UPDATE Requisitions 
    SET 
        Description = @Description, 
        RequisitionSeries = @RequisitionSeries, 
        RequisitionType = 1,
RequisitionStatus=1
    WHERE RequisitionID = @RequisitionID", connection);

                cmd2.Parameters.AddWithValue("@RequisitionSeries", Aq.RequisitionSeries ?? (object)DBNull.Value);
                cmd2.Parameters.AddWithValue("@Description", Aq.Description);
                cmd2.Parameters.AddWithValue("@RequisitionID", Aq.RequisitionID);
            
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


        public List<QuotationModel> OrderTable(Guid RequisitionID)
        {
            try
            {
                List<QuotationModel> OL = new();
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select I.ItemName,R.Quantity from Items I  join RequisitionItems R  ON R.ItemId=I.ItemId where RequisitionID= '{RequisitionID}'";
             

                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OL.Add(new QuotationModel()
                    {
                        ItemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        Quantity = reader["Quantity"] != DBNull.Value ? (int)reader["Quantity"] : 0,


                    });
                }

                return OL;

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


        public List<Production> GoToNextStage(Guid productionOrderId, Guid productId)
        {
            var materials = new List<Production>();
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("ProceedToNextStage", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductionOrderID", productionOrderId);
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materials.Add(new Production
                                {
                                    stage = reader.GetByte(reader.GetOrdinal("Stage")),
                                    stageWork = reader.GetString(reader.GetOrdinal("StageWork")),
                                    stageTime = reader.GetInt32(reader.GetOrdinal("StageTime")),
                                    status = reader.GetString(reader.GetOrdinal("Status"))
                                });
                            }
                        }
                    }
                }

                return materials;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }



        public bool UpdateInventoryDetails(Guid productId, Guid productionOrderId)
        {
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"UPDATE Inventory SET InStock = InStock + (SELECT Quantity FROM ProductionOrder WHERE ProductID = '{productId}' AND ProductionOrderID = '{productionOrderId}') WHERE ItemId = '{productId}' AND InventoryCenter = 2; UPDATE ProductionOrder SET ProductionStatus = 4 WHERE ProductionOrderID = '{productionOrderId}'";



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



        public List<Items> ViewProductionItems()
        {
            try
            {
                List<Items> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select it.ItemId, it.ItemName, it.Specification, ct.CategoryName From Items it JOIN Categories ct ON it.CategoryId = ct.CategoryId  Where  it.ItemType = 1";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Items
                    {
                        itemId = reader["ItemId"] != DBNull.Value ? (Guid)reader["ItemId"] : Guid.Empty,
                        itemName = reader["ItemName"] != DBNull.Value ? (string)reader["ItemName"] : string.Empty,
                        category = reader["CategoryName"] != DBNull.Value ? (string)reader["CategoryName"] : string.Empty,
                        specification = reader["Specification"] != DBNull.Value ? (string)reader["Specification"] : string.Empty,
                        //HSN = reader["HSN"] != DBNull.Value ? (string)reader["HSN"] : string.Empty,


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



        public List<Production> ViewProductionItemsStages(Guid productId)
        {
            try
            {
                List<Production> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select StageWork,  StageTime from ProductionStages where ProductID = '{productId}'";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Production
                    {
                  
                        stageWork = reader["StageWork"] != DBNull.Value ? (string)reader["StageWork"] : string.Empty,

                        stageTime = reader["StageTime"] != DBNull.Value ? (int)reader["StageTime"] : 0,

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


        public List<Items> ViewProductionMaterialsList(Guid productId)
        {
            try
            {
                List<Items> prod = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" SELECT m.ItemID AS MaterialID, m.ItemName AS Material, m.UnitOFMeasure FROM ProductMaterialMapping pmm  JOIN ProductionOrder po ON pmm.ProductID =  po.ProductID Left JOIN Items m ON pmm.MaterialID = m.ItemId Where pmm.ProductID ='{productId}'";
                cmd.Connection = connection;


                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prod.Add(new Items
                    {
                        itemId = reader["MaterialID"] != DBNull.Value ? (Guid)reader["MaterialID"] : Guid.Empty,

                        itemName = reader["Material"] != DBNull.Value ? (string)reader["Material"] : string.Empty,
                        UOM = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,

                       

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

        public List<Production> ViewProductionListOfMaterials(Guid productionOrderId)
        {
            try
            {
                List<Production> material = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                 cmd.CommandText = $"SELECT m.ItemID AS MaterialID, m.ItemName AS Material, (pmm.Quantity * po.Quantity) AS RequiredQuantity, m.UnitOFMeasure, i.InStock AS AvailableQuantity FROM ProductionOrder po JOIN ProductMaterialMapping pmm ON po.ProductID = pmm.ProductID JOIN Items m ON pmm.MaterialID = m.ItemId JOIN Inventory i ON pmm.MaterialID = i.ItemId WHERE po.ProductionOrderID = '{productionOrderId}' AND i.InventoryCenter = 3";

                cmd.Connection = connection;

                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    material.Add(new Production()
                    {
                        materialName = reader["Material"] != DBNull.Value ? (string)reader["Material"] : string.Empty,
                        unitOfMeasure = reader["UnitOFMeasure"] != DBNull.Value ? (string)reader["UnitOFMeasure"] : string.Empty,
                        quantityRequired = reader["RequiredQuantity"] != DBNull.Value ? Convert.ToDecimal(reader["RequiredQuantity"]) : 0m,
                        availableQuantity = reader["AvailableQuantity"] != DBNull.Value ? (int)reader["AvailableQuantity"] : 0,
                        materialId = reader["MaterialID"] != DBNull.Value ? (Guid)reader["MaterialID"] : Guid.Empty

                    });
                }

                return material;

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
