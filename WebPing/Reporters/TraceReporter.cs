using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
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
