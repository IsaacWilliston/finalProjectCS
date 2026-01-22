namespace FinalProject.Services;
using Domain;
using Domain.Filtering;

public class InventoryService
{
    private readonly List<Product> _products;

    public InventoryService(IProductRepository repository) 
    {
        _products = repository.GetAll();
    }

    // Fixes the 'GetAll' unresolved error
    public List<Product> GetAll() => new List<Product>(_products);

    // Fixes the 'GetAllSorted' unresolved error
    public List<Product> GetAllSorted(string sortBy, bool ascending = true)
    {
        var query = _products.AsEnumerable();
        query = sortBy.ToLower() switch
        {
            "id" => ascending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id),
            "price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
            _ => query
        };
        return query.ToList();
    }

    // The core method that makes the Predicate system work
    public List<Product> Search(IPredicate<Product> condition)
    {
        return _products.Where(condition.Matches).ToList();
    }
}