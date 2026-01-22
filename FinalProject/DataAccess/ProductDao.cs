namespace FinalProject.DataAccess;
using Domain;
using Domain.Filtering;

public class DaoException(string message, Exception? inner = null) : Exception(message, inner);

public interface IProductDao<T> where T : Product
{
    IList<T> FindAll();
    IList<T> Find(IPredicate<T> predicate);
}

public class ProductDao<T>(ISource<T> source) : IProductDao<T> where T : Product
{
    public IList<T> FindAll() => source.ToList();

    public IList<T> Find(IPredicate<T> predicate) 
        => source.Where(predicate.Matches).ToList();
}


