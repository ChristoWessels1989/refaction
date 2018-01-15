using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace refactor_me.Models
{
   public class ProductOptions :BusinessListBase<ProductOptions,ProductOption>
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}'");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
        }
    }

}