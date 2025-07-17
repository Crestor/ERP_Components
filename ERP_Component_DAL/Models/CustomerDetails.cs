namespace ERP_Component_DAL.Models
{
    public class CustomerDetails
    {
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string GSTIN { get; set; } 
        public string ContactNumber { get; set; }
        public string Branch { get; set; }
    }
}