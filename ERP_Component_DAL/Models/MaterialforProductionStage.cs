using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class MaterialforProductionStage
    {
        public List<Item> materials { get; set; } = new List<Item>();

        public List<Item> products { get; set; } = new List<Item>();
    }
}
