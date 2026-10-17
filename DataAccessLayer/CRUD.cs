using Model;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class CRUD
    {
        public static List<ProductModel> GetAllData()
        {
            string query = "SELECT * FROM \"Products\"";
            List<ProductModel> products = new List<ProductModel>();

            using (NpgsqlConnection connection = DBHelper.GetConnection())
            {
                connection.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductModel model = new ProductModel();

                        model.Id = reader.GetInt64(reader.GetOrdinal("Id"));
                        model.Name = reader["Name"].ToString()!;
                        model.Price = Convert.ToDecimal(reader["Price"]);

                        products.Add(model);
                    }
                }
            }

            return products;
        }
    }
}