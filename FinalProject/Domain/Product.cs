namespace FinalProject.Domain;

public abstract class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Category { get; } // Added this
    public decimal Price { get; }
    public int Quantity { get; }

    protected Product(int id, string name, string category, decimal price, int quantity)
    {
        this.Id = id > 0 ? id : throw new ArgumentException("ID must be positive");
        this.Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be empty");
        this.Category = !string.IsNullOrWhiteSpace(category) ? category : throw new ArgumentException("Category cannot be empty");
        this.Price = price >= 0 ? price : throw new ArgumentException("Price cannot be negative");
        this.Quantity = quantity >= 0 ? quantity : throw new ArgumentException("Quantity cannot be negative");
    }
}

public class Tableware : Product
{
    public string Material { get; }

    private Tableware(Builder builder) 
        : base(builder.Id, builder.Name, builder.Category, builder.Price, builder.Quantity)
    {
        this.Material = builder.Material;
    }

    public class Builder
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public string Material { get; private set; }

        public Builder SetId(int id) { Id = id; return this; }
        public Builder SetName(string name) { Name = name; return this; }
        public Builder SetCategory(string category) { Category = category; return this; }
        public Builder SetPrice(decimal price) { Price = price; return this; }
        public Builder SetQuantity(int qty) { Quantity = qty; return this; }
        public Builder SetMaterial(string mat) { Material = mat; return this; }

        public Tableware Build() => new Tableware(this);
    }
}