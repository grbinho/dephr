using System;
using System.Threading.Tasks;

namespace WebPing.Reporting
{
    class StatsdReporter : IHearthBeatReporter
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
