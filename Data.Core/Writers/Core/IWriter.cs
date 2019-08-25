using System.Threading.Tasks;

namespace Data.Core.Writers.Core
{
    public interface IWriter<in TEntity>
    {
        Task Add(TEntity mapping);

        Task Update(TEntity mapping);

        Task SaveChanges();
    }
}
