using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace refactor_me.Models
{
    [Serializable()]
    public abstract class BusinessBase<T>
    {
        [JsonIgnore]
        public bool IsNew { get; set; }

        public abstract void Delete(); //all models inherits from base and all of then needs a delete function

        public abstract void Save();  //all models inherits from base and all of then needs a save function

        internal abstract void SetFieldsReader(ref SqlDataReader rdr);  //method to cater for the data reader field mapping

        internal abstract List<SqlParameter> PrepareCommandParameters();  //method to cater for the SQL Parameters

        internal void SelectData(StringBuilder qry, IEnumerable<SqlParameter> whereParameters = null) //method that selects data from database
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

                        SetFieldsReader(ref rdr);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
              throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        internal void InsertData(StringBuilder qry)//method that inserts data to database
        {
            try
            {
                using (var conn = Helpers.NewConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var cmd = Helpers.CreateSqlCommand(conn))
                    {
                        List<SqlParameter> parameters = PrepareCommandParameters();
                        foreach (var par in parameters)
                            cmd.Parameters.Add(par);

                        cmd.CommandText = qry.ToString();
                        cmd.ExecuteScalar();
                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
              throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        internal void UpdateData(StringBuilder qry) //method that update data to database
        {
            try
            {
                using (var conn = Helpers.NewConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var cmd = Helpers.CreateSqlCommand(conn))
                    {
                        cmd.Parameters.AddRange(PrepareCommandParameters().ToArray());
                        cmd.CommandText = qry.ToString();
                        object obj = cmd.ExecuteScalar();

                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
              throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
           
        }

        internal void DeleteData(StringBuilder qry) //method that update data from database
        {
            try
            {
                using (var conn = Helpers.NewConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    using (var cmd = Helpers.CreateSqlCommand(conn))
                    {
                        cmd.CommandText = qry.ToString();
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
              throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}