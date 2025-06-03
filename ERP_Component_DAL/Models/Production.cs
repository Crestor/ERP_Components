using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Production
    {
        public Guid productionOrderId { get; set; }
        public Guid productId { get; set; }
        public Guid materialId { get; set; }
        public Guid salesForcastId { get; set; }
        public Guid RequisitionID { get; set; }
        public int quantity{ get; set; }
        public decimal quantityRequired{ get; set; }
        public int availableQuantity{ get; set; }
        public int productStatus{ get; set; }
        public int stage{ get; set; }
        public int stageTime{ get; set; }

        public string? productName { get; set; }
        public string? description { get; set; }
        public string? status { get; set; }
        public string? stageWork { get; set; }
        public string? materialName { get; set; }
        public string? unitOfMeasure { get; set; }

        public DateTime createdAt { get; set; }


        public List<Production> materials { get; set; }

        //public List<Production> materials { get; set; }

    }
}
