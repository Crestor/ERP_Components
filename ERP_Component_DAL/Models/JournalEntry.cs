using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class JournalEntry
    {
        public Guid TransactionID { get; set; }
         public DateOnly   TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public string Remarks { get; set; }
        public string AccountID { get; set; }
        public string transactionType { get; set; }
    }
}
