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
        ItemDAL ibl = new ItemDAL();
         // [Fact]
        // public void GetInvoiceDetailsid()
        // {
        //     Assert.NotNull(i.GetInvoiceDetailsByID(1));
        // }
            
            
        [Fact]
        public void Test_Create_Invoice()
        {    
            Invoice invo = new Invoice();
            invo.staff = new Staff();
            invo.staff.StaffID = "S111";
            invo.Invoice_Status = 1;
            invo.itemList = new List<Item>();
            Item it = new Item();
            it.itemID = "GT1";
            it.amount = 1;
            invo.itemList.Add(it);
            Assert.True(i.Create_Invoice(invo));


            // Orders or = new Orders();
            // or.user = new User();
            // or.Order_id = 1;
            // or.Order_status = 1;
            // or.user.User_id = 1;
            // or.shoesList = new List<Shoes>();
            // Shoes sh = new Shoes();
            // sh.Shoes_id = 1;
            // sh.Amount = 2;
            // or.shoesList.Add(sh);
            // Assert.True(odl.CreateOrders(or));
        }
       
        // [Fact]
        // public void TestGetInvoiceDetails()
        // {
           
        //     Assert.NotNull(i.GetInvoiceDetails());
        // }




        [Fact]
        public void Test_Update()
        {
            Assert.True(i.Update(1));

        }



        [Fact]
        public void GetAllOrder()
        {   
            List<Invoice> s  = i.GetInvoices(0,null);
            Assert.NotNull(s);
     
        }


         [Theory]
        [InlineData(1)]
        public void Test_GetAllOrderbyStatus(int Status)
        {
            var result = i.GetInvoices(2,new Invoice{Invoice_Status = Status});
            Assert.NotNull(result);
        }



        [Fact]
        public void GetINvoicebystaff()
        {
            var res = i.GetInvoices(1,new Invoice{staff = new Staff{StaffID = "S111"}});
            Assert.NotNull(res);    
      
        }
        
       
    }
}