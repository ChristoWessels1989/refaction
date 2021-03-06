﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Web.Http;


namespace refactor_me.Models
{
  public abstract class BusinessListBase<T,C>
    where C : BusinessBase<C>
  {
        internal abstract void SetReaderValues(ref SqlDataReader rdr); //method to let model know what to do with data from reader

        public void SelectData(StringBuilder qry, IEnumerable<SqlParameter> whereParameters = null)
    {
        try
        {
            using (var conn = Helpers.NewConnection())
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                using (var cmd = Helpers.CreateSqlCommand(conn))
                {
                    if (whereParameters != null)
                    {
                        foreach (var item in whereParameters)
                            cmd.Parameters.AddWithValue(item.ParameterName, item.Value);
                    }
                    cmd.CommandText = qry.ToString();

                    var rdr = cmd.ExecuteReader();
                    if (!rdr.Read())
                        return;

                    SetReaderValues( ref rdr);
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
          throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }
    }//method that selects data from database
  }
}