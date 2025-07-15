using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Accounthead
    {
        public string AccountHeadName { get; set; }

        public string AccountNumber { get; set; }

        public string IFSC { get; set; }

        public decimal CurrentBalance { get; set; }
    }
}
