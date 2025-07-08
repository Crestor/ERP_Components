using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class BOM
    {
        public Guid ProductID { get; set; }
        public List<Materail>? materials { get; set; }
        
        public class Materail
        {
            public Guid MatrialID { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
