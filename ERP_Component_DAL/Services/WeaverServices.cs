using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ERP_Component_DAL.Services
{
    public class WeaverServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;


        public WeaverServices(IConfiguration config)
        {
            this.configuration = config;
        }

    }
}


        