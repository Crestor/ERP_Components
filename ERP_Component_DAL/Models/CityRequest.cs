using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class CityRequest
    {
        public int StateIndex { get; set; }
        public int DistrictIndex { get; set; }
        public string NewCity { get; set; }

        public string StateName { get; set; }
        public string DistrictName { get; set; }
    }
}
