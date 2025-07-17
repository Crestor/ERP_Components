namespace ERP_Component_DAL.Models
{
    public class ShippingDetails
    {
       
        public Guid DispatchID { get; set; }
        public Guid TransportID { get; set; }
        public Guid CourierID { get; set; }
        public string ModeOfShipment { get; set; }
        public string DeliveryTerms { get; set; }
        public string EWayBill { get; set; }
        public string ConsignmentNumber { get; set; }
        public string TransporterID { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public int Distance { get; set; }
        public string CourierCompany { get; set; }
        public string TrackingNumber { get; set; }
    }
}