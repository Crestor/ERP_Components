using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ERP_Component_DAL.Models
{
    public class Contact
    {
        public int CustomerId { get; set; }
        public string ContactNo { get; set; }
        public string AlterContactNo { get; set; }
        public string Email { get; set; }
        public string CompanyEmail { get; set; }
        public string Social { get; set; }
        public string Whatsapp { get; set; }
        public string CustomerName { get; set; }
    }
}
