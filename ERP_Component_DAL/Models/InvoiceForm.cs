using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class InvoiceForm
    {
       
        public Guid InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateOnly InvoiceDate { get; set; }
        public string Status { get; set; }
        public CustomerDetails Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }
        public decimal GrossTotal { get; set; }              
        public decimal TDSPercentage { get; set; }
        public decimal TDSAmount { get; set; }
        public decimal TCSPercentage { get; set; }
        public decimal TCSAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public decimal PaymentTerm { get; set; }
        public Guid TermConditionID { get; set; }
        public Guid QuotationID { get; set; }
        public string QuotationSeries { get; set; }
        public string OtherReferences { get; set; }
    }
}
