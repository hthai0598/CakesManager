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
           Assert.NotNull(inn.GetInvoiceDetails());
        }
    
       
        
        // [Fact]
        // public void Create()
        // {
        //     Assert.True(inn.Create_Invoice(new Invoice()));
        // }
    }
}    