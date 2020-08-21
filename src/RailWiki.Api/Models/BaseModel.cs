namespace RailWiki.Api.Models.Entities
{
    public abstract class BaseModel : BaseModel<int>
    {

    }

    /// <summary>
    /// The base entity to define the key
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key</typeparam>
    public abstract class BaseModel<TKey>
    {
        public TKey Id { get; set; }
    }
}
