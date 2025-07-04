namespace ERP_Component_DAL.Models
{
    public class Vendor
    {
        public Guid vendorId { get; set; }
        public Guid vendorQuotationId { get; set; }
        public Guid requisitionId { get; set; }
        public Guid purchaseOrderId { get; set; }
        public string? vendorName { get; set; }

        public decimal amount { get; set; }
        public decimal discountRate { get; set; }
        public decimal advanceRate { get; set; }
        public decimal finalAmount { get; set; }
        public decimal discountAmount { get; set; }
        public decimal advancedAmount { get; set; }
        public decimal advance { get; set; }

        public DateTime createdAt { get; set; }
         
        public int creditPeriod { get; set; }
        public int status { get; set; }

        public string? paymentTerms { get; set; }
        public string? GstIn { get; set; }
        public string? vendorCode { get; set; }
        public string? deliveryTerms { get; set; }
        public string? description { get; set; }

        public string Address1 { get; set; }
        public string? invoiceNumber { get; set; }
        
      
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string? PostalCode { get; set; }

        public List<Vendor> vendor { get; set; }
        public List<Vendor> lists{ get; set; }

        public List<AddPurchaseRequisition> Items { get; set; }

        public List<VendorQuotationItem> Product { get; set; }

    }
}
