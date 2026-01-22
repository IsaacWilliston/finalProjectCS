namespace FinalProject.Domain.Filtering;

public interface IPredicate<in T> where T : Product
{
    bool Matches(T item);
}

public class Range<T> where T : struct, IComparable<T> 
{ 
    public T? Min { get; } 
    public T? Max { get; } 
 
    public Range(T? min = null, T? max = null)  
    { 
        if (min.HasValue && max.HasValue && min.Value.CompareTo(max.Value) > 0)  
            throw new ArgumentException("Min cannot be greater than Max"); 
        Min = min; Max = max;  
    } 
 
    public bool Contains(T value)  
    { 
        if (Min.HasValue && value.CompareTo(Min.Value) < 0) return false;  
        if (Max.HasValue && value.CompareTo(Max.Value) > 0) return false;  
        return true;  
    } 
}

public static class UniversalConditions 
{ 
    // Exact match for ID, Category, etc.
    public class ExactSearchCondition<T, TValue>(TValue target, Func<T, TValue> selector) 
        : IPredicate<T> where T : Product 
    { 
        public bool Matches(T item) => selector(item)!.Equals(target); 
    } 
 
    // Range match for Price
    public class RangeSearchCondition<T, TValue>(Range<TValue> range, Func<T, TValue> valueSelector) 
        : IPredicate<T> where T : Product where TValue : struct, IComparable<TValue> 
    { 
        public bool Matches(T item) => range.Contains(valueSelector(item)); 
    }

    // String "Contains" for Name search
    public class StringContainsCondition<T>(string term, Func<T, string> selector) 
        : IPredicate<T> where T : Product
    {
        public bool Matches(T item) => selector(item).Contains(term, StringComparison.OrdinalIgnoreCase);
    }
}