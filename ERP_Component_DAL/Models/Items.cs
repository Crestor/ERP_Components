using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Items
    {
        //Items
        public Guid itemId {  get; set; }
        public string? itemName { get; set; }
        public string? itemType { get; set; }

        public int SKU { get; set; }
        public int HSN { get; set; }
        public string? category { get; set; }
        public string? subCategory { get; set; }
        public string? specification { get; set; }
        public int gst { get; set; }
        public string? UOM { get; set; }
        public int categoryId {  get; set; }
      public int subCategoryId { get; set; }

        // Price
        public Guid priceId { get; set; }
        public decimal unitPrice { get; set; }
        public decimal mrp { get; set; }
        public decimal costPrice { get; set; }
        public decimal sellingPrice { get; set; }
       
      
 
        public Guid lotId{ get; set; }
        public Guid employeeId{ get; set; }
      
        public string? lotSeries{get;set;}
        public string?  batchSeries{get;set;}
        public DateTime expiry { get; set; }

        public int daysToExpire { get; set; }
        public DateTime manufacture { get; set; }

        public DateTime deliverd { get; set; }
        public string? type { get; set; }


        public string? warehouse { get; set; }

        public Guid inventoryId { get; set; }
      
     
        public int inStock { get; set; }
        public int stockAlert { get; set; }


        public Guid warehouseId { get; set; }
        

    }
}
