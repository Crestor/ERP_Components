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
    }
}
