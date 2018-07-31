using System;
using Xunit;
using DAL;
using Persistence;
namespace DAL.Test
{
    public class TestDBsql
    {
        [Fact]
        public void Test_OpenConnection()
        {
            Assert.NotNull(DBsql.OpenConnection());
        //Given
        
        //When
        
        //Then
        }
        [Theory]
        [InlineData("server=localhost1;user id=vtca;password=vtcacademy;port=3306;database=OrderDB;SslMode=None")]
        public void OpenConnectionFail(string connectionString)
        {
            Assert.Null(DBsql.OpenConnection(connectionString));
        }
    }
}    