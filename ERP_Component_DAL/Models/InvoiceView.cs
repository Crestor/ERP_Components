namespace ERP_Component_DAL.Models
{
    public class InvoiceView
    {
        public List<Invoice> ProductView { get; set; }
        public List<Invoice> ListView { get; set; }

        public BusinessSetUp BusinessSetUp { get; set; }
    }
}
