using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class SalesGrowth
    {

        public string Period { get; set; }
        public decimal TotalSales { get; set; }
        public decimal? PreviousSales { get; set; }
        public decimal? GrowthPercentage { get; set; }
    }
}
