using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{   
    /*----Use these models----*/
    public class StageMaterial
    {
        public decimal? quantity { get; set; }
        public Guid? materialId { get; set; }
        public string? materialName { get; set; }
        public string? specification { get; set; }
        public string? uom { get; set; }
        public byte materialType { get; set; } 
    }
    public class InputMaterial: StageMaterial
    {
        public InputMaterial()
        {
            this.materialType = 1;
        }
    }
    public class OutputMaterial : StageMaterial
    {
        public OutputMaterial()
        {
            this.materialType = 2;
        }
    }

    /// <summary>
    /// Use this one as a main model
    /// </summary>
    public class Stage
    {
        public Stage()
        {
            this.inputMaterial = new InputMaterial();
            this.outputMaterial = new List<OutputMaterial>();
        }

        public byte stage { get; set; } // 1, 2, 3, 4
        public string? stageWork { get; set; }
        public int stageTime { get; set; }    
        public InputMaterial? inputMaterial { get; set; }
        public List<OutputMaterial>? outputMaterial { get; set; }

    }

}
