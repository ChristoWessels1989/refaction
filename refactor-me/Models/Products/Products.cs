using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;

namespace refactor_me.Models
{
    public class Products : BusinessListBase<Products, Product>
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            FetchProducts(null);
        }

        public Products(string name)
        {
            FetchProducts(name.ToLower());
        }

        private void FetchProducts(string name)
        {
            var qry = new StringBuilder();
            qry.Append("SELECT * ");
            qry.Append(@"FROM [PRODUCT] ");
            if (name != null)
            {
                qry.Append(" where lower(name) like '%@0%' ");
                var whereParameters = new List<SqlParameter> { new SqlParameter("@0", name.ToLower()) };
                SelectData(qry, whereParameters);
            }else
            {
                SelectData(qry);
            }
        }

        #region internal Overrides
        internal override void SetReaderValues(ref SqlDataReader rdr)
        {
          
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new Product(id));
            }
        }
        #endregion internal Overrides
    }
}