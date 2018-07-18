using System;
using Xunit;
using BL;
using MySql.Data.MySqlClient;
using Persistence;
using System.Collections.Generic;
namespace BL.Test
{
    public class TestInvoiceBL
    {
        InvoiceBL inn = new InvoiceBL();
    
        [Fact]
        public void TestGetAll()
        {
            Assert.NotNull(inn.GetAllInvoice());
        }
        [Fact]
        public void TestGetInvoiceDeTails()
        {
            int id = 2;
           Assert.NotNull(inn.GetInvoiceDetails(id));
        }
        [Fact]
        public void TestGetInvoiceDeTailsbyid()
        {
            Assert.NotNull(inn.GetInvoiceByID("S111"));   
        }
    
       
        
        // [Fact]
        // public void Create()
        // {
        //     Assert.True(inn.Create_Invoice(new Invoice()));
        // }
    }
}    