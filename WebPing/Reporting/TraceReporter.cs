using System.Diagnostics;
using System.Threading.Tasks;

namespace WebPing.Reporting
{
    /// <summary>
    /// Reports heart beat information to Diagnostics.Trace
    /// </summary>
    public class TraceReporter : IHearthBeatReporter
    {
        public void Report(HearthBeat beat)
        {
            Trace.WriteLine(beat);
        }

        public Task ReportAsync(HearthBeat beat)
        {
            return Task.Factory.StartNew(() => Report(beat));
        }
    }
}
