using System;
using Persistence;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DAL
{
    public class InvoiceDAL
    {
        string query;
        MySqlConnection conn;
        MySqlDataReader reader;
        Staff staff = new Staff();
        public InvoiceDAL()
        {
            conn = DBsql.OpenConnection();
        }
        public bool Create_Invoice(Invoice invoice)
        {
            // System.Console.WriteLine("Begin Create Invoice");
            if (invoice.ItemList == null || invoice.ItemList.Count == 0)
            {
                return false;
            }
            bool result = true;
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            MySqlCommand command = conn.CreateCommand();
            command.Connection = conn;
            command.CommandText = "lock tables Staff write , Invoice write, Item write, InvoiceDetails write;";
            command.ExecuteNonQuery();
            MySqlTransaction tran = conn.BeginTransaction();
            command.Transaction = tran;
            reader = null;
            try
            {
                // them bang invoice
                command.CommandText = "insert into Invoice(StaffID) value ('"+ invoice.staff.StaffID+ "');";
               
                command.ExecuteNonQuery();
                command.CommandText = "select LAST_INSERT_ID() as InvoiceID;";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    invoice.invoiceID = reader.GetInt32("InvoiceID");
                }
                reader.Close();

                // // them bang invoicedetails
                foreach (var s in invoice.ItemList)
                {
                    command.CommandText = "select UnitPrice from Item where ItemID = @ItemID;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemID", s.itemID);
                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        throw new Exception("not exists Item");
                    }
                    s.unitPrice = reader.GetDecimal("UnitPrice");
                    reader.Close();

                    // them invoicedetails
                    command.CommandText = "insert into InvoiceDetails(InvoiceID,ItemID,Amount,UnitPrice) value (" + 
                            invoice.invoiceID + ",'" + s.itemID + "'," + s.amount + "," + s.unitPrice  + ");";
                    command.ExecuteNonQuery();
                    // update so luong
                    command.CommandText = "update Item set Amount = Amount - @Amount where ItemID ='" + s.itemID + "';";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Amount", s.amount);
                    command.ExecuteNonQuery();
                }
                tran.Commit();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // StackTrace
                try
                {

                    tran.Rollback();
                }
                catch
                {
                }

            }
            finally
            {
                // mo all bang
                command.CommandText = "unlock table;";
                command.ExecuteNonQuery();
                conn.Close();
            }
            return result;
        }

        
        private Invoice GetInvoice(MySqlDataReader reader)
        {
            Invoice inn = new Invoice();
            inn.invoiceID = reader.GetInt32("InvoiceID");
            inn.staff = new Staff();
            inn.staff.StaffID = reader.GetString("StaffID");
            inn.invoiceDate = reader.GetDateTime("InvoiceDate");
            return inn;
        }
        public List<Invoice> GetInvoices(MySqlCommand command)
        {
            List<Invoice> list = new List<Invoice>();
            reader = command.ExecuteReader();
            while (reader.Read())
            {

                list.Add(GetInvoice(reader));
            }
            reader.Close();
            conn.Close();
            return list;

        }
        public List<Invoice> GetInvoices(int Filter, Invoice i)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            MySqlCommand cmd = new MySqlCommand(" ", conn);
            switch (Filter)
            {
                case 0:
                    query = @"select * from Invoice;";
                    break;
                case 1:
                    query = @"select *from Invoice where StaffID = @staffid;";

                    cmd.Parameters.AddWithValue("@staffid", i.staff.StaffID);
                    break;
              
            }
            cmd.CommandText = query;

            return GetInvoices(cmd);
        }
       
        public Invoice GetInvoiceDetailsByID(int id)
        {
            //DBsql.OpenConnection();
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            string query = "select staff.StaffID,staff.StaffName, invoice.InvoiceID,invoicedetails.Amount,invoicedetails.UnitPrice,item.ItemName,item.ItemID,item.UnitPrice,item.Amount,item.Size,item.Promotion,item.ItemType from staff inner join invoice on staff.StaffID = invoice.StaffID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join item on item.ItemID = invoicedetails.ItemID where invoice.InvoiceID =" + id;
            Invoice i = new Invoice();
            
            MySqlCommand command = new MySqlCommand(query, conn);
            using (reader = command.ExecuteReader())
            {     
                i.ItemList = new List<Item>();
                while (reader.Read())
                {
                    
                    i.invoiceID = reader.GetInt32("InvoiceID");
                    i.invoiceDate = reader.GetDateTime("InvoiceDate");
                    i.staff = new Staff();
                    i.staff.StaffID = reader.GetString("StaffID");
                    i.staff.StaffName = reader.GetString("StaffName");

                    Item item = new Item();
                    item.itemID = reader.GetString("ItemID");
                    item.itemName = reader.GetString("ItemName");
                    item.itemType = reader.GetString("ItemType");
                    item.amount = reader.GetInt32("Amount");
                    item.size = reader.GetString("Size");
                    item.unitPrice = reader.GetDecimal("UnitPrice");
                    item.Promotion = reader.GetString("Promotion");
                    i.ItemList.Add(item);
                } 
                reader.Close();
            }
            conn.Close();
            return i;
        }
        public Invoice GetInvoiceDetails()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            Invoice invo = new Invoice();
            invo.ItemList = new List<Item>();
            string query="select staff.StaffID,staff.StaffName,Item.Size,Item.Promotion, invoice.InvoiceDate,invoice.InvoiceID,invoicedetails.Amount,invoicedetails.UnitPrice,item.ItemName,item.ItemID,item.UnitPrice from staff inner join invoice on staff.StaffID = invoice.StaffID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join item on item.ItemID = invoicedetails.ItemID order by invoice.InvoiceID asc;";
            MySqlCommand command = new MySqlCommand(query,conn);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                invo.invoiceID = reader.GetInt32("InvoiceID");
                invo.invoiceDate = reader.GetDateTime("InvoiceDate");
                invo.staff = new Staff();
                invo.staff.StaffID = reader.GetString("StaffID");
                invo.staff.StaffName = reader.GetString("StaffName");
                Item u = new Item();
                u.itemID = reader.GetString("ItemID");
                u.itemName = reader.GetString("ItemName");
                u.size = reader.GetString("Size");
                u.Promotion = reader.GetString("Promotion");
                u.amount = reader.GetInt32("Amount");
                u.unitPrice = reader.GetDecimal("UnitPrice");
                invo.ItemList.Add(u);
            }
            reader.Close();
            conn.Close();
            return invo;
        }



    }


}
