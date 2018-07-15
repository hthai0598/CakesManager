using System;
using System.Collections.Generic;
using DAL;
using Persistence;

namespace BL
{
    public class InvoiceBL
    {
         InvoiceDAL inv = new InvoiceDAL();
        public bool Create_Invoice(Invoice invoice)
        {
            InvoiceDAL inv = new InvoiceDAL();
            bool result = inv.Create_Invoice(invoice);
            return result;
        }
        public Invoice GetInvoiceDetailsByID(int id)
        {
            return inv.GetInvoiceDetailsByID(id);
        }
        public List<Invoice> GetInvoiceByID(string id)
        {
            return inv.GetInvoices(1,new Invoice{staff = new Staff{StaffID = id }});
        }
        public List<Invoice> GetInvoiceByStatus(int status)
        {
            return inv.GetInvoices(2,new Invoice{Invoice_Status = status});
        }
        public List<Invoice> GetAllInvoice()
        {
            return inv.GetInvoices(0,null);
        }
        public bool Update(int id)
        {
            
            return (inv.Update(id));
        }
       
    }

}
