namespace ERP_Component_DAL.Models
{
    public class InvoiceItem
    {
        public Guid ProductID { get; set; }       
        public string ProductName { get; set; }   
        public string HSNCode { get; set; }       
        public int Quantity { get; set; }
        public string UOM { get; set; }           
        public decimal UnitPrice { get; set; }     

        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }

       
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }

        public decimal TotalAmount { get; set; }
    }
}