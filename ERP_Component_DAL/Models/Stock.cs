using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Stock
    {
        public Guid itemId { get; set; }
        public Guid stockId { get; set; }
        public Guid purchaseId { get; set; }
        public Guid lotId { get; set; }
        public Guid warehouseId { get; set; }
     
        public DateTime expiry { get; set; }
        public DateTime arrival { get; set; }
       
        public string? invoice { get; set; }
        public string? itemName { get; set; }
        public string? warehouseName { get; set; }
        public string? batchSeries { get; set; }
        public decimal quantity { get; set; }
        public decimal sellingPrice { get; set; }
        public decimal unitPrice { get; set; }
        public decimal costPrice { get; set; }
        public decimal totalPrice { get; set; }
        public bool type { get; set; }


        public List<Items> items { get; set; }
        public List<Warehouse> warehouse { get; set; }
    }
}
