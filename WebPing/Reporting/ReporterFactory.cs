namespace Dephr.Reporting
{
    public static class ReporterFactory
    {
        public static IHearthBeatReporter CreateReporter(Reporters reporter)
        {
            switch (reporter)
            {
                case Reporters.Trace: return new TraceReporter();
                //statsd
                //graphite
                default: return new TraceReporter();
            }
        }
    }
}
