using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;


namespace refactor_me.Models
{
  public class Product :BusinessBase<Product>
  {
    #region Properties
      public Guid Id { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }

      public decimal Price { get; set; }

      public decimal DeliveryPrice { get; set; }
        
      [JsonIgnore]
      public bool IsNew { get; set; }

    #endregion Properties

    #region constructors
      public Product()
      {
          Id = Guid.NewGuid();
          IsNew = true;
      }

      public Product(Guid id)
      {
        FetchProduct(id);
      }
    #endregion constructor

    #region overrides
        public override void Save()
        {
          if (IsNew)
          {
            InsertProduct();
          }
          else
          {
            UpdateProduct();
          }
        }

        public override void Delete()
        {
          foreach (var option in new ProductOptions(Id).Items)
              option.Delete();

          DeleteProduct();
        }
    #endregion overrides

    #region Data Portal
      #region Select
        private void FetchProduct(Guid id)
        {
           var qry = new StringBuilder();
           qry.Append("SELECT * ");
          qry.Append(@"FROM [PRODUCT] ");
          qry.Append(" WHERE id = @0 ");
          var whereParameters = new List<SqlParameter> {new SqlParameter("@0", id)};

          SelectData(qry, whereParameters);
        }
      #endregion Select

      #region Insert
        private void InsertProduct()
        {
          var qry = new StringBuilder();
          qry.Append("INSERT INTO [PRODUCT]");
          qry.Append(@"( (id, name, description, price, deliveryprice)");
          qry.Append(" VALUES ");
          qry.Append(@" ('@Id', '@Name', '@Description', @Price, @DeliveryPrice)");

          InsertData(qry);
        }
      #endregion Insert

      #region Update
        private void UpdateProduct()
        {
          var qry = new StringBuilder();
          qry.Append("UPDATE [product]");
          qry.Append(@"set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice");
          qry.Append(" WHERE id = @0 ");

          UpdateData(qry);
        }
      #endregion Update
        
      #region Delete
        private void DeleteProduct()
        {
          var qry = new StringBuilder();
          qry.Append("DELETE FROM [product]");
          qry.Append(" WHERE id = @0 ");

          DeleteData(qry);
        }
      #endregion Delete
    #endregion Data Portal

    #region internal Overrides
      internal override void SetFieldsReader(SqlDataReader rdr)
      {
        if (rdr == null)
          return;

        IsNew = false;
        Id = Guid.Parse(rdr["Id"].ToString());
        Name = rdr["Name"].ToString();
        Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        Price = decimal.Parse(rdr["Price"].ToString());
        DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
      }

      internal override List<SqlParameter> PrepareCommandParameters()
      {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(new SqlParameter("@Id", Id));
        parameters.Add(new SqlParameter("@Name", Name));
        parameters.Add(new SqlParameter("@Description", Description));
        parameters.Add(new SqlParameter("@Price", Price));
        parameters.Add(new SqlParameter("@DeliveryPrice", DeliveryPrice));
        return parameters;
      }
    #endregion internal Overrides

  }
}