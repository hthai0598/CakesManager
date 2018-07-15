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
        public void TestGetInvoiceByStatus()
        {
            Assert.NotNull(inn.GetInvoiceByStatus(1));
        
        }
        [Fact]
        public void TestGetAll()
        {
            Assert.NotNull(inn.GetAllInvoice());
        }
        [Fact]
        public void TestUpdate()
        {
            Assert.True(inn.Update(1));
        }
        // [Fact]
        // public void TestGetInvoiceDeTailsById()
        // {
        //     Assert.NotNull(inn.GetInvoiceDetailsById(1));
        // }
    
       
        
        // [Fact]
        // public void Create()
        // {
        //     Assert.True(inn.Create_Invoice(new Invoice()));
        }
    }
}    