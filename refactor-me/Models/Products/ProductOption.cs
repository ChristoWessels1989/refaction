using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;


namespace refactor_me.Models
{
  public class ProductOption:BusinessBase<ProductOption>
  {
    #region Properties
      public Guid Id { get; set; }

      public Guid ProductId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      [JsonIgnore]
      public bool IsNew { get; set; }
    #endregion Properties

    #region constructors
      public ProductOption()
      {
          Id = Guid.NewGuid();
          IsNew = true;
      }

      public ProductOption(Guid id)
      {
         IsNew = true;
          var conn = Helpers.NewConnection();
          var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
          conn.Open();

          var rdr = cmd.ExecuteReader();
          if (!rdr.Read())
              return;

          IsNew = false;
          Id = Guid.Parse(rdr["Id"].ToString());
          ProductId = Guid.Parse(rdr["ProductId"].ToString());
          Name = rdr["Name"].ToString();
          Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
         FetchProductOption(id);
      }
    #endregion constructor

    #region overrides
      public override void Save()
      {
        if (IsNew)
        {
          InsertProductOption();
        }
        else
        {
          UpdateProductOption();
        }
      }

      public override void Delete()
      {
        DeleteProductOption();
      }
    #endregion overrides

    #region Data Portal
      #region Select
        private void FetchProductOption(Guid id)
        {
           var qry = new StringBuilder();
           qry.Append("SELECT * ");
          qry.Append(@"FROM [productoption] ");
          qry.Append(" WHERE id = @0 ");
          var whereParameters = new List<SqlParameter> {new SqlParameter("@0", id)};

          SelectData(qry, whereParameters);
        }
      #endregion Select

      #region Insert
        private void InsertProductOption()
        {
          var qry = new StringBuilder();
          qry.Append("INSERT INTO [productoption]");
          qry.Append(@"( (id,productid, name, description)");
          qry.Append(" VALUES ");
          qry.Append(@" ('@Id', '@productid', '@name', @description)");

          InsertData(qry);
        }
      #endregion Insert

      #region Update
        private void UpdateProductOption()
        {
          var qry = new StringBuilder();
          qry.Append("UPDATE [productoption]");
          qry.Append(@"set name = @Name, description = @Description");
          qry.Append(" WHERE id = @0 ");

          UpdateData(qry);
        }
      #endregion Update
        
      #region Delete
        private void DeleteProductOption()
        {
          var qry = new StringBuilder();
          qry.Append("DELETE FROM [productoption]");
          qry.Append(" WHERE id = @0 ");

          DeleteData(qry);
        }
      #endregion Delete
    #endregion Data Portal

    #region internal Overrides
      internal override void SetFieldsReader(ref SqlDataReader rdr)
      {
        if (rdr == null)
          return;

        IsNew = false;
        Id = Guid.Parse(rdr["Id"].ToString());
        ProductId = Guid.Parse(rdr["ProductId"].ToString());
        Name = rdr["Name"].ToString();
        Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
      }

      internal override List<SqlParameter> PrepareCommandParameters()
      {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(new SqlParameter("@Id", Id));
        parameters.Add(new SqlParameter("@Name", Name));
        parameters.Add(new SqlParameter("@Description", Description));
        parameters.Add(new SqlParameter("@productid", ProductId));
        return parameters;
      }
    #endregion internal Overrides
    }
}