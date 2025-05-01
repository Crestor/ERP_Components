using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using ERP_Component_DAL.Models;


namespace ERP_Component_DAL.Services
{

    public class UserServices
    {

        private readonly IConfiguration configuration;
        SqlConnection connection;
        public UserServices(IConfiguration config)
        {
            this.configuration = config;
        }

        public List<Users> Login(Users users)
        {
            try
            {
                List<Users> user = new();

                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"select * from Users where UserName = @UserName and Password = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = users.UserName;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = users.Password;

                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.Add(new Users
                                {
                                    UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : string.Empty,
                                    Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : string.Empty,
                                });
                            }
                        }
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to login.", ex);
            }
        }

        public bool AddProduct(Product inv)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Product
                (ProductCategory, ProductCode, ProductName, ProductDescription, ProductQty, UOM, ApplicableGST, UnitPrice, TotalValue, Location)
                VALUES 
                (@ProductCategory, @ProductCode, @ProductName, @ProductDescription, @ProductQty, @UOM, @ApplicableGST, @UnitPrice, @TotalValue, @Location)";

                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@ProductCategory", inv.ProductCategory);
                    cmd.Parameters.AddWithValue("@ProductCode", inv.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", inv.ProductName);
                    cmd.Parameters.AddWithValue("@ProductDescription", string.IsNullOrEmpty(inv.ProductDescription) ? (object)DBNull.Value : inv.ProductDescription);
                    cmd.Parameters.AddWithValue("@ProductQty", inv.ProductQty);
                    cmd.Parameters.AddWithValue("@UOM", inv.UOM);
                    cmd.Parameters.AddWithValue("@ApplicableGST", inv.ApplicableGST);
                    cmd.Parameters.AddWithValue("@UnitPrice", inv.UnitPrice);

                    cmd.Parameters.AddWithValue("@TotalValue", inv.TotalValue);
                    cmd.Parameters.AddWithValue("@Location", inv.Location);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // You can log ex.Message if needed
                return false;
            }
        }


        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Product";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open(); // Make sure to open the connection before executing
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductCategory = reader["ProductCategory"].ToString(),
                                ProductCode = reader["ProductCode"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                ProductDescription = reader["ProductDescription"].ToString(),
                                ProductQty = Convert.ToInt32(reader["ProductQty"]),
                                UOM = reader["UOM"].ToString(),
                                ApplicableGST = reader["ApplicableGST"].ToString(),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                GSTValue = Convert.ToDecimal(reader["GSTValue"]),
                                TotalValue = Convert.ToDecimal(reader["TotalValue"]),
                                Location = reader["Location"].ToString()
                            });
                        }
                    }
                }
            }
            return products;
        }








        public List<Stock> ViewStock()
        {
            List<Stock> stockList = new List<Stock>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
    s.StockId,
    s.Quantity,
    s.LastUpdated,
    p.ProductName,
    p.ProductCode,
    p.UnitPrice,
    w.WarehouseName,
s.LastUpdated,
    s.ProductId,
    s.WarehouseId
FROM Stock s
JOIN Product p ON s.ProductId = p.ProductId
JOIN Warehouses w ON s.WarehouseId = w.WarehouseId";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stockList.Add(new Stock
                            {
                                StockId = Convert.ToInt32(reader["StockId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                LastUpdated = Convert.ToDateTime(reader["LastUpdated"]),

                                WarehouseId = Convert.ToInt32(reader["WarehouseId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),

                                ProductName = reader["ProductName"].ToString(),
                                ProductCode = reader["ProductCode"].ToString(),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                WarehouseName = reader["WarehouseName"].ToString()
                            });
                        }
                    }
                }
            }

            return stockList;
        }






        public List<Warehouse> GetWarehouseList()
        {
            List<Warehouse> warehouses = new List<Warehouse>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT WarehouseId, WarehouseName FROM Warehouses";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            warehouses.Add(new Warehouse
                            {
                                WarehouseId = Convert.ToInt32(reader["WarehouseId"]),
                                WarehouseName = reader["WarehouseName"].ToString()
                            });
                        }
                    }
                }
            }

            return warehouses;
        }

        public List<Product> GetProductList()
        {
            List<Product> products = new List<Product>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductId, ProductName FROM Product";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString()
                            });
                        }
                    }
                }
            }

            return products;
        }












        public bool AddStock(Stock s)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Stock (ProductId, WarehouseId, Quantity) VALUES (@ProductId, @WarehouseId, @Quantity)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", s.ProductId);
                        cmd.Parameters.AddWithValue("@WarehouseId", s.WarehouseId);
                        cmd.Parameters.AddWithValue("@Quantity", s.Quantity);

                        connection.Open();
                        cmd.ExecuteNonQuery(); // This returns the number of rows affected

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // You can log ex.Message if needed
                return false;
            }
        }





        // UpdateStock method
        public bool UpdateStock(Stock s)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Stock 
                             SET ProductId = @ProductId, 
                                 WarehouseId = @WarehouseId, 
                                 Quantity = @Quantity,
                                 LastUpdated = GETDATE()
                             WHERE StockId = @StockId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@StockId", s.StockId);
                        cmd.Parameters.AddWithValue("@ProductId", s.ProductId);
                        cmd.Parameters.AddWithValue("@WarehouseId", s.WarehouseId);
                        cmd.Parameters.AddWithValue("@Quantity", s.Quantity);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // DeleteStock method
        public bool DeleteStock(int stockId)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Stock WHERE StockId = @StockId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@StockId", stockId);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }



        public List<StockAdjustment> StockAdjustment()
        {
            List<StockAdjustment> adjustments = new List<StockAdjustment>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT sa.AdjustmentId, sa.QuantityChange, sa.Reason, sa.AdjustmentDate,
                   p.ProductName, w.WarehouseName
            FROM StockAdjustments sa
            JOIN Product p ON sa.ProductId = p.ProductId
            JOIN Warehouses w ON sa.WarehouseId = w.WarehouseId
            ORDER BY sa.AdjustmentDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        adjustments.Add(new StockAdjustment
                        {
                            AdjustmentId = Convert.ToInt32(reader["AdjustmentId"]),
                            QuantityChange = Convert.ToInt32(reader["QuantityChange"]),
                            Reason = reader["Reason"].ToString(),
                            AdjustmentDate = Convert.ToDateTime(reader["AdjustmentDate"]),
                            ProductName = reader["ProductName"].ToString(),
                            WarehouseName = reader["WarehouseName"].ToString()
                        });
                    }
                }
            }
            return adjustments;
        }



        public bool UpdateStockAdjustment(StockAdjustment sa)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE StockAdjustments 
                                 SET QuantityChange = @QuantityChange, 
                                     Reason = @Reason, 
                                     AdjustmentDate = @AdjustmentDate
                                 WHERE AdjustmentId = @AdjustmentId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@AdjustmentId", sa.AdjustmentId);
                        cmd.Parameters.AddWithValue("@QuantityChange", sa.QuantityChange);
                        cmd.Parameters.AddWithValue("@Reason", sa.Reason);
                        cmd.Parameters.AddWithValue("@AdjustmentDate", sa.AdjustmentDate);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteStockAdjustment(int adjustmentId)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM StockAdjustments WHERE AdjustmentId = @AdjustmentId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@AdjustmentId", adjustmentId);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public List<StockTransfer> StockTransfer()
        {
            List<StockTransfer> transfers = new List<StockTransfer>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT st.TransferId, st.Quantity, st.TransferDate,
               p.ProductName, 
               wf.WarehouseName AS FromWarehouse, 
               wt.WarehouseName AS ToWarehouse
        FROM StockTransfers st
        JOIN Product p ON st.ProductId = p.ProductId
        JOIN Warehouses wf ON st.FromWarehouseId = wf.WarehouseId
        JOIN Warehouses wt ON st.ToWarehouseId = wt.WarehouseId
        ORDER BY st.TransferDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        transfers.Add(new StockTransfer
                        {
                            TransferId = Convert.ToInt32(reader["TransferId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            TransferDate = Convert.ToDateTime(reader["TransferDate"]),
                            ProductName = reader["ProductName"].ToString(),
                            FromWarehouse = reader["FromWarehouse"].ToString(),
                            ToWarehouse = reader["ToWarehouse"].ToString()
                        });
                    }
                }
            }
            return transfers;
        }

        public bool AddStockTransfer(StockTransfer st)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO StockTransfers (ProductId, FromWarehouseId, ToWarehouseId, Quantity, TransferDate)
                                 VALUES (@ProductId, @FromWarehouseId, @ToWarehouseId, @Quantity, @TransferDate)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", st.ProductId);
                        cmd.Parameters.AddWithValue("@FromWarehouseId", st.FromWarehouseId);
                        cmd.Parameters.AddWithValue("@ToWarehouseId", st.ToWarehouseId);
                        cmd.Parameters.AddWithValue("@Quantity", st.Quantity);
                        cmd.Parameters.AddWithValue("@TransferDate", st.TransferDate);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public bool AddStockAdjustment(StockAdjustment sa)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO StockAdjustments (ProductId, WarehouseId, QuantityChange, Reason, AdjustmentDate)
                                 VALUES (@ProductId, @WarehouseId, @QuantityChange, @Reason, @AdjustmentDate)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", sa.ProductId);
                        cmd.Parameters.AddWithValue("@WarehouseId", sa.WarehouseId);
                        cmd.Parameters.AddWithValue("@QuantityChange", sa.QuantityChange);
                        cmd.Parameters.AddWithValue("@Reason", sa.Reason);
                        cmd.Parameters.AddWithValue("@AdjustmentDate", sa.AdjustmentDate);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public bool AddWarehouses(Warehouse w)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Warehouses (WarehouseName, Location)
                                 VALUES (@WarehouseName, @Location)";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@WarehouseName", w.WarehouseName);
                        cmd.Parameters.AddWithValue("@Location", w.Location);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public List<Stock> GetStocksByWarehouseId(int warehouseId)
        {
            List<Stock> stocks = new List<Stock>();
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT
                            s.StockId,
                            s.ProductId,
                            p.ProductName,
                            s.Quantity,
                            s.LastUpdated,
                        p.UnitPrice,
                            s.WarehouseId,
                            w.WarehouseName,
                            p.ProductCode
                         FROM
                            Stock s
                         JOIN
                            Product p ON s.ProductId = p.ProductId
                         JOIN
                            Warehouses w ON s.WarehouseId = w.WarehouseId
                         WHERE
                            s.WarehouseId = @warehouseId;";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@warehouseId", warehouseId);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        stocks.Add(new Stock
                        {
                            StockId = Convert.ToInt32(reader["StockId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            LastUpdated = Convert.ToDateTime(reader["LastUpdated"]),
                            UnitPrice = Convert.ToInt32(reader["UnitPrice"]),
                            WarehouseId = Convert.ToInt32(reader["WarehouseId"]),
                            WarehouseName = reader["WarehouseName"].ToString(),
                            ProductCode = reader["ProductCode"].ToString()
                        });
                    }
                }
            }
            return stocks;
        }

    }
}