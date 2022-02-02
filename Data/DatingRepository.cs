using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task.app.Helpers;
using task.app.Models;
using Microsoft.EntityFrameworkCore;
using task.app.Helper;
using task.app.Dtos;

namespace task.app.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                        && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId 
                        && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false 
                    && m.SenderId == recipientId 
                    || m.RecipientId == recipientId && m.SenderId == userId 
                    && m.SenderDeleted == false)
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<User>> GetUserThread(User user)
        {
            return await _context.Messages.Include(m => m.Sender).Where(m => m.IsRead == false && m.RecipientId == user.Id).Select(u => u.Sender).Distinct().ToListAsync<User>();
        }

        public  int RemoveConnectionRange(string connectionId)
        {
            int currUserId = _context.Connections.Where(c => c.SignalrId == connectionId).Select(c => c.UserId).SingleOrDefault();
            _context.Connections.RemoveRange(_context.Connections.Where(p => p.UserId == currUserId).ToList());
            _context.SaveChangesAsync();
            return currUserId;
       }

        public async Task<User> CreateMessageConnection(int userId, string connectionId)
        {
            if((await _context.Connections.FirstOrDefaultAsync(c => c.UserId == userId)) == null){
                Connections currUser = new Connections
                {
                    UserId = userId,
                    SignalrId = connectionId,
                    TimeStamp = DateTime.Now
                };
                await _context.Connections.AddAsync(currUser);
                await _context.SaveChangesAsync();
            }
            return await this.GetUser(userId, false);

        }

        public async Task<IEnumerable<User>> GetOnlineUser(string connectionId)
        {
            Console.Error.WriteLine(connectionId);
            // int currUserId = _context.Connections.Where(c => c.SignalrId == connectionId).Select(c => c.UserId).SingleOrDefault();
            return await _context.Users.Include(p => p.ConnectionNavigation).Where(c => !c.ConnectionNavigation.SignalrId.Equals(connectionId)).ToListAsync();
        }

        public async Task<bool> ReadMessage(int messageId)
        {
            Message message = null;
            var isRead = false;
            try{
                if((message = await _context.Messages.FirstOrDefaultAsync(d => d.Id >= messageId && d.IsRead == false)) != null){
                    message.IsRead = true;
                    await _context.SaveChangesAsync();
                    isRead = true;
                }
            }catch(Exception e){
                Console.WriteLine(e.StackTrace);
            }
            return isRead;
        }
    }
}