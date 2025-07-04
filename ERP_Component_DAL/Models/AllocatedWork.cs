using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class AllocatedWork
    {
        public Guid AllocatedWorkID { get; set; }
        public Guid WorkOrderID { get; set; }
        public string AllocationCode { get; set; }
        public Guid WorkerID { get; set; }
        public string WorkerName { get; set; }
        public int Quantity { get; set; }
        public decimal RatePerPeices { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RecievedDate { get; set; }
        public int RecievedQuantity { get; set; }
        public byte WorkStatus { get; set; }
        public Guid? AllocatedYarnID { get; set; }

        public List<AllocatedWork> allocatedWorks { get; set; }

    }
}
