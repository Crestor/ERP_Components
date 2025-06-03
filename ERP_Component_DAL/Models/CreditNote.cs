namespace ERP_Component_DAL.Models
{
    public class CreditNote
    {
        public Guid CreditNoteId { get; set; }
        public DateOnly CreatedOn { get; set; }
        public string OrderSeriesID { get; set; }
        public string CreditNoteFor { get; set; }
        public string CreditNoteType { get; set; }
        public DateOnly CreditNoteDate { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public int NumberOfCartons { get; set; }
        public string LorryReceiptNumber { get; set; }
        public DateOnly LorryReceiptDate { get; set; }
        public string ModeOfShipment { get; set; }
        public string Remarks { get; set; }
        public string CostCenter { get; set; }
        public string Location { get; set; }

        public string VenderNoteId { get; set; }
        public string Status { get; set; }

    }
}