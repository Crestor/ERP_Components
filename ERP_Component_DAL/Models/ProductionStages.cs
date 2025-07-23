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
    /*----Use these models----*/
    public class StageMaterial
    {
        public string? quantity { get; set; }
        public Guid? materialId { get; set; }
    }
    public class InputMaterial: StageMaterial
    {
        public byte materialType() => 1; //Input
    }
    public class OutputMaterial : StageMaterial
    {
        public byte materialType() => 2; //Output
    }

    /// <summary>
    /// Use this one as a main model
    /// </summary>
    public class Stage
    {   
        public byte stage { get; set; } // 1, 2, 3, 4
        public string? stageWork { get; set; }
        public InputMaterial? inputMaterial { get; set; }
        public List<OutputMaterial>? outputMaterial { get; set; }

    }

}
