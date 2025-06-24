using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class SalesSummary
    {
     
   

        public List<string> Periods { get; set; } = new();
        public List<DynamicRetailSales> Data { get; set; } = new();

     
       
    }


    public class SalesRow
    {
        public string StoreName { get; set; }
        public Dictionary<string, decimal> SalesByPeriod { get; set; } = new Dictionary<string, decimal>();
    }




}

