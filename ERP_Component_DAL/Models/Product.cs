﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Product
    {
   

       public List<Warehouse> warehouse { get; set; }

       public List<Category> category {  get; set; }
    }
}
