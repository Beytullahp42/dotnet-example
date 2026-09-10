using Microsoft.AspNetCore.SignalR;

namespace dotnet_example.Chat;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        Console.WriteLine("A user connected: " + Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("A user disconnected: " + Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}