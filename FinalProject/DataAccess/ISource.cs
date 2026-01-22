namespace FinalProject.DataAccess;
using Domain;

public interface ISource<out T> : IEnumerable<T>, ICloneable where T : Product
{
    string FilePath();
}