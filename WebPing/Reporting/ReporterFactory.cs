namespace WebPing.Reporting
{
    public static class ReporterFactory
    {
        public static IHearthBeatReporter CreateReporter(Reporters reporter)
        {
            switch (reporter)
            {
                case Reporters.Trace: return new TraceReporter();
                case Reporters.SignalR: return new SignalRReporter();
                //statsd
                //graphite
                default: return new TraceReporter();
            }
        }
    }
}
