using System;
using Demo.Model;
using Demo.Data.Core;

namespace Demo
{
    class Program
    {
        private const string SqlConnectionStr = @"Data Source=ILOVEZCD\ilovezcd;Initial Catalog=Test;User ID=sa;Password=xiaobendan";

        static void Main(string[] args)
        {
            
            DbHelper.Init(SqlConnectionStr, Data.Config.DatabaseType.MicrosoftSql);
            DbHelper.OpenDataBase();



            var users = DbHelper.ReadEntitys<User>("Select * From [User]");

            foreach (var user in users) Console.WriteLine(user.UserName);

            Console.ReadKey();



            DbHelper.Dispose();

        }
    }
}
