namespace ERP_Component_DAL.Models
{
    public class DeliveryChallan
    {
        public Guid DeliveryChallanID { get; set; }
        public string DeliveryChallanType { get; set; }
        public string Branch { get; set; }
        public string OrderSeriesID { get; set; }
        public string CustomerName { get; set; }
        public string SecuritySealNumber { get; set; }
        public DateOnly ChallanDate { get; set; }
        public string Status { get; set; }
    }
}