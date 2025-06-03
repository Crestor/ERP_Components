using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Category
    {
        public string? categoryName { get; set; }
        public string? subCategoryName { get; set; }

        public string? categoryCode {  get; set; }

        public string? categoryDescription { get; set; }

        public int categoryId { get; set; }
        public Guid userId { get; set; }
        public int SubCategoryId { get; set; }
        public  string isActive { get; set; }

        public string formType { get; set; }
        public string userName { get; set; }
        public DateTime createdOn { get; set; }

        public List<Category> names { get; set; }
        public List<Category> list { get; set; }

    }
}
