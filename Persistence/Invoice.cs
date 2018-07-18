using System;
using System.Collections.Generic;
namespace Persistence
{
    public class Invoice
    {
        public int invoiceID { get; set; }
        public Staff staff;
        public DateTime invoiceDate { get; set; }
        public decimal unitPrice { get; set; }
        public int amount { get; set; }
        
        // public int Invoice_Status { get; set; }

        public List<Item> ItemList { get; set; }
        
        
    }
}