using System.Collections.Generic;
using System.Threading.Tasks;
using task.app.Helpers;
using task.app.Models;
using task.app.Helper;
using task.app.Dtos;

namespace task.app.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<IEnumerable<User>> GetUserThread(User user);
        Task<User> GetUser(int id, bool isCurrentUser);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
        int RemoveConnectionRange(string connectionId);
        Task<User> CreateMessageConnection(int userId, string connectionId);
        Task<IEnumerable<User>> GetOnlineUser(string connectionId);
        Task<bool> ReadMessage(int messageId);
    }
}