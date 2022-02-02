using System.Threading.Tasks;
using task.app.Dtos;

namespace task.app.Data
{
    public interface IDataHub
    {
        Task RemoveRange(int userId);

        Task CreateMessageConnection(PersonInfo personInfo);

        Task ReconnectAgain(int personInfo);
    }
}