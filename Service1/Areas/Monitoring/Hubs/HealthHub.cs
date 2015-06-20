using Microsoft.AspNet.SignalR;

namespace Service1.Areas.Monitoring.Hubs
{
    public class HealthHub: Hub
    {
        public void Echo(string message)
        {
            Clients.All.echo(message);
        }

    }
}
