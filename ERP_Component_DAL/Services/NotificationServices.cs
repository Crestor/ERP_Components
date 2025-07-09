using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class NotificationServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;
        private string _connectionString;

        public NotificationServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = config.GetConnectionString("DefaultConnectionString");
        }

        public MonthlyRetailSales GetPendingQuotationCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS PendingRequisitions FROM PurchaseRequisitions WHERE RequisitionStatus = 2";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["PendingRequisitions"] != DBNull.Value ? (int)reader["PendingRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetPurchaseViewRequisitionsCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS PendingRequisitions FROM PurchaseRequisitions  Where RequisitionStatus = 1";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["PendingRequisitions"] != DBNull.Value ? (int)reader["PendingRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetCreateVendorQuotationCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS PendingRequisitions FROM PurchaseRequisitions Where RequisitionStatus = 1";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["PendingRequisitions"] != DBNull.Value ? (int)reader["PendingRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetGeneratePurchaseOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS PurchaseOrders FROM PurchaseOrders Where OrderStatus = 2 ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["PurchaseOrders"] != DBNull.Value ? (int)reader["PurchaseOrders"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetPurchaseOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS PurchaseOrders FROM PurchaseRequisitions Where RequisitionStatus = 3 ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["PurchaseOrders"] != DBNull.Value ? (int)reader["PurchaseOrders"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetInvoiceCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalInvoice FROM Invoice WHERE Status = 'Unpaid' ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalInvoice"] != DBNull.Value ? (int)reader["TotalInvoice"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetViewOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM Requisitions WHERE RequisitionType IN (1,4) And RequisitionStatus = 1 ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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

        public MonthlyRetailSales GetViewCompletedOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM Requisitions r JOIN WorkOrder wo ON wo.SalesForecastID = r.RequisitionID  WHERE r.RequisitionType IN (1,4) AND r.RequisitionStatus = 4  GROUP BY r.RequisitionID, r.Description, r.RequisitionSeries,r.CreatedAt  HAVING COUNT(wo.WorkOrderID) = SUM(CASE WHEN wo.WorkOrderStatus = 3 THEN 1 ELSE 0 END)";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetMaterialRequisitionCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM Requisitions WHERE RequisitionType = 3 AND RequisitionStatus = 1";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetViewStoreOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM Requisitions WHERE RequisitionType = 2 AND RequisitionStatus = 1";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetRecievePurchaseOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM PurchaseOrders WHERE OrderStatus = 1 ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetViewPrMrCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"SELECT COUNT(*) AS TotalRequisitions FROM Requisitions WHERE RequisitionType = 2 AND RequisitionStatus = 7";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetWorkOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $" Select COUNT(*) AS TotalRequisitions from WorkOrder Where WorkOrderStatus = 1\r\n";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetViewWeavingOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"  SELECT COUNT(*) AS TotalRequisitions FROM AllocatedWork aw JOIN Workers w ON aw.WorkerID = w.WorkerID where aw.RecievedQuantity<aw.Quantity";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetDyeingOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"  SELECT  COUNT(*) AS TotalRequisitions FROM DyeingOrder where OrderStatus = 1";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalRequisitions"] != DBNull.Value ? (int)reader["TotalRequisitions"] : 0;

                }


                return name;

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

        public MonthlyRetailSales GetCompletedWorkOrderCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"Select COUNT(*) AS TotalCount from WorkOrder where WorkOrderStatus = 5";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalCount"] != DBNull.Value ? (int)reader["TotalCount"] : 0;

                }


                return name;

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
        public MonthlyRetailSales GetReceivableCount()
        {
            try
            {
                MonthlyRetailSales name = new();
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(connectionstring);
                SqlCommand cmd = new();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select COUNT(*) AS TotalCount from Invoice I join CustomerQuotation Q on Q.QuotationID=I.QuotationID join Customers C on C.CustomerID=Q.CustomerID ";

                cmd.Connection = connection;
                cmd.CommandTimeout = 300;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    name.BillCount = reader["TotalCount"] != DBNull.Value ? (int)reader["TotalCount"] : 0;

                }


                return name;

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
