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
        public string? itemName { get; set; }
        public int itemType { get; set; }
        public string? category { get; set; }
        public string? serialNumber { get; set; }
        public DateTime warrantyEndDate { get; set; }
        public decimal price { get; set; }
        public string? vendorName { get; set; }
        public int warranty { get; set; }
        public string? location { get; set; }
        public string? description { get; set; }
        public string? departmentName { get; set; }
        public string? assignto { get; set; }
        public Guid employeeId { get; set; }
        public Guid departmentId { get; set; }

        public int status { get; set; }
       

    }
}
