namespace ERP_Component_DAL.Models
{
    public class QuotationModel
    {
        public List<RetailItemModel> IDetails { get; set; }

        public decimal MRP { get; set; }
       public Guid RetailBillID { get; set; }
        public Guid RetailCustomerId { get; set; }
        public List<QuotationModel> items { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public string DispatchNumber { get; set; }

        public string ItemCode { get; set; }
        public Guid ItemId { get; set; }
        public int RequisitionType { get; set; }
        public string RequisitionSeries { get; set; }
        public Guid RequisitionID { get; set; }
        public decimal SellingPrice { get; set; }
        public string UnitOFMeasure { get; set; }


        public string HSN { get; set; }
        public string QuotationSeries { get; set; }
        public Guid TermConditionID { get; set; }



        //new
        public Guid QuotationID { get; set; }

        //public decimal TotalDiscountAmount { get; set; }
        public string OrderSeries { get; set; }

        public string CustomerName { get; set; }
        public Guid CustomerID { get; set; }


        public Guid ProductID { get; set; }


        public string ProductName { get; set; }



        public int Quantity { get; set; }



        public decimal UnitPrice { get; set; }


        public decimal TaxableAmount { get; set; }


        public decimal discountRate { get; set; }


        public decimal DiscountAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }


        public decimal CGST { get; set; }


        public decimal SGST { get; set; }


        public decimal IGST { get; set; }

        public decimal TotalAmount { get; set; }
        //public decimal GrossTotal { get; set; }

        //public decimal grossTotal { get; set; }

        public string DeliveryTerms { get; set; }


        public string PaymentTerm { get; set; }
        public string Other { get; set; }

        public DateOnly QuotationDate { get; set; }
        public DateOnly date { get; set; }

        public String Status { get; set; }

        public long ContactNumber { get; set; }
        public string ContactNO { get; set; }
        public decimal finalAmount { get; set; }
        public decimal TotalDiscountPercent { get; set; }

     
        public decimal totalPriceBeforeDiscount { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }

        public RetailItemModel Store {get; set; }
        public List<QuotationModel> ItemLists { get; set; }
    }
}
