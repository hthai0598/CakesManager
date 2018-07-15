using System;
using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class ItemBL
    {
        private ItemDAL idal = new ItemDAL();
        public Item GetItemById(string itemId)
        {
            return idal.GetItemById(itemId);
        }
        public List<Item> GetAllItem()
        {   
            
            return idal.GetAllItem();
        }
    } 
}