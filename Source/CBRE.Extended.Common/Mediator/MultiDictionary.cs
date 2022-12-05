using System.Runtime.Serialization;

namespace CBRE.Extended.Common.Mediator;

/* http://www.codeproject.com/Articles/35277/MVVM-Mediator-Pattern */
/// <summary>
/// The multi dictionary is a dictionary that contains 
/// more than one value per key
/// </summary>
/// <typeparam name="TKey">The type of the key</typeparam>
/// <typeparam name="TValue">The type of the list contents</typeparam>
[Serializable]
public class MultiDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>
{
    public MultiDictionary()
    {
    }

    protected MultiDictionary(SerializationInfo Information, StreamingContext Context) : base(Information, Context)
    {
    }

    //checks if the key is already present
    private void EnsureKey(TKey Key)
    {
        if (!ContainsKey(Key)) this[Key] = new List<TValue>(1);
        else if (this[Key] == null) this[Key] = new List<TValue>(1);
    }

    /// <summary>
    /// Adds a new value in the Values collection
    /// </summary>
    /// <param name="Key">The key where to place the 
    /// item in the value list</param>
    /// <param name="NewItem">The new item to add</param>
    public void AddValue(TKey Key, TValue NewItem)
    {
        EnsureKey(Key);
        this[Key].Add(NewItem);
    }

    /// <summary>
    /// Adds a list of values to append to the value collection
    /// </summary>
    /// <param name="Key">The key where to place the item in the value list</param>
    /// <param name="NewItem">The new items to add</param>
    public void AddValues(TKey Key, IEnumerable<TValue> NewItem)
    {
        EnsureKey(Key);
        this[Key].AddRange(NewItem);
    }

    /// <summary>
    /// Removes a specific element from the dict
    /// If the value list is empty the key is removed from the dict
    /// </summary>
    /// <param name="Key">The key from where to remove the value</param>
    /// <param name="Value">The value to remove</param>
    /// <returns>Returns false if the key was not found</returns>
    public bool RemoveValue(TKey Key, TValue Value)
    {
        if (!ContainsKey(Key)) return false;
        this[Key].Remove(Value);
        if (this[Key].Count == 0) Remove(Key);
        return true;
    }

    /// <summary>
    /// Removes all items that match the prediacte
    /// If the value list is empty the key is removed from the dict
    /// </summary>
    /// <param name="Key">The key from where to remove the value</param>
    /// <param name="Match">The predicate to match the items</param>
    /// <returns>Returns false if the key was not found</returns>
    public bool RemoveAllValue(TKey Key, Predicate<TValue> Match)
    {
        if (!ContainsKey(Key)) return false;
        this[Key].RemoveAll(Match);
        if (this[Key].Count == 0) this.Remove(Key);
        return true;
    }
}