using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class StockTransaction
    {
        public Guid TransactionID { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SourceDC { get; set; }
        public string DestinationDC { get; set; }
        public string? Reason { get; set; }
        public StockTransactionType Type { get; set; }
    }
}
