using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class BOM
    {
            public List<Item> Products { get; set; }
        public List<Item> MaterialList { get; set; }
        public Guid ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Spesification { get; set; }
        public List<Materail>? materials { get; set; }  = new List<Materail>();
        
        public class Materail
        {
            public Guid MatrialID { get; set; }
            public decimal Quantity { get; set; }
            public string? UOM { get; set; }
            public string? MaterialName { get; set; }
        }
    }
}
