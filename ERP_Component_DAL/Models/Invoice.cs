namespace ERP_Component_DAL.Models

{
    public class Invoice

    {
        public string ItemName { get; set; }
        public Guid ItemId { get; set; }
        public decimal SellingPrice { get; set; }
        public string UnitOFMeasure { get; set; }
        public int AddressID { get; set; }
        public string Country { get; set; }
        public int HSN { get; set; }
        public Guid TermConditionID { get; set; }
        public Guid QuotationID { get; set; }
        //Shipped To Content
        public decimal TCSAmount { get; set; }
        public decimal TCSPercentage { get; set; }
        public string PaymentMode { get; set; }
        public string ReferenceNumber { get; set; }
        public string PaymentType { get; set; }
        public Guid InvoiceID { get; set; }
        public string EWayBill { get; set; }
        public string InvoiceNumber { get; set; }
        public string CompanyName { get; set; }


        public DateOnly InvoiceDate { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string ConsignmentNumber { get; set; }
        public string Branch { get; set; }
        public string District { get; set; }
        //public long ContactNumber { get; set; }
        public string GSTIN { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string ShippingRemote { get; set; }
        public string OtherReferences { get; set; }


        //Transport/E-Way Bill  Content
        public Guid TransportID { get; set; }

        public string TransporterID { get; set; }
        public string ModeOfShipment { get; set; }

        public string GRNumber { get; set; }
        public int NumberOfCartons { get; set; }
        public string TransporterName { get; set; }
        public DateOnly TransporterDocumentDate { get; set; }
        public string TransporterDocumentNumber { get; set; }
        public string VehicleType { get; set; }
        //public string TransportContact { get; set; }
        public decimal NetWeight { get; set; }
        public string TransactionType { get; set; }
        public int Distance { get; set; }
        public string VehicleNumber { get; set; }
        public TimeOnly TimeOfSupply { get; set; }
        public decimal GrossWeight { get; set; }
        public string FreightCharges { get; set; }





        //Dispatch content

        public Guid DispatchID { get; set; }
        public string AddressLine1 { get; set; }




        //CourierDetails content

        public Guid CourierID { get; set; }

        public string CourierDetail { get; set; }
        public string CourierCompany { get; set; }
        public string TrackingNumber { get; set; }
        //Quotation details
        //public decimal TotalDiscountAmount { get; set; }
        public string OrderSeries { get; set; }
        public string QuotationSeries { get; set; }

        public string CustomerName { get; set; }


        public int ProductID { get; set; }


        public string ProductName { get; set; }


        public string HSNCode { get; set; }

        public int Quantity { get; set; }

        public string UOM { get; set; }


        public decimal UnitPrice { get; set; }


        public decimal TaxableAmount { get; set; }


        public decimal discountRate { get; set; }


        public decimal DiscountAmount { get; set; }


        public decimal CGST { get; set; }


        public decimal SGST { get; set; }


        public decimal IGST { get; set; }

        public decimal TotalAmount { get; set; }
        //public decimal GrossTotal { get; set; }


        public string DeliveryTerms { get; set; }


        public decimal PaymentTerm { get; set; }
        public string Other { get; set; }

        public DateOnly date { get; set; }

        public String Status { get; set; }

        public long ContactNumber { get; set; }
        public decimal finalAmount { get; set; }
        public decimal TotalDiscountPercent { get; set; }

        public decimal TotalDiscountAmount { get; set; }
        public decimal totalPriceBeforeDiscount { get; set; }
        public decimal TotalAmountAfterDiscount { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal TDSPercentage { get; set; }
        public decimal TDSAmount { get; set; }
        public List<Invoice> ViewInvoiceProd { get; set; }
    }
}
