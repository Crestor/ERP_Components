using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
   public class MakePayment
    {
        public decimal TotalAmountPaid { get; set; }
        public decimal paymentAmount { get; set; }
       public decimal AdvanceAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public string BillNumber { get; set; }
        public string Description { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal RemainingNetAmount { get; set; }
        public decimal finalPaidAmount { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TotalAmount { get; set; }
        //public decimal PaidAmount { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public string VendorName { get; set; }
      public Guid  VendorID { get; set; }
       public decimal Balance { get; set; }
       public decimal TaxableAmount { get; set; }
          public decimal NetTotal { get; set; }
        public decimal PaidAmount { get; set; }
       public DateOnly DueDate { get; set; }
        public List<MakePayment> VendorNameList { get; set; }
        public List<MakePayment> VendorAmount { get; set; }
        public List<MakePayment> PaymentSummary { get; set; }
    }
}
