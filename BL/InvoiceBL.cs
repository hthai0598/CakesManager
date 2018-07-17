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
            
            return inv.Create_Invoice(invoice);
            
        }
        public Invoice GetInvoiceDetailsByID(int id)
        {
            return inv.GetInvoiceDetailsByID(id);
        }
        public List<Invoice> GetInvoiceByID(string id)
        {
            return inv.GetInvoices(1,new Invoice{staff = new Staff{StaffID = id }});
        }
        public Invoice GetInvoiceDetails()
        {
            return inv.GetInvoiceDetails();
        }
        
        public List<Invoice> GetAllInvoice()
        {
            return inv.GetInvoices(0,null);
        }  
       
    }

}
