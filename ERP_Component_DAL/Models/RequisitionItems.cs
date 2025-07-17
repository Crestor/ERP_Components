using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class RequisitionItem
    {
        public Guid itemId { get; set; }
        public string? itemName { get; set; }
        public decimal quantity { get; set; }
        public string uom { get; set; }
    }
    public class PurchaseRequisitionItems : RequisitionItem
    {
        public PurchaseRequisitionItems()
        {
            this.storePRIDs = new List<Guid>();
        }
        public Guid StorePRID { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalAmount { get; set; }
        public List<Guid> storePRIDs { get; set; }
    }
    public class PurchaseItem: RequisitionItem
    {
        public int quantity { get; set; }
        public string specifications { get; set; }
    }
}
