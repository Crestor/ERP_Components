﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class User
    {
        public int userId {  get; set; }
        public string? userName { get; set; }

        public string? password { get; set; }

        public int role {  get; set; }
    }
}
