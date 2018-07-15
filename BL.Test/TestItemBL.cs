using System;
using Xunit;
using Persistence;
using System.Collections.Generic;

namespace BL.Test
{
    public class TestItemBL
    {
        ItemBL itembl = new ItemBL();
        [Fact]
        public void Test_ItemBL()
        {
            Assert.NotNull(itembl.GetItemById("GT1"));
        }
        [Fact]
        public void TestFailItemBL()
        {
            Assert.Null(itembl.GetItemById("123"));
        }
        [Fact]
        public void TestGetAll()
        {   
            List<Item> i = new List<Item>();
            Assert.NotNull(i);
        }
    }
}