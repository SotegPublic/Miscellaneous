using System.Threading.Tasks;

namespace Equipment
{
    public interface IItemFactory<T,T2> where T: ItemModel where T2: IItemConfigurator
    {
        Task<T> CreateItemAsync(T2 configurator);
    }
}