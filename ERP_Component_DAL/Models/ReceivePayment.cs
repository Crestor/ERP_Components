using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class ReceivePayment
    {
       public Guid InvoiceID { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Newpayment { get; set; }
        public decimal OldGrossTotal { get; set; }
        public decimal NewGrossTotal { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal GrossTotal { get; set; }
        public Guid QuotationID { get; set; }
       public Guid  CustomerID { get; set; }
        public decimal Balance { get; set; }
       public string CustomerName { get; set; }
       public List<ReceivePayment> CustomerNameList { get; set; }
        public List<ReceivePayment> ListOfCustomerAmount { get; set; }
    }
}
