using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Product
    {

        public int Quantity { get; set; }
        public DateOnly TransactionDate { get; set; }

        public string? centerName{ get; set; }
        public string ItemName { get; set; }
        public List<Items> items {  get; set; }
       public List<Warehouse> warehouse { get; set; }

       public List<Category> category {  get; set; }
       public Stock stock {  get; set; }
       public Order order {  get; set; }
    }
}
