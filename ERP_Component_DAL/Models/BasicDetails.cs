namespace ERP_Component_DAL.Models
{
    public class BasicDetails
    {
       public string OrderSeries { get; set; }

        public string OrderType { get; set; }

        public string VendorName { get; set; }

        public string VendorRQ { get; set; }
        
        public string PurchaseOD { get; set; }

        public string InvoiceDueIn { get; set; }

        public DateOnly PostDate { get; set; }

        public string CostCenter { get; set;}

        public string JobName { get; set; }

        public string Project { get; set; }

        public string JobNumber { get; set; }      

        public DateOnly ExpectedDPD { get; set; }

        public DateOnly ExpectedDLD { get; set; }

        public DateOnly ExternalSPo { get; set; }

        public string items { get; set; }
        public long Quantity { get; set; }
        public long Unit { get; set; }
        public long Rate { get; set; }
        public long  Total { get; set; }
        public long Discount { get; set; }
        public long Taxablevalue { get; set; }
        public long Cgst { get; set; }
        public long Sgst { get; set; }
        public long TaxAmount { get; set; }           
        public long TotalAmount { get; set; }

        public List<ItemDetails> ListItemdetail {get; set;}


        public string Coyname { get; set; }

        public string BName { get; set; }

        public string ArName { get; set; }

        public string SName { get; set; }

        public string DistName { get; set; }

        public string CName { get; set; }

        public string PinNo { get; set; }

        public string Rmarks { get; set; }

        

    }
}
