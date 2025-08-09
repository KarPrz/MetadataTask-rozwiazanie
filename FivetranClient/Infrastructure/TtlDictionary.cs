namespace FivetranClient.Infrastructure;

public class TtlDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue, DateTime)> _dictionary = new();


    // Our custom method for getting value should not be called on _dictionary as it already process this dict inside itself.
    // Additionally, first returned object is wrong type and if-condition is redundant, as it is checked inside our custom TryGetValue
    //public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, TimeSpan ttl)
    //{
    //    if (_dictionary.TryGetValue(key, out var entry))
    //    {
    //        if (DateTime.UtcNow < entry.Item2)
    //        {
    //            return entry.Item1;
    //        }

    //        _dictionary.Remove(key);
    //    }

    //    var value = valueFactory();
    //    _dictionary[key] = (value, DateTime.UtcNow.Add(ttl));
    //    return value;
    //}

    // The name of this method could be confusing as it is the same as name for generic dict method.
    //public bool TryGetValue(TKey key, out TValue value)
    //{
    //    if (_dictionary.TryGetValue(key, out var entry) && DateTime.UtcNow < entry.Item2)
    //    {
    //        value = entry.Item1;
    //        return true;
    //    }

    //    value = default!;
    //    return false;
    //}


    // New methods:
    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, TimeSpan ttl)
    {
        if (CustomTryGetValue(key, out TValue value))
            return value;



        _dictionary.Remove(key);

        TValue value = valueFactory();
        _dictionary[key] = (value, DateTime.UtcNow.Add(ttl));
        return value;
    }

    public bool CustomTryGetValue(TKey key, out TValue value)
    {
        if (_dictionary.TryGetValue(key, out var entry) && DateTime.UtcNow < entry.Item2)
        {
            value = entry.Item1;
            return true;
        }

        value = default!;
        return false;
    }
}