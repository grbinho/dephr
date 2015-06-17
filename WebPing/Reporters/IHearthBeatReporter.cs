using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
    /*
        Use SignalR for reporting?
        How does this communication work?

        SinalR sounds reasonable enough for receiving messages and passing them trough
    */
    public interface IHearthBeatReporter
    {
        void Report(HearthBeat beat);

        Task ReportAsync(HearthBeat beat);
    }

   

}
