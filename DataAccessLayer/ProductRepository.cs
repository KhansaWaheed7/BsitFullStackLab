using Model;
using Npgsql;

namespace DataAccessLayer
{
    public class ProductRepository : IProductRepository
    {
        private const string Columns =
            "\"Id\", \"Name\", \"Price\", \"Category\", \"Description\", \"IsActive\"";

        public async Task<List<ProductModel>> GetAllAsync()
        {
            string sql = $"SELECT {Columns} FROM \"Products\" ORDER BY \"Id\"";

            var products = new List<ProductModel>();

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(Map(reader));
            }

            return products;
        }

        public async Task<ProductModel?> GetByIdAsync(long id)
        {
            string sql = $"SELECT {Columns} FROM \"Products\" WHERE \"Id\" = @id";

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);

            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return Map(reader);
            }

            return null;
        }

        public async Task CreateAsync(ProductModel product)
        {
            // "Id" is an identity column, so the database generates it.
            const string sql =
                "INSERT INTO \"Products\" (\"Name\", \"Price\", \"Category\", \"Description\", \"IsActive\") " +
                "VALUES (@name, @price, @category, @description, @isActive) " +
                "RETURNING \"Id\"";

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            AddProductParameters(cmd, product);

            object? newId = await cmd.ExecuteScalarAsync();

            if (newId != null && newId != DBNull.Value)
            {
                product.Id = Convert.ToInt64(newId);
            }
        }

        public async Task UpdateAsync(ProductModel product)
        {
            const string sql =
                "UPDATE \"Products\" SET " +
                "\"Name\" = @name, " +
                "\"Price\" = @price, " +
                "\"Category\" = @category, " +
                "\"Description\" = @description, " +
                "\"IsActive\" = @isActive " +
                "WHERE \"Id\" = @id";

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            AddProductParameters(cmd, product);
            cmd.Parameters.AddWithValue("id", product.Id);

            int rows = await cmd.ExecuteNonQueryAsync();

            if (rows == 0)
            {
                throw new InvalidOperationException("Product not found. It may have been deleted.");
            }
        }

        public async Task DeleteAsync(long id)
        {
            const string sql = "DELETE FROM \"Products\" WHERE \"Id\" = @id";

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<ProductModel>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllAsync();
            }

            // Escape LIKE wildcards so user input is treated as plain text
            string escaped = keyword.Trim()
                .Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");

            string sql =
                $"SELECT {Columns} FROM \"Products\" " +
                "WHERE \"Name\" ILIKE @kw OR \"Category\" ILIKE @kw OR \"Description\" ILIKE @kw " +
                "ORDER BY \"Id\"";

            var products = new List<ProductModel>();

            await using NpgsqlConnection connection = DBHelper.GetConnection();
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("kw", "%" + escaped + "%");

            await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(Map(reader));
            }

            return products;
        }

        private static void AddProductParameters(NpgsqlCommand cmd, ProductModel product)
        {
            cmd.Parameters.AddWithValue("name", product.Name);
            cmd.Parameters.AddWithValue("price", product.Price);
            cmd.Parameters.AddWithValue("category", product.Category);
            cmd.Parameters.AddWithValue("description", product.Description);
            cmd.Parameters.AddWithValue("isActive", product.IsActive);
        }

        // Column order must match the "Columns" constant above.
        private static ProductModel Map(NpgsqlDataReader reader)
        {
            return new ProductModel
            {
                Id = reader.GetInt64(0),
                Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                Price = reader.IsDBNull(2) ? 0L : reader.GetInt64(2),
                Category = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Description = reader.IsDBNull(4) ? "" : reader.GetString(4),
                IsActive = !reader.IsDBNull(5) && reader.GetBoolean(5)
            };
        }
    }
}