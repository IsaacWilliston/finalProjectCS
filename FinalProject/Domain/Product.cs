namespace FinalProject.Domain;

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Category { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public Product(int id, string name, string category, decimal price, int quantity)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        Quantity = quantity;
    }
}