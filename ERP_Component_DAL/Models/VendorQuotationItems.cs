using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class VendorQuotationItem
    {
        public VendorQuotationItem()
        {
            this.VendorQuotationItems = new List<VendorQuotationItem>();
        }

        public List<VendorQuotationItem>? VendorQuotationItems { get; set; }
        public Guid ItemID { get; set; }
        public string? ItemName { get; set; }
        public string? UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
