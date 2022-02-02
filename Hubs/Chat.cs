using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using task.app.Dtos;
using task.app.Models;

namespace task.app.Hubs
{
    public partial class MyHub
    {
        //get Online Users
        public async Task getOnlineUsers()
        {
            Console.Error.WriteLine("Get Online Users");
            var onlineUsers = mapper.Map<IEnumerable<UserForDetailedDto>>(await repo.GetOnlineUser(Context.ConnectionId));
            await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
        }

        //Send message
        public async Task sendMsg(string connId, int msg)
        {
            Console.Error.WriteLine("Message Sent:" + connId);
            var message = mapper.Map<MessageToReturnDto>(await repo.GetMessage(msg));

            await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, message);
        }

        public async Task readMsg(string connId, int msg){
            Console.Error.WriteLine("Message Read:" + connId );
            await repo.ReadMessage(msg);
            await Clients.Client(connId).SendAsync("readMsgResponse", Context.ConnectionId, msg);
        }
    }
}