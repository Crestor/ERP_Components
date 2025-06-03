namespace ERP_Component_DAL.Models
{
    public class ProformaInvoice
    {
        public Guid ProformaInvoiceID { get; set; }
        public Guid TermConditionID { get; set; }
        public string JobCode { get; set; }
        public string POReferenceNumber { get; set; }
        public string CustomerName { get; set; }
        public string OrderSeriesID { get; set; }
        public string DeliveryTerms { get; set; }


        public string PaymentTerm { get; set; }

        public string VenderCode { get; set; }
        public DateOnly PODate { get; set; }
        public string Status { get; set; }


    }
}