namespace RailWiki.Shared.Entities
{
    public abstract class BaseEntity : BaseEntity<int>
    {

    }

    /// <summary>
    /// The base entity to define the key
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key</typeparam>
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
