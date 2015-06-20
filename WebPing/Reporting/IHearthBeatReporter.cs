using System.Threading.Tasks;

namespace Dephr.Reporting
{
    /// <summary>
    /// Reports hearth beats for configured services.
    /// </summary>
    public interface IHearthBeatReporter
    {
        /// <summary>
        /// Report a hearth beat.
        /// </summary>
        /// <param name="beat">Beat to report.</param>
        void Report(HearthBeat beat);
        /// <summary>
        /// Report a hearth beat async.
        /// </summary>
        /// <param name="beat">Beat to report.</param>
        /// <returns></returns>
        Task ReportAsync(HearthBeat beat);
    }
}
