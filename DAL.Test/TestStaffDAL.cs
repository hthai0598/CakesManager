using System;
using Xunit;
using DAL;
using Persistence;
namespace DAL.Test
{
    public class TestStaffDAL
    {
        staffDAL StaffDAL = new staffDAL();
        [Fact]
        public void Test_Login1()
        {   
            Staff staff = StaffDAL.Staff_Login("S111","123");
            Assert.NotNull(staff);
            // string username = "S111";
            // string pass = "123";
            // Staff staff = StaffDAL.Staff_Login(username,pass);
            // Assert.NotNull(staff);
            // Assert.Equal(username,staff.StaffID);
            // Assert.Equal(pass,staff.Staffpass);
            
        }
        // [Fact]
        // public void Test_Login2()
        // {
        //     string username = "C111";
        //     string pass = "456";
        //     Staff staff = StaffDAL.Staff_Login(username,pass);
        //     Assert.NotNull(staff);
            
        //     Assert.Equal(username,staff.StaffID);
        //     Assert.Equal(pass,staff.Staffpass);
        // // Given
        
        // // When
        
        // // Then
        // }
        [Fact]
        public void Test_LoginFail()
        {
            Staff staff = StaffDAL.Staff_Login("aa","12");
            Assert.Null(staff);
        //Given
        
        //When
        
        //Then
        }
    }
}

