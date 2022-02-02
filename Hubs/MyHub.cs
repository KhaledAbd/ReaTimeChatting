using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using task.app.Data;
using task.app.Dtos;
using task.app.Models;

namespace task.app.Hubs
{
    public partial class MyHub: Hub
    {
        // private readonly DataContext _context;
        // private readonly UserManager<User> userManager;
        // private readonly IMapper mapper;
        private readonly IDatingRepository repo;
        private readonly IMapper mapper;

        // public MyHub(DataContext _context, UserManager<User> userManager, IMapper mapper){
        //     this._context = _context;
        //     this.userManager = userManager;
        //     this.mapper = mapper;
        // }

        public MyHub(IDatingRepository _repo,  IMapper mapper){
            repo = _repo;
            this.mapper = mapper;
        }
        ///when disConnect to server.
        public override  Task OnDisconnectedAsync(Exception exception)
        {
            // int currUserId = _context.Connections.Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.UserId).SingleOrDefault();
            // _context.Connections.RemoveRange(_context.Connections.Where(p => p.UserId == currUserId).ToList());
            // _context.SaveChanges();
            
            Clients.Others.SendAsync("userOff", repo.RemoveConnectionRange(Context.ConnectionId));
            Console.Error.WriteLine("Disconnect ...");
            return base.OnDisconnectedAsync(exception);
        }
        ///create Connection.
        public async Task authMe(int userId)
        {
                // Connections currUser = new Connections
                // {
                //     UserId = personInfo.userId,
                //     SignalrId = Context.ConnectionId,
                //     TimeStamp = DateTime.Now
                // };
                // await _context.Connections.AddAsync(currUser);
                // await _context.SaveChangesAsync();
                Console.Error.WriteLine("Authenticate ....");
                var user = mapper.Map<UserForDetailedDto>(await repo.CreateMessageConnection(userId, Context.ConnectionId));
                await Clients.Caller.SendAsync("authMeResponseSuccess", user);
                await Clients.Others.SendAsync("userOn", user);
        }
        ///toReconnect
         public async Task reauthMe(int userId)
        {
            Console.Error.WriteLine("Reconnect Again..!!");
            User tempPerson = null;
            tempPerson = await repo.CreateMessageConnection(userId, Context.ConnectionId);
            var user = mapper.Map<UserForDetailedDto>(tempPerson);
            await Clients.Caller.SendAsync("reauthMeResponse", user);
            await Clients.Others.SendAsync("userOn", user);
            
        }
        ///For Logout
        public void logOut(int personId)
        {
            repo.RemoveConnectionRange(Context.ConnectionId);
            Console.Error.WriteLine("Logout ...!!");
            Clients.Caller.SendAsync("logoutResponse");
            Clients.Others.SendAsync("userOff", personId);
        }

    }
}