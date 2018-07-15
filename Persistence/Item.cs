using System;
using System.Collections.Generic;
namespace Persistence
{
    public class ItemStatus
    {
        public const int NOT_ACTIVE = 0;
        public const int ACTIVE = 1;
    }
    public class Item
    {
        public string itemID { get; set; }
        public string itemName { get; set; }
        public string size { get; set; }
        public string itemType { get; set; }
        public decimal unitPrice { get; set; }
        public int amount { get; set; }
        public string Promotion { get; set; }
        

    }
}