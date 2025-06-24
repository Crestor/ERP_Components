using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
  

    public class DynamicRetailSales
    {
        public string StoreName { get; set; }
        public Dictionary<string, decimal> SalesByPeriod { get; set; } = new Dictionary<string, decimal>();
    }

}
