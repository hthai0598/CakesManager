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
            // lock (this)
            // {
            if (invoice == null || invoice.itemList == null || invoice.itemList.Count == 0)
            {
                return false;
            }

            //}
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
            reader = null;
            try
            {
                // them bang invoice
                command.CommandText = "insert into Invoice(StaffID,InvoiceStatus) value (@staffid,@status);";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@staffid", invoice.staff.StaffID);
                command.Parameters.AddWithValue("@status", invoice.Invoice_Status);
                command.ExecuteNonQuery();

                command.CommandText = "select LAST_INSERT_ID() as InvoiceID;";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    invoice.invoiceID = reader.GetInt32("InvoiceID");
                }
                reader.Close();

                // them bang invoicedetails
                foreach (var s in invoice.itemList)
                {
                    if (s.itemID == null || s.amount <= 0)
                    {
                        throw new Exception("Not exists Item");
                    }
                    command.CommandText = "select UnitPrice from Item where ItemID = @itemid;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@itemid", s.itemID);
                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        throw new Exception("not exists Item");
                    }
                    s.unitPrice = reader.GetDecimal("UnitPrice");
                    reader.Close();

                    // them invoicedetails
                    command.CommandText = "insert into InvoiceDetails(InvoiceID,ItemID,Amount,UnitPrice,InvoiceStatus) value (" + invoice.invoiceID + "," + s.itemID + "," + s.amount + "," + s.unitPrice + "," + invoice.Invoice_Status + ");";
                    command.ExecuteNonQuery();
                    //update so luong
                    command.CommandText = "update Item set Amount = Amount - @Quantity where ItemID =" + s.itemID + ";";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Quantity", s.amount);
                    command.ExecuteNonQuery();
                }
                tran.Commit();
                result = true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
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
            inn.Invoice_Status = reader.GetInt32("InvoiceStatus");
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
                case 2:
                    query = @"select *from Invoice where  InvoiceStatus = @status;";
                    cmd.Parameters.AddWithValue("@status", i.Invoice_Status);
                    break;
            }
            cmd.CommandText = query;

            return GetInvoices(cmd);
        }
        public bool Update(int invoiceid)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            Invoice order = new Invoice();
            bool a = true;
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "lock tables Staff write , Invoice write, Item write, InvoiceDetails write;";
            cmd.ExecuteNonQuery();
            MySqlTransaction trans = conn.BeginTransaction();
            cmd.Transaction = trans;
            try
            {
                cmd.CommandText = "update Invoice set InvoiceStatus = 0 where  InvoiceID = @invoice_id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@invoice_id", invoiceid);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update InvoiceDetails set InvoiceStatus = 0 where InvoiceID = @invoice_id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@invoice_id", invoiceid);
                cmd.ExecuteNonQuery();
                trans.Commit();
                a = true;


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                a = false;
                try
                {
                    trans.Rollback();
                }
                catch { }
            }
            finally
            {
                cmd.CommandText = "unlock tables;";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return a;
        }
        public Invoice GetInvoiceDetailsByID(int id)
        {
            DBsql.OpenConnection();
            // if (conn.State == System.Data.ConnectionState.Closed)
            // {
            //     DBsql.OpenConnection();
            //     //conn.Open();
            // }
            string query = "select staff.StaffID,staff.StaffName, invoice.InvoiceID,invoicedetails.Amount,invoicedetails.UnitPrice,item.ItemName,item.ItemID,item.UnitPrice,item.Amount,item.Size,item.Promotion,item.ItemType from staff inner join invoice on staff.StaffID = invoice.StaffID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join item on item.ItemID = invoicedetails.ItemID where invoice.InvoiceID =" + id;
            Invoice i = new Invoice();
            i.itemList = new List<Item>();
            MySqlCommand command = new MySqlCommand(query, conn);
            using (reader = command.ExecuteReader())
            {
                
                if (reader.Read())
                {
                    i.invoiceID = reader.GetInt32("InvoiceID");
                    i.invoiceDate = reader.GetDateTime("InvoiceDate");
                    i.Invoice_Status = reader.GetInt32("InvoiceStatus");
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
                    i.itemList.Add(item);
                }

                reader.Close();
            }


            conn.Close();
            return i;
        }


        // public Invoice GetInvoiceDetails()
        // {
        //     if (conn.State == System.Data.ConnectionState.Closed)
        //     {
        //         conn.Open();
        //     }
        //     Invoice invo = new Invoice();
        //     invo.itemList = new List<Item>();
        //     string query="select staff.StaffID,staff.StaffName,invoice.InvoiceDate,item.ItemName,item.ItemID,item.Size,item.Promotion,invoicedetails.Amount,invoicedetails.UnitPrice,invoice.InvoiceStatus from staff inner join invoice on staff.StaffID = invoice.InvoiceID inner join invoicedetails on invoice.InvoiceID = invoicedetails.InvoiceID inner join  item on item.ItemID = invoicedetails.InvoiceID order by invoice.InvoiceID limit 1;";
        //     MySqlCommand command = new MySqlCommand(query,conn);
        //     reader = command.ExecuteReader();
        //     if (reader.Read())
        //     {
        //         invo.invoiceID = reader.GetInt32("InvoiceID");
        //         invo.invoiceDate = reader.GetDateTime("InvoiceDate");
        //         invo.Invoice_Status = reader.GetInt32("InvoiceStatus");
        //         invo.staff = new Staff();
        //         invo.staff.StaffID = reader.GetString("StaffID");
        //         invo.staff.StaffName = reader.GetString("StaffName");
        //         Item u = new Item();
        //         u.itemID = reader.GetString("ItemID");
        //         u.itemName = reader.GetString("ItemName");
        //         u.size = reader.GetString("ItemSize");
        //         u.Promotion = reader.GetString("Promotion");
        //         u.amount = reader.GetInt32("Amount");
        //         u.unitPrice = reader.GetDecimal("UnitPrice");
        //         invo.itemList.Add(u);
        //     }
        //     reader.Close();
        //     conn.Close();
        //     return invo;
        // }



    }


}
