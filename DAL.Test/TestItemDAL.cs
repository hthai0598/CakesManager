// using System;
// using Xunit;
// using DAL;
// using Persistence;
// using System.Collections.Generic;
// namespace DAL.Test
// {
//     public class TestItemDAL
//     {   
//         ItemDAL item = new ItemDAL();
//         [Fact]
//         public void TestGetItem()
//         {
//             Assert.NotNull(item.GetItemById("GT1"));
//         }
//         [Fact]
//         public void TestFail()
//         {
//             Assert.Null(item.GetItemById("123"));
//         }
//         [Fact]
//         public void TestGetAll()
//         {
//             List<Item> i = item.GetAllItem();
//             Assert.NotNull(i);
            
//         //Given
        
//         //When
        
//         //Then
//         }
//     }
// }