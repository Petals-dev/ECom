using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZohoIntegration
{
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        
        public string ItemType { get; set; }
        public string Status { get; set; }
        public string AvailableStock { get; set; }

        public string SKU { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string SellingPrice { get; set; }
        public string StockOnHand { get; set; }
    }
}