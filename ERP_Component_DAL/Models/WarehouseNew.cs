using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class WarehouseNew
    {
        public List<Warehouse> warehouseNames { get; set; }
        public List<Items> itemNames { get; set; }

        public Warehouse warehouseLocation { get; set; }
        public Warehouse warehouseStock { get; set; }


    }
}
