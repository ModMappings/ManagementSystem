using System.Threading.Tasks;

namespace Data.Core.Writers.Core
{
    public interface IChangeCommitter
    {
        Task SaveChanges();
    }
}