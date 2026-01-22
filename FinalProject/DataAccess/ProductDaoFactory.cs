namespace FinalProject.DataAccess;
using FinalProject.Domain;

public interface IProductDaoFactory
{
    IProductDao<T> CreateProductDao<T>() where T : Product;
}

public class ProductDaoFactory : IProductDaoFactory
{
    private static readonly ProductDaoFactory _instance = new();
    public static ProductDaoFactory Instance => _instance;

    private readonly Dictionary<Type, object> _sources = new();

    private ProductDaoFactory() { }

    public void RegisterSource<T>(ISource<T> source) where T : Product 
        => _sources[typeof(T)] = source;

    public IProductDao<T> CreateProductDao<T>() where T : Product
    {
        if (_sources.TryGetValue(typeof(T), out var source))
            return new ProductDao<T>((ISource<T>)source);
        
        throw new DaoException($"No source registered for {typeof(T).Name}");
    }
}