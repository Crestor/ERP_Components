using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class SubCategory
    {
        public int subCategoryId { get; set; }
        public string? subCategoryName { get; set; }
        public string? categoryDescription { get; set; }
        public int categoryId { get; set; }        
        
    }
}
