using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Asset
    {
        public Guid assetId { get; set; }

        public Guid itemId { get; set; }
        public Guid userId { get; set; }
        public Guid maintenanceId { get; set; }
        public string? itemName { get; set; }
        public string? userName { get; set; }
        public string? category { get; set; }
        public int itemType { get; set; }
        public int categoryId { get; set; }
        public int depreciation { get; set; }
        public string? serialNumber { get; set; }
        public DateTime expectedReturn { get; set; }
        public DateTime assignDate { get; set; }
        public DateTime warrantyEndDate { get; set; }
        public decimal price { get; set; }
        public decimal currentValue { get; set; }
        public decimal cost { get; set; }
        public string? vendorName { get; set; }
        public int warranty { get; set; }
        public string? location { get; set; }
        public string? description { get; set; }
        public string? departmentName { get; set; }
        public string? assignto { get; set; }
        public string? type { get; set; }
       
        public Guid employeeId { get; set; }
        public Guid departmentId { get; set; }

        public string? status { get; set; }

        public List<Asset> items { get; set; }

        
        public string Method { get; set; }
        public int UsefulLife { get; set; }
        public decimal ScrapValue { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? currentOwner { get; set; }
        public string? newOwner { get; set; }
    }
}
