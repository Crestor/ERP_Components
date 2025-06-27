using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Account
    {
        public Guid AccountID { get; set; }
          public string  AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }

        public string IFSCCode { get; set; }


    }
}
