using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class DashBoard
    {
        public int Complete { get; set; }
        public int Pending { get; set; }

        public string FilterType { get; set; }
        public decimal TotalAmount { get; set; }
        public DateOnly CreatedAt { get; set; }
        public int Categories { get; set; }
        public int PendingOrder { get; set; }
        public decimal TotalPurchases { get; set; }
        public int TotalVendor { get; set; }
        public int PendingDispatch { get; set; }
        public int TotalStorageItems { get; set; }
        public string CategoryName { get; set; }
        public int TotalStock { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalOrder { get; set; }
        public decimal TotalSales { get; set; }
        public int VenderInvoice { get; set; }
        public int TotalPurchaseRequisition { get; set; }
        public int TotalMaterialRequisition { get; set; }
        public decimal TotalStockValue { get; set; }
        public int InStock { get; set; }
        public DateOnly LastUpdated { get; set; }
        public List<DashBoard> Stockdata { get; set; }
        public List<DashBoard> PieChartData { get; set; }
        public List<DashBoard> ComparisonSalesPurchase { get; set; }
        public List<DashBoard> OrderSummary { get; set; }
    }
}
