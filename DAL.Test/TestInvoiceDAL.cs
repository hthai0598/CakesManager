using System;
using Xunit;
using DAL;
using Persistence;
using System.Collections.Generic;
namespace DAL.Test

{
    public class TestInvoiceDAL
    {   

        
       InvoiceDAL i = new InvoiceDAL();
        // [Fact]
        // public void GetInvoiceDetailsid()
        // {   
        //     Assert.NotNull(i.GetInvoiceDetailsByID("S111"));
        // }

        
            
        [Fact]
        public void Test_Create_Invoice()
        {    
            ItemDAL idl = new ItemDAL();
            Invoice invo = new Invoice();
            // invo.staff = new Staff{StaffID = "S111"};
            // invo.ItemList = new List<Item>();
            // Item ite = new Item();
            // ite.itemID = "GT2";
            // ite.amount= 2;
            //  ite.itemID = "GT1";
            // ite.amount= 2;
            // invo.ItemList.Add(ite);

            invo.staff = new Staff{StaffID = "C111"};
            invo.ItemList = new List<Item>();
            invo.ItemList.Add(idl.GetItemById("BM1"));
            invo.ItemList[0].amount = 2;
            invo.ItemList.Add(idl.GetItemById("BM2"));
            invo.ItemList[1].amount = 2;

            Assert.True(i.Create_Invoice(invo));
        }
       
        [Fact]
        public void TestGetInvoiceDetails()
        {
           
            Assert.NotNull(i.GetInvoiceDetails(1));
        }




        // [Fact]
        // public void GetAllINvoice()
        // {   
        //     List<Invoice> s  = i.GetInvoices(0,null);
        //     Assert.NotNull(s);
    
        // }

        [Fact]
        public void GetINvoicebystaff()
        {
            var res = i.GetInvoices(1,new Invoice{staff = new Staff{StaffID = "S111"}});
            Assert.NotNull(res);    
      
        }
        
       
    }
}