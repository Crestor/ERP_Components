using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Item
    {
        public Guid itemId { get; set; }
        public string? itemName { get; set; }
        public int sku { get; set; }
        public string? specification { get; set; }
        public byte itemType { get; set; }
        public int locationId { get; set; }
        public string hsn { get; set; }
        public decimal gstRate { get; set; }
        public string? unitOfMeasure { get; set; }
        public Category? category { get; set; }
        public SubCategory? subCategory { get; set; }
        public decimal price { get; set; }
        public DateOnly manufacturingDate { get; set; }
        public override string? ToString()
        {
            return $"{itemName}-{specification}-{hsn}-{sku}-{price}-{manufacturingDate.ToString()}";
        }
    }
}
