using Microsoft.AspNet.SignalR;
using WebPing;

namespace Service1.Areas.Monitoring.Hubs
{
    public class HealthHub: Hub
    {
        public void Report(HearthBeat beat)
        {
            //Do some aggregation for uptime
            //Notify all clients with summary
            Clients.All.report("1");
        }

        public void Echo(string message)
        {
            Clients.All.echo(message);
        }

    }
}
