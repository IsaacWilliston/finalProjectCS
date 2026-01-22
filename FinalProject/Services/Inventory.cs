namespace FinalProject.Services;
using Domain;
using Domain.Filtering;
using DataAccess;

public class InventoryService
{
    private readonly IProductDao<Product> _productDao;

    public InventoryService(IProductDaoFactory factory)
    {
        _productDao = factory.CreateProductDao<Product>();
    }


    public List<Product> GetAll()
    {
        try
        {
            return _productDao.FindAll().ToList();
        }
        catch (DaoException ex)
        {
            throw new Exception("Service Error: Load failed.", ex);
        }
    }


    public List<Product> GetAllSorted(string sortBy, bool ascending = true)
    {
        try
        {
            var products = _productDao.FindAll();
        
            IEnumerable<Product> query = sortBy.ToLower() switch
            {
                "id"       => ascending ? products.OrderBy(p => p.Id) : products.OrderByDescending(p => p.Id),
                "name"     => ascending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name),
                "category" => ascending ? products.OrderBy(p => p.Category) : products.OrderByDescending(p => p.Category),
                "price"    => ascending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price),
                "quantity" => ascending ? products.OrderBy(p => p.Quantity) : products.OrderByDescending(p => p.Quantity),
                _          => products
            };

            return query.ToList();
        }
        catch (DaoException ex)
        {
            throw new Exception("Service Error: Sort failed.", ex);
        }
    }

    public List<Product> Search(IPredicate<Product> condition)
    {
        try
        {
            return _productDao.Find(condition).ToList();
        }
        catch (DaoException ex)
        {
            throw new Exception("Service Error: Search failed.", ex);
        }
    }
}