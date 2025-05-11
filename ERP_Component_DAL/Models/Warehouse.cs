using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Warehouse
    {
        public Guid warehouseId { get; set; }
        public Guid itemId { get; set; }
        public Guid detailId { get; set; }
        public string warehouseName { get; set; }
        public string itemName { get; set; }
        public int addressId { get; set; }
        public int quantity { get; set; }
        public int locationId { get; set; }

        public string EmployeeId { get; set; } 
        public int type { get; set; }
        public string batchSeries { get; set; }

        public string Address2 { get; set; }
        public string aisle { get; set; }
        public string description { get; set; }
        public string shelfNumber { get; set; }
        public string zoneName { get; set; }
        public string Address1 { get; set; }
        public string locationCode { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string? PostalCode { get; set; }

        public DateTime CreatedOn {  get; set; }

    }
}
