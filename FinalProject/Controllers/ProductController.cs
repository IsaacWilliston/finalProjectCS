using System.Collections.Generic; // Fixes unresolved 'List' if necessary
using FinalProject.Domain;
using FinalProject.Domain.Filtering; // Fixes 'UniversalConditions' and 'IPredicate'
using FinalProject.Services;

namespace FinalProject.Controllers;

public class ProductController 
{
    private readonly InventoryService _inventory;
    public ProductController(InventoryService inventory) => _inventory = inventory;

    public List<Product> GetAllProducts() => _inventory.GetAll();

    public List<Product> GetAllSorted(string sortBy, bool ascending = true) 
        => _inventory.GetAllSorted(sortBy, ascending);

    public List<Product> SearchById(int id) 
    {
        var condition = new UniversalConditions.ExactSearchCondition<Product, int>(id, p => p.Id);
        return _inventory.Search(condition);
    }

    public List<Product> SearchByName(string name)
    {
        var condition = new UniversalConditions.StringContainsCondition<Product>(name, p => p.Name);
        return _inventory.Search(condition);
    }

    public List<Product> SearchByCategory(string category)
    {
        var condition = new UniversalConditions.ExactSearchCondition<Product, string>(category, p => p.Category);
        return _inventory.Search(condition);
    }

    public List<Product> SearchByPrice(decimal price)
    {
        // For exact price search as used in your ConsoleView
        var condition = new UniversalConditions.ExactSearchCondition<Product, decimal>(price, p => p.Price);
        return _inventory.Search(condition);
    }
}