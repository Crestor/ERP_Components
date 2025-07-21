using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class BusinessSetUp
    { 
        public Guid CompanyID { get; set; }
        public string BussinessName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string AlternateMobile { get; set; }
        public Address address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string GstIn { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string TAN { get; set; }

    }
}
