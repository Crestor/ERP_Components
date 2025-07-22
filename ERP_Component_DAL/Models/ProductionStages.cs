using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
  
    public class OutputModel
    {
        public string OutputValue { get; set; }
    }

    public class StageModel
    {
        public string StageName { get; set; }
        public string WorkInput { get; set; }
        public List<OutputModel> Outputs { get; set; } = new List<OutputModel>();
    }

    public class StageFormModel
    {
        public List<StageModel> Stages { get; set; } = new List<StageModel>();
    }

}
