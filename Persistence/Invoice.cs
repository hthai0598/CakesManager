using System;
using System.Collections.Generic;
namespace Persistence
{
    public class Invoice
    {
        public int invoiceID {get;set;}
        public Staff staff; 
        public DateTime invoiceDate {get;set;}
        public int amount {get;set;}
        public int Invoice_Status {get;set;}

        private List<Item> ItemList;
        public List<Item> itemList { get => itemList; set => itemList = value; }

        // public Invoice()
        // {
        //     staff = new Staff();
        //     //ItemList = new List<Item>();
        // }
    }
}