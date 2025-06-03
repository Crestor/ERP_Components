namespace ERP_Component_DAL.Models
{
    public class addeditems
    {
      //public  List<ProductItemsD> Product { get; set; } = new List<ProductItemsD>();

      //public  List<ItemDetails> ItemDetails { get; set; } = new List<ItemDetails>();
        public string ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal discountRate { get; set; }

        public decimal discountAmount { get; set; }

        public decimal taxableAmount { get; set; }

        public decimal cgst { get; set; }

        public decimal sgst { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
