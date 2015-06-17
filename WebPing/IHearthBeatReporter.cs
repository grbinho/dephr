using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
    public interface IHearthBeatReporter
    {
        void Report(HearthBeat beat);

        Task ReportAsync(HearthBeat beat);
    }
}
