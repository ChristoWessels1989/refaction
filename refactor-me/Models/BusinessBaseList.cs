using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;


namespace refactor_me.Models
{
  public abstract class BusinessListBase<T,C>
    where C : BusinessBase<C>
  {
        internal abstract void SetReaderValues(ref SqlDataReader rdr);

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
            throw; //Exception handling
        }
    }
  }
}