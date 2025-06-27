using System;
namespace ERP_Component_DAL.Models

{
 public class RetailItemModel
    {
        public string ItemName { get; set; }
        public decimal MRP { get; set; }
        public int Quantity { get; set; }
        public DateOnly date { get; set; }
        public Guid RetailBillID { get; set; }
        public Guid RetailCustomerId { get; set; }
        public string CustomerName { get; set; }

    }
}
