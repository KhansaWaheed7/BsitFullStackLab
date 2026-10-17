namespace Model
{
    public class ProductModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        public string Category { get; set; } = "";

        public string Description { get; set; } = "";

        public bool IsActive { get; set; }
    }
}