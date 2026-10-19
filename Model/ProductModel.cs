namespace Model
{
    public class ProductModel
    {
        // int8 identity column in Supabase
        public long Id { get; set; }

        public string Name { get; set; } = "";

        // The Supabase column is int8 (bigint), so whole numbers only
        public long Price { get; set; }

        public string Category { get; set; } = "";

        public string Description { get; set; } = "";

        public bool IsActive { get; set; }
    }
}