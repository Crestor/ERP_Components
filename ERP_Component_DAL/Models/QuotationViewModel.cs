namespace ERP_Component_DAL.Models
{
    public class QuotationViewModel
    {
        public List<QuotationModel> items { get; set; }

        public List<QuotationModel> ItemNames { get; set; }
        public List<QuotationModel> OrderList { get; set; }
        public List<QuotationModel> OrderTable { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalDiscountPercent { get; set; }
        public decimal GrossTotal { get; set; }

        public decimal totalPriceBeforeDiscount { get; set; }
        public string OrderSeries { get; set; }

        public string CustomerName { get; set; }
        public string DeliveryTerms { get; set; }

        public decimal TotalAmountAfterDiscount { get; set; }
        public string PaymentTerm { get; set; }

        public List<Customer>? Customers { get; set; }

        public RetailItemModel Retail { get; set; }
        public List<RetailItemModel> RetailItem { get; set; }

        public class Customer
        {
            public Guid CustomerID { get; set; }
            public string CustomerName { get; set; }
        }

       
    }
}