using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;

namespace refactor_me.Models
{
   public class ProductOptions :BusinessListBase<ProductOptions,ProductOption>
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            FetchProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            FetchProductOptions(productId);
        }

        private void FetchProductOptions(Guid? productId)
        {
            var qry = new StringBuilder();
            qry.Append("SELECT * ");
            qry.Append(@"FROM [productoption] ");

            if (productId != null)
            {
                qry.Append(" WHERE productId = @0 ");
                var whereParameters = new List<SqlParameter> { new SqlParameter("@0", productId) };
                SelectData(qry, whereParameters);
            }
            else
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
                Items.Add(new ProductOption(id));
            }
        }
        #endregion internal Overrides
    }

}