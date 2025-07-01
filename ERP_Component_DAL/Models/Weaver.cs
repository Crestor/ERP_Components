using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
        public class Weaver
        {
            public Guid WeaverId { get; set; }

        public List<Weaver> Weavers { get; set; }

        public List<Weaver> Dyer { get; set; }
        public string? WeaverName { get; set; }

        public string? DeyerName { get; set; }
        public string? Firm { get; set; }

            public string? ContactNumber { get; set; }

            public string? Email { get; set; }

            public string? Address { get; set; }

            public string? City { get; set; }

            public string? District { get; set; }

            public string? State { get; set; }

            public string? Pincode { get; set; }

            public string? Country { get; set; }

            public string? AadharNumber { get; set; }

            public string? PANNumber { get; set; }

            public IFormFile? DocPANCard { get; set; }

            public IFormFile? DocAadhar { get; set; }

        public Guid WorkOrderId { get; set; }
        public string? WorkOrderSeries {  get; set; }

        public string? ProductName { get; set; }

        public Guid ProductId { get; set; }

        public string? Description { get; set; }

        public string? MaterialName { get; set; }

        public Guid MaterialId { get; set; }

        public int Quantity { get; set; }

        public string? Status { get; set; }

        public int requiredQuantity { get; set; }
        public int? availableQuantity { get; set; }

        public int? pendingQuantity { get; set; }
        public int? AllocatedQuantity { get; set; }
        public int? dyeingQuantity { get; set; }

       public string AllocationSeries { get; set; }
        public string? RequisitionSeries { get; set; }
        public string? Specification { get; set; }
         public int? PerPiecePrize { get; set; }
        public List<Weaver>? PendingWorkOrder { get; set; }
        public List<Weaver>? OngoingWorkOrder { get; set; }
        public List<Weaver>? CompletedWorkOrder { get; set; }
        public List<Weaver>? DispatchedWorkOrder { get; set; }
        public List<Weaver> MaterialRequired { get; set; }

        public List<WorkOrderPhases>? workOrderPhases { get; set; }
        public List<YarnInfo>? Yarns { get; set; }

        public class WorkOrderPhases
        {
            public byte phase { get; set; }
            public decimal phaseTime { get; set; }
            public string? phaseWork { get; set; }
            public string? status { get; set; }
        }


    }
    public class YarnInfo { 
        public Guid YranID { get; set; }
        public string? YarnName { get; set; }
        public string? Specification { get; set; }
    }

 }

