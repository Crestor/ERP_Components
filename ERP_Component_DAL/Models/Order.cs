﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Component_DAL.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid itemId { get; set; }
        public Guid lotId { get; set; }
        public string? type { get; set; }
        public string? batchSeries { get; set; }
        public decimal sellingPrice { get; set; }
        public decimal totalPrice { get; set; }
        public Guid warehouseId { get; set; }
        public Guid sourceDc { get; set; }
        public Guid destinationDc { get; set; }
        public string? itemName {  get; set; }
        public string? reason {  get; set; }
        public string? warehouseName {  get; set; }
        public string? customerName {  get; set; }
        public decimal quantity { get; set; }
        public int available { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime expiry { get; set; }

        public List<Items> items { get; set; }
        public List<Warehouse> warehouse { get; set; }




        public string? Category { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDetail { get; set; }
        
        public int ProductQuantity { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal ProductGstAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal ProductGSTRate { get; set; }
    }
}
