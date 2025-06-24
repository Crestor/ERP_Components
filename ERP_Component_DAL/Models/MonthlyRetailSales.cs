using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class MonthlyRetailSales
    {

    
            public string StoreName { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal May { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Aug { get; set; }
            public decimal Sep { get; set; }
            public decimal Oct { get; set; }
            public decimal Nov { get; set; }
            public decimal Dec { get; set; }
            public decimal TotalStore { get; set; }

     
        public List<MonthlyRetailSales> Items { get; set; }

          public Guid RetailId { get; set; }
            public string CustomerName { get; set; }
            public string ContactNumber { get; set; }
            public decimal GrossTotal { get; set; }
            public DateTime CreatedAt { get; set; }

        public int centerType { get; set; }


        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
      

        public int BillCount { get; set; }


        public bool IsStore { get; set; } 



    }



}

