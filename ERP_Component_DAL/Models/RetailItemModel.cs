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
        public decimal GrossTotal { get; set; }
        public decimal NetTotal { get; set; }
        public decimal GST { get; set; }
        public string CustomerName { get; set; }
        public string CenterCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Pincode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public string CenterName { get; set; }

        public List<RetailItemModel> retailItem { get; set; }

        public List<QuotationModel> Products { get; set; }
    }
}
