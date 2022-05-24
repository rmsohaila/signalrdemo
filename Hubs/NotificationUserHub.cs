﻿using Microsoft.AspNetCore.SignalR;
using System;
using SignalRDemo.Interface;
using System.Threading.Tasks;

namespace SignalRDemo.Hubs
{
    public class NotificationUserHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationUserHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        
        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();

            var userId = httpContext.Request.Query["userId"];
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

            return Context.ConnectionId;
        }

        
        // On Hub terminated
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;

            _userConnectionManager.RemoveUserConnection(connectionId);

            //adding dump code to follow the template of Hub > OnDisconnectedAsync
            var value = await Task.FromResult(0);
        }
    }
}
