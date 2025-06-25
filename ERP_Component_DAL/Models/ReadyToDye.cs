using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class VeiwOrdersReadyForDyeing
    {

        public List<ReadyToDye> orders { get; set; }

        public class ReadyToDye
        {
            public Guid workOrderId { get; set; }
            public string workOrderSeries { get; set; }
            public string productName { get; set; }
            public string specifications { get; set; }
            public decimal totalQuantity { get; set; }
            public int totalReceived { get; set; }

        }
    }
}
