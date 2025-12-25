namespace FinalProject.Controllers;
using Domain;
using Services;

public class ProductController 
{
    private readonly InventoryService _inventory;
    public ProductController(InventoryService inventory) => _inventory = inventory;

    public List<Product> SearchByName(string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) return new List<Product>();
        return _inventory.SearchByName(term);
    }

    public List<Product> SearchById(int id)
    {
        if (id == 0) return new List<Product>();
        return _inventory.SearchById(id);
    }

    public List<Product> SearchByCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return new List<Product>();
        return _inventory.SearchByCategory(name);
    }

    public List<Product> SearchByPrice(decimal price)
    {
        if (price == 0) return new List<Product>();
        return _inventory.SearchByPrice(price);
    }

    public List<Product> GetAllProducts() => _inventory.GetAll();
}