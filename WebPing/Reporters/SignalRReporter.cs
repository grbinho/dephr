using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
    /// <summary>
    /// Creates SignalR hub and reports changes to that hub?
    /// </summary>
    public class SignalRReporter : IHearthBeatReporter
    {
        public void Report(HearthBeat beat)
        {
            throw new NotImplementedException();
        }

        public Task ReportAsync(HearthBeat beat)
        {
            throw new NotImplementedException();
        }
    }
}
