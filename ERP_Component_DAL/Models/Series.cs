using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Series
    {

        public string? Series_Type { get; set; }

        public string? Prifix { get; set; }

        public int? StartFrom { get; set; }
        public string? CurrentSeries { get; set; }
    }
}
