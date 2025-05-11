using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Adjustment
    {
        public Guid itemId { get; set; }
        
       public Guid EmployeeId { get; set; }
        public Guid lotId { get; set; }
        public Guid warehouseId { get; set; }
       
        public DateOnly Date { get; set; }

        public string? reference { get; set; }
        public string? itemName { get; set; }
        public string? warehouseName { get; set; }
        public string? type { get; set; }
        public string? reason { get; set; }
        public int currentStock { get; set; }
        public int newStock { get; set; }
      

    }
}
