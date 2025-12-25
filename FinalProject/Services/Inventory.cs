namespace FinalProject.Services;
using FinalProject.Domain;

public class InventoryService
{
    private readonly List<Product> _products;

    public InventoryService(IProductRepository repository) 
    {
        _products = repository.GetAll();
    }

    public List<Product> GetAll()
    {
        return new List<Product>(_products);
    }

    public List<Product> SearchByName(string name)
    {
        return _products
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return _products
            .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchById(int id)
    {
        return _products
            .Where(p => p.Id == id).ToList();
    }

    public List<Product> SearchByPrice(decimal price)
    {
        return _products
            .Where(p => p.Price >= price && p.Price <= (price + 1)).ToList();
    }
}
