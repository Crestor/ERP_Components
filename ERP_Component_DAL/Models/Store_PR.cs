using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Store_PR
    {
        public Guid StorePRID { get; set; }
        public string StorePRSeries { get; set; }
        public Guid RequisitionID { get; set; } 
        public Item item { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
