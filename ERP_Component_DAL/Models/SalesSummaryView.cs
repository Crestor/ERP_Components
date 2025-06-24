using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class SalesSummaryView
    {
        public List<SalesSummary> Daily { get; set; }
        public List<SalesSummary> Weekly { get; set; }
        public List<SalesSummary> Monthly { get; set; }
        public List<SalesSummary> Quarterly { get; set; }
        public List<SalesSummary> Yearly { get; set; }
    }

}
