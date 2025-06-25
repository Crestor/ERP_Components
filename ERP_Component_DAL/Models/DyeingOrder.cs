using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class DyeingOrder
    {
        public Guid DyeingOrderID { get; set; }
        public Guid WorkerID { get; set; }
        public string WorkerName { get; set; }
        public int Quantity { get; set; }
        public WorkOrderStatuses OrderStatus { get; set; }
        public Guid WorkOrderID { get; set; }

        public List<DyeingOrder> dyeingOrders { get; set; }
    }
}
