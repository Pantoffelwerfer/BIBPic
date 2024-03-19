using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BIBPic.Model
{
    internal class SQLConnect
    {
        public void SQLConnection()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = "Server=server_name;" +
                                              "Database=database_name;" +
                                              "User ID=user_name;" +
                                              "Password=password;" +
                                              "Trusted_Connection=False;";
                connection.Open();
            }
        }
    }
}
