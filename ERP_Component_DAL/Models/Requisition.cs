namespace ERP_Component_DAL.Models
{
    public class Requisition
    {
        public Requisition() { 

            this.requisitionItems = new List<RequisitionItem> ();
        }
        public Guid requisitionId { get; set; }
        public string? requisitionSeries { get; set; }
        public string? description { get; set; }
        public DateTime createdAt {get; set;}
        public RequisitionTypes requisitionType { get; set; }
        public RequisitionStatus requisitionStatus { get; set; }
        public List<RequisitionItem>? requisitionItems { get; set; }
        public List<Item> items { get; set; }
    }

    public class PurchaseRequisition : Requisition
    {
        public PurchaseRequisition()
        {
            this.purchaseRequisitionItems = new List<PurchaseRequisitionItems>();
        }
        public decimal totalAmount { get; set; }
        public List<PurchaseRequisitionItems>? purchaseRequisitionItems { get; set; }
        public List<Store_PR>? store_PRs { get; set; } //isko replace karna hai;
    }
}
