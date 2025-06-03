namespace ERP_Component_DAL.Models
{
    public class CustomerHistory
    {
        public Guid QuotationID { get; set; }
        public Guid InvoiceID { get; set; }

        public string CustomerName { get; set; }


        public int ProductID { get; set; }


        public string ProductName { get; set; }



        public int Quantity { get; set; }



        public decimal UnitPrice { get; set; }


        public decimal TaxableAmount { get; set; }


        public decimal discountRate { get; set; }


        public decimal DiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }
        //public decimal GrossTotal { get; set; }




        public DateOnly InvoiceDate { get; set; }

        public String Status { get; set; }

        public long ContactNumber { get; set; }
        public decimal finalAmount { get; set; }
        public decimal TotalDiscountPercent { get; set; }

        public decimal TotalDiscountAmount { get; set; }
        public decimal totalPriceBeforeDiscount { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
    }
}
