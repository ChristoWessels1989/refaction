using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace refactor_me.Models
{
  [Serializable()]
  public abstract class BusinessBase<T>
  {
    
    public abstract void Delete();

    public abstract void Save();

    internal abstract void SetFieldsReader(SqlDataReader rdr);

    internal abstract List<SqlParameter> PrepareCommandParameters();

    public void SelectData(StringBuilder qry, IEnumerable<SqlParameter> whereParameters = null)
    {
      using (var conn = Helpers.NewConnection())
      {
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

          SetFieldsReader(rdr);
        }
      }
    }

    internal void InsertData(StringBuilder qry)
    {
      try
      {
        using (var conn = Helpers.NewConnection())
        {
          using (var cmd = Helpers.CreateSqlCommand(conn))
          {
            List<SqlParameter> parameters = PrepareCommandParameters();
            foreach (var par in parameters)
              cmd.Parameters.Add(par);

            cmd.CommandText = qry.ToString();
            cmd.ExecuteScalar();
          }
        }
      }
      catch (Exception ex)
      {
        throw; //Exception handling
      }
    }

    internal void UpdateData(StringBuilder qry)
    {
      try
      {
        using (var conn = Helpers.NewConnection())
        {
          using (var cmd = Helpers.CreateSqlCommand(conn))
          {
            cmd.Parameters.AddRange(PrepareCommandParameters().ToArray());
            cmd.CommandText = qry.ToString();
            object obj = cmd.ExecuteScalar();
             
          }
        }
      }
      catch (Exception ex)
      {
         throw; //Exception handling
      }
    }

    internal void DeleteData(StringBuilder qry)
    {
      try
      {
        using (var conn = Helpers.NewConnection())
        {
          using (var cmd = Helpers.CreateSqlCommand(conn))
          {
            cmd.CommandText = qry.ToString();
            cmd.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex)
      {
        throw; //Error Handling
      }
    }
  }
}