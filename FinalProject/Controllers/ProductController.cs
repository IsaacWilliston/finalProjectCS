namespace FinalProject.Controllers;
using Domain;
using Services;

public class ProductController {
    private readonly InventoryService _inventory;
    public ProductController(InventoryService inventory) => _inventory = inventory;

    public List<Product> GetAllProducts() => _inventory.GetAll();
    public List<Product> SearchByName(string term) => _inventory.SearchByName(term);
    public List<Product> SearchByCategory(string term) => _inventory.SearchByCategory(term);
    public List<Product> SearchById(int id) => _inventory.SearchById(id);
    public List<Product> SearchByPrice(decimal price) => _inventory.SearchByPrice(price);
}