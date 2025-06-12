using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
   public class itemList
    {
        public Guid itemId { get; set; }
        public string? itemName { get; set; }

        public string? specification { get; set; }

        public int? quantity { get; set; }

        public decimal? UnitPrice { get; set; }  
    }
}
