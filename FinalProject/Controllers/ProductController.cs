using FinalProject.Domain;
using FinalProject.Domain.Filtering;
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
        decimal minPrice = price - 1;
        decimal maxPrice = price + 1;


        var searchRange = new Range<decimal>(minPrice, maxPrice);
        var condition = new UniversalConditions.RangeSearchCondition<Product, decimal>(searchRange, p => p.Price);

        return _inventory.Search(condition);
    }
}