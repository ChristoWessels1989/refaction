using System;
using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models
{
    public static class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";
        private static int _sqlCommandTimeout = 1800;

        public static int SqlCommandTimeout
        {
          get
          {
            return _sqlCommandTimeout;
          }
          set
          {
            _sqlCommandTimeout = Math.Max(0, value);
          }
        }
        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }

        public static SqlCommand CreateSqlCommand(SqlConnection conn, int timeout = -1)
        {
          if (timeout == -1)
          {
            timeout = SqlCommandTimeout;
          }

          SqlCommand cmd = new SqlCommand();
          cmd.Connection = conn;
          //cmd.CommandTimeout = 0; //forever
          cmd.CommandTimeout = timeout;// 1800; //30 minutes
          return cmd;
        }
    }
}