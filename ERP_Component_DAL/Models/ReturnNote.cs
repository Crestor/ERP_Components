namespace ERP_Component_DAL.Models
{
    public class ReturnNote
    {
        public string ItemName { get; set; }
        public Guid ItemId { get; set; }
        public decimal SellingPrice { get; set; }
        public string UOM { get; set; }


        public int HSN { get; set; }

        public Guid ReturnNoteId { get; set; }
        public string DeliveryChallanID { get; set; }
        //public string ProductName { get; set; }
        public int Quantity { get; set; }

        public int ReturnableQuantity { get; set; }
        public string Status { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly ReturnNotedate { get; set; }
    }
}