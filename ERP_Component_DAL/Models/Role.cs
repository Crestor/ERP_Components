using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Role
    {
        public int roleId { get; set; }
        public string role { get; set; } = string.Empty;
        public string homePage { get; set; } = string.Empty;
        public string controllerName { get; set; } = string.Empty;
    }
}
