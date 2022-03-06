using System.Threading.Tasks;

namespace Infrastructure.Queues
{
    public interface IStorageQueueClient
    {
        Task AddMessageAsync(UserDetailsCommand message);
    }
}