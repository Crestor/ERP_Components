using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class SalesForecast
    {
        public Guid RequisitionID  { get; set; }
        public string Description { get; set; }
        public DateOnly CreatedAt { get; set; }
        public byte RequisitionStatus { get; set; }

    }
}
