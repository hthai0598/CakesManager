using System;
using Xunit;
using Persistence;

namespace BL.Test
{
    public class TestStaffBL
    {
        StaffBL staffbl = new StaffBL();
        [Fact]
        public void TestStaff()
        {
            Assert.NotNull(staffbl. Login("S111", "123"));
        }
         [Fact]
        public void TestStaff1()
        {
            Assert.NotNull(staffbl. Login("C111", "456"));
        }
        [Fact]
        public void TestFail()
        {
            Assert.Null(staffbl.Login("123","122"));
        }
    }
}
