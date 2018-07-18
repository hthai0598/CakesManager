using System;
using Persistence;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DAL
{
    public class InvoiceDAL
    {
        private string query;
        private MySqlConnection conn;
        private MySqlDataReader reader;
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
            Invoice i = new Invoice();
            // inn.invoiceID = reader.GetInt32("InvoiceID");
            // inn.staff = new Staff();
            // inn.staff.StaffID = reader.GetString("StaffID");
            // inn.invoiceDate = reader.GetDateTime("InvoiceDate");
                    i.invoiceID = reader.GetInt32("InvoiceID");
                    i.invoiceDate = reader.GetDateTime("InvoiceDate");
                    i.staff = new Staff();
                    i.staff.StaffID = reader.GetString("StaffID");
                    Item item = new Item();
                    item.itemID = reader.GetString("ItemID");
                    // item.itemName = reader.GetString("ItemName");
                    // item.itemType = reader.GetString("ItemType");
                    item.amount = reader.GetInt32("Amount");
                    // item.size = reader.GetString("Size");
                    item.unitPrice = reader.GetDecimal("UnitPrice");
                    item.total = item.amount * item.unitPrice;

                    // item.Promotion = reader.GetString("Promotion");
            return i;
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
                    query = "select * from invoice inner join invoicedetails on invoice.InvoiceID = invoicedetails . InvoiceID ;";
                    break;
                case 1:
                    query = @"select * from invoice inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID where StaffID = @staffid";

                    cmd.Parameters.AddWithValue("@staffid", i.staff.StaffID);
                    break;
              
            }
            cmd.CommandText = query;

            return GetInvoices(cmd);
        }
       
       
        public Invoice GetInvoiceDetails()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            Invoice invo = new Invoice();
            invo.ItemList = new List<Item>();
            string query= "select staff.StaffID,staff.StaffName,Item.Size,Item.Promotion, invoice.InvoiceDate,invoice.InvoiceID,invoicedetails.Amount,item.ItemName,item.ItemID,item.UnitPrice from staff inner join invoice on staff.StaffID = invoice.StaffID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join item on item.ItemID = invoicedetails.ItemID ;";
            MySqlCommand command = new MySqlCommand(query,conn);
            reader = command.ExecuteReader();
            while(reader.Read())
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
                u.total = u.amount * u.unitPrice;
                
                invo.ItemList.Add(u);
           
            }
            reader.Close();
            conn.Close();
            return invo;
        }
        // public List<Item> GetDetails()
        // {
        //     List<Item> list = new List<Item>();
        //     using (conn = DBsql.OpenConnection())
        //     {
        //         MySqlCommand command = new MySqlCommand(query,conn);
        //         using (reader = command.ExecuteReader())
        //         {
        //             while (reader.Read())
        //             {
        //                 Invoice invoice = new Invoice();
        //                 invoice = GetInvoice(reader);
        //                 list.Add(invoice);

        //             }

        //         }
                
        //     }
        //     return list;
        // }



    }


}
