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
        //TODO: Connect to the SQL Server.

        public SqlConnection Connection { get; set; }

        //public SQLConnect()
        //{
        //    Connection = new SqlConnection("Server=ServerName\\SQLEXPRESS; Database=BIBPic; Integrated Security=True");
        //    if (Connection.State == System.Data.ConnectionState.Closed)
        //    {
        //        Connection.Open();
        //    }
        //}

        //public void CloseConnection()
        //{
        //    if (Connection.State == System.Data.ConnectionState.Open)
        //    {
        //        Connection.Close();
        //    }
        //}
    }
}
