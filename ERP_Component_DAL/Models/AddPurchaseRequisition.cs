namespace ERP_Component_DAL.Models
{
    public class AddPurchaseRequisition

    {
        public Guid itemId { get; set; }
        public Guid MaterialId { get; set; }
        public string itemName { get; set; }
        public string vendorName { get; set; }

        public string category { get; set; }
        public string specification { get; set; }
        public string unitofmeasure { get; set; }
        public int gst { get; set; }
        public decimal quantity { get; set; }
        public decimal cgstRate { get; set; }
        public decimal cgstAmount { get; set; }
        public decimal sgstRate { get; set; }
        public decimal sgstAmount { get; set; }
        public decimal igstRate { get; set; }
        public decimal igstAmount { get; set; }
        public decimal requiredQuantity { get; set; }
        public decimal NetAmount { get; set; }
        public decimal availableQuantity { get; set; }
        public string itemtype { get; set; }
        public string? paymentTerms { get; set; }
        public string? deliveryTerms { get; set; }
        public string? status { get; set; }
        public int sku { get; set; }
        public string? hsn { get; set; }
        public decimal discount { get; set; }
        public decimal AvaiableQuantity { get; set; }
        //public decimal RequiredQuantity { get; set; }
        public decimal discountAmount { get; set; }

        List<AddPurchaseRequisition> Listbyreq { get; set; }

        public int PurchaseReqID { get; set; }
        public Guid RequisitionId { get; set; }
        public string? requisitionSeries { get; set; }

        public string Descripion { get; set; }

        public string RaisedBy { get; set; }

        public string RequiredBy { get; set; }
        //public string RequisitionSeries { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal unitPrice { get; set; }
        public decimal taxableValue { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }


        public List<AddPurchaseRequisition> listItesms { get; set; }

    }

}


