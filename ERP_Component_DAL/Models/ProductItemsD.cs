namespace ERP_Component_DAL.Models
{
    public class ProductItemsD
    {
         public List<ItemDetails> ProductNames { get; set; }

         public  string ProductName  {get;  set;}

         public long   Quantity {get; set;}

         public long UnitPrice { get; set; }

         public long discountRate { get; set; }

         public long discountAmount { get; set; }

         public long taxableAmount { get; set; }

         public long cgst { get; set; }
 
         public long sgst { get; set; }

         public long TotalAmount { get; set; }

    }
}
