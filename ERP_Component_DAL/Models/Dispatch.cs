
namespace ERP_Component_DAL.Models
{
    public class Dispatch
    {
        public Guid InvoiceID { get; set; }
        public string ?TransporterID { get; set; }
        public string ?ModeOfShipment { get; set; }
        public string ?TransporterDocumentNumber { get; set; }
        public string ?GRNumber { get; set; }
        public string ?NumberOfCartons { get; set; }
        public string ?TransporterName { get; set; }
        public string ?VehicleType { get; set; }
        public DateOnly ?TransporterDocumentDate { get; set; }
        public int ?NetWeight { get; set; }
        public string ?TransactionType { get; set; }
        public int ?DistanceKM { get; set; }
        public string ?VehicleNumber { get; set; }

        public TimeOnly ?TimeOfSupply { get; set; }
        public int ?GrossWeight { get; set; }
        public FreightChargesEnum ?FreightCharges { get; set; }
        public string ?AddressLine1 { get; set; }
        public string ?District { get; set; }
        public string ?Area { get; set; }
        public string ?City { get; set; }
        public string ?State { get; set; }
        public string ?Pincode { get; set; }
        public string ?CourierDetail { get; set; }
        public string ?CourierCompany { get; set; }
        public string ?TrackingNumber { get; set; }


        public override string ToString()
        {
            return $"Dispatch Details:\n" +
                   $"  Transporter: {TransporterName} (ID: {TransporterID})\n" +
                   $"  Mode of Shipment: {ModeOfShipment}\n" +
                   $"  Vehicle: {VehicleType} - {VehicleNumber}\n" +
                   $"  Document: {TransporterDocumentNumber} on {TransporterDocumentDate}\n" +
                   $"  GR Number: {GRNumber}\n" +
                   $"  Cartons: {NumberOfCartons}\n" +
                   $"  Weight: {GrossWeight} kg (Net: {NetWeight} kg)\n" +
                   $"  Freight: {(byte)FreightCharges}\n" +
                   $"  Distance: {DistanceKM} KM\n" +
                   $"  Transaction Type: {TransactionType}\n" +
                   $"  Time of Supply: {TimeOfSupply}\n" +
                   $"  Dispatch Address: {AddressLine1}, {Area}, {City}, {District}, {State} - {Pincode}\n" +
                   $"  Courier: {CourierCompany} ({CourierDetail}) - Tracking: {TrackingNumber}";
        }
    }

    public enum FreightChargesEnum
    {
        Unpaid, //0
        Paid //1
    }
}
