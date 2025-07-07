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
    public enum RequisitionTypes
    {
        SALES_FORCASTING = 1,
        MATERIAL_REQUISITION = 2,
        WEAVER_MATERIAL_REQUISITION = 3,
        SALES_FORECAST_RETAIL_STORE = 4
    }
    public enum StockTransactionType
    {
        IN_STOCK = 1,
        OUT_STOCK = 2,
        TRANSFER = 3
    }
    public enum TransactionType
    {
        DEBIT = 1,
        CREDIT = 2
    }
    public enum RequisitionStatus
    {
        PENDING	= 1,
        CLOSED = 4,
        DISPATCHED_TO_SALES	= 5,
        REJECTED = 6,
        SENT_FOR_PURCHASE_REQUISITION = 7
    }
}
