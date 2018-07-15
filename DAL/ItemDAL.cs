using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;
namespace DAL
{
    public class  ItemDAL
    {
        private string query;
        MySqlConnection conn = null;
        
        private MySqlDataReader reader;
        public ItemDAL()
        {
            conn = DBsql.OpenConnection();
        }
        public Item GetItemById(string itemId)
        {
            if(conn.State == System.Data.ConnectionState.Closed){
                conn.Open();
            }
            Item item=null;
            query = @"select * from Item where ItemID = '" + itemId + "';";
            MySqlCommand command = new MySqlCommand(query,conn);
            reader = command.ExecuteReader();
            if(reader.Read())
            {
                item = GetItem(reader);
            }
            reader.Close();
            conn.Close();
            return item;
           
        }
        private Item GetItem(MySqlDataReader reader)
        {
            Item item = new Item();
            item.itemID = reader.GetString("ItemID");
            item.itemName = reader.GetString("ItemName");
            item.itemType = reader.GetString("ItemType");
            item.amount = reader.GetInt32("Amount");
            item.size = reader.GetString("Size");
            item.unitPrice = reader.GetDecimal("UnitPrice");
            item.Promotion = reader.GetString("Promotion");
            return item;
        }
        public List<Item> GetAllItem()
        {
            List<Item> lstitem = new List<Item>();
            string query = @"select * from Item;";
            if(conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            MySqlCommand command = new MySqlCommand(query,conn);
            using(reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        lstitem.Add(GetItem(reader));
                    }
                    reader.Close();
                }
                conn.Close();
            return lstitem;
        }


    }
}