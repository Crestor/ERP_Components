using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public enum CenterTypes
    {
       SALES_CENTER = 1, WAREHOUSE = 2, PRODUCTION_CENTER = 3, STORE = 4, RETAIL_STORE = 5, WEAVER = 6
    }
    public enum WorkOrderStatuses
    {
       PENDING = 1, UNDER_PROGRESS = 2,	RECEIVED = 3, SENT_FOR_MR = 4, COMPLETED = 5
    }
    public enum PurchaseOrderStatus
    {
        PENDING = 1, ITEMS_RECEIVED = 2, ADVANCED_PAID = 3, CONVERTED_TO_BILL = 4
    }
}
