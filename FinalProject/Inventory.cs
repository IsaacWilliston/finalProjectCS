namespace FinalProject;

class Inventory
{
    private readonly List<Product> _products;

    public Inventory(List<Product> products)
    {
        _products = products;
    }

    public List<Product> GetAll()
    {
        return _products;
    }

    public List<Product> SearchByName(string name)
    {
        return _products
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return _products
            .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
