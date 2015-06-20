using Microsoft.AspNet.SignalR;
using Service1.Areas.Monitoring.Hubs;
using Service1.Areas.Monitoring.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebPing;
using WebPing.Reporting;
using System.Linq;

namespace Service1.Areas.Monitoring
{
    public class SignalRReporter : IHearthBeatReporter
    {
        private readonly IHubContext _healthHub;
        private readonly ICollection<HealthSummary> _serviceSummaryList;

        public SignalRReporter()
        {
            _healthHub = GlobalHost.ConnectionManager.GetHubContext<HealthHub>();
            _serviceSummaryList = new Collection<HealthSummary>();
        }

        public void Report(HearthBeat beat)
        {
            addBeatToSummary(beat);
            _healthHub.Clients.All.report(_serviceSummaryList);
        }

        private void addBeatToSummary(HearthBeat beat)
        {
            var summary = _serviceSummaryList.FirstOrDefault(s => s.Name.Equals(beat.ServiceName, StringComparison.InvariantCultureIgnoreCase));

            if(summary == null)
            {
                summary = new HealthSummary(beat.ServiceName);
                _serviceSummaryList.Add(summary);
            }

            summary.AddHearthBeat(beat);
        }

        public Task ReportAsync(HearthBeat beat)
        {
            throw new NotImplementedException();
        }
    }
}
